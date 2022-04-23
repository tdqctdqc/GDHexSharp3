using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;
using Priority_Queue; 

public class HexAStarPathFinder : Node
{
    public List<HexModel> FindPath(UnitModel unit, HexModel start, HexModel end)
    {
        var moveType = unit.UnitType.MoveType;
        var path = new List<HexModel>();
        var open = new List<HexModel>();
        var closed = new List<HexModel>();
        var heuristicCosts = new Dictionary<HexModel, float>();
        var costsFromStart = new Dictionary<HexModel, float>();
        var nodes = new Dictionary<HexModel, HexPathFinderNode>();

        //add start node to open
        var startNode = new HexPathFinderNode(start);
        open.Add(start);
        costsFromStart.Add(start, 0f);
        heuristicCosts.Add(start, start.WorldPos.DistanceTo(end.WorldPos));

        nodes.Add(start, new HexPathFinderNode(start));

        while(open.Count > 0)
        {
            HexModel current = open[0];
            if(current == end)
            {
                return BuildPathBackwards(nodes[current]);
            }

            open.Remove(current);
            open.OrderBy(h => costsFromStart[h] + heuristicCosts[h]);
            closed.Add(current);

            var neighbors = current.GetNeighbors();

            foreach (var n in neighbors)
            {
                if(closed.Contains(n)) continue; 

                if(open.Contains(n) == false)
                {
                    var nNode = new HexPathFinderNode(n);
                    var costFromStart = GetTotalEdgeCost(unit, current, n) + costsFromStart[current];

                    if(float.IsInfinity(costFromStart) == false)
                    {
                        nodes.Add(n, nNode);
                        nNode.Parent = nodes[current];
                        heuristicCosts.Add(n, n.WorldPos.DistanceTo(end.WorldPos));
                        costsFromStart.Add(n, costFromStart);
                        open.Add(n); 
                    }
                    open.OrderBy(h => costsFromStart[h] + heuristicCosts[h]);
                }
                else
                {
                    float newCost = costsFromStart[current] + GetTotalEdgeCost(unit, current, n);
                    float oldCost = costsFromStart[n];
                    if(newCost < oldCost)
                    {
                        nodes[n].Parent = nodes[current];
                        costsFromStart[n] = newCost;
                    }
                }
            }
        }
        return null; 
    }
    public List<HexModel> FindRadius(UnitModel unit, HexModel start, float maxCost)
    {
        var moveType = unit.UnitType.MoveType;
        var open = new List<HexModel>();
        var closed = new List<HexModel>();
        var costsFromStart = new Dictionary<HexModel, float>();

        //add start node to open
        open.Add(start);
        costsFromStart.Add(start, 0f);

        while(open.Count > 0)
        {
            HexModel current = open[0];

            open.Remove(current);
            //open.OrderBy(h => costsFromStart[h]);
            closed.Add(current);

            var neighbors = current.GetNeighbors();

            foreach (var n in neighbors)
            {
                if(closed.Contains(n)) continue; 

                if(costsFromStart.ContainsKey(n) == false)
                {
                    var costFromStart = GetTotalEdgeCost(unit, current, n) + costsFromStart[current];
                    costsFromStart.Add(n, costFromStart);

                    if(costFromStart <= maxCost)
                    {
                        open.Add(n); 
                    }
                    //open.OrderBy(h => costsFromStart[h]);
                }
                else
                {
                    float oldCost = costsFromStart[n];
                    if(oldCost <= maxCost) continue; 
                    float newCost = costsFromStart[current] + GetTotalEdgeCost(unit, current, n);
                    if(newCost <= maxCost)
                    {
                        costsFromStart[n] = newCost;
                        open.Add(n);
                        open.OrderBy(h => costsFromStart[h]);
                    }
                }
            }
        }
        return closed; 
    }

    private float GetTotalEdgeCost(UnitModel unit, HexModel from, HexModel to)
    {
        if(to.Units.Count >= Constants.MaxUnitsInHex) return Mathf.Inf; 
        var moveType = unit.UnitType.MoveType;
        var road = from.GetRoadToHex(to);
        var hostileMult = 1f;
        if(to.FactionID != unit.FactionID)
        {
            hostileMult = Constants.HostileFactionTerritoryMoveCostMult;
        }
        var river = from.GetRiverToHex(to);
        float riverCost = 0f;
        if(river != null) riverCost = moveType.RiverCosts[river.RiverType];


        float terrainCost = (riverCost + moveType.TerrainCosts[to.Terrain]) * hostileMult;
        if(road != null)
        {
            float roadCost = moveType.RoadCosts[road.RoadType] * hostileMult;
            if(roadCost < terrainCost) return roadCost; 
        }
        return terrainCost;
    }
    private List<HexModel> BuildPathBackwards(HexPathFinderNode endNode)
    {
        var path = new List<HexModel>();
        var current = endNode;
        while(current.Parent != null)
        {
            path.Add(current.Hex);
            current = current.Parent;
        }
        path.Add(current.Hex);
        path.Reverse();
        return path;
    }
}
