using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class AStarPathFinder 
{
    private List<HexModel> _path, _open, _closed;
    private Dictionary<HexModel, float> _heuristicCosts, _costsFromStart;
    private Dictionary<HexModel, HexPathFinderNode> _nodes;
    public AStarPathFinder()
    {
        _path = new List<HexModel>();
        _open = new List<HexModel>();
        _closed = new List<HexModel>();
        _heuristicCosts = new Dictionary<HexModel, float>();
        _costsFromStart = new Dictionary<HexModel, float>();
        _nodes = new Dictionary<HexModel, HexPathFinderNode>();
    }
    public List<HexModel> FindPath(Func<HexModel, HexModel, float> hexToHexCost, HexModel start, HexModel end)
    {

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

                    var costFromStart = hexToHexCost(current, n) + costsFromStart[current];

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

                    float newCost = costsFromStart[current] + hexToHexCost(current, n);

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
