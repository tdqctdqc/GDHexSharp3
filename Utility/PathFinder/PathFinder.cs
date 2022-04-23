using Godot;
using HexWargame;
using System;
using System.Linq;
using System.Collections.Generic;

public class PathFinder 
{
    private HexAStarPathFinder _hexAStar; 
    private AStarPathFinder _aStar;
    private HexEdgePathFinder _edgeAStar;
    public PathFinder()
    {
        _hexAStar = new HexAStarPathFinder();
        _aStar = new AStarPathFinder();
        _edgeAStar = new HexEdgePathFinder();
    }
    public float HexHeuristicFunc(HexModel h1, HexModel h2)
    {
        return h1.WorldPos.DistanceTo(h2.WorldPos) / (Constants.HexRadius * 2f);
    }
    public List<HexModel> HexNeighborFunc(HexModel h)
    {
        return h.Neighbors;
    }
    public float EdgeHeuristicFunc(int e1, int e2)
    {
        return e1.GetPosForPairID().DistanceTo(e2.GetPosForPairID()) / (Constants.HexRadius * 2f);
    }
    public List<int> EdgeNeighborFunc(int e)
    {
        return e.GetEdgeNeighbors();
    }
    public List<HexModel> FindUnitPath(UnitModel unit, HexModel start, HexModel end)
    {
        Func<HexModel,HexModel,float> cost = (h,i) => GetTotalEdgeCostForUnit(h, i, unit);
        return GenericPathFinder.FindPath<HexModel>(cost, HexNeighborFunc, HexHeuristicFunc, start, end);
    }
    public List<HexModel> FindPath(HexModel start, HexModel end, MovementType moveType)
    {
        Func<HexModel, HexModel, float> cost = (h,i) => h.GetHexEdgeCost(i, moveType);
        return GenericPathFinder.FindPath<HexModel>(cost, HexNeighborFunc, HexHeuristicFunc, start, end);
    }
    public List<HexModel> FindPath(HexModel start, HexModel end, Func<HexModel,HexModel,float> costFunc)
    {
        return GenericPathFinder.FindPath<HexModel>(costFunc, HexNeighborFunc, HexHeuristicFunc, start, end);
    }
    public List<HexModel> FindBuildRoadPath(RoadType roadType, HexModel start, HexModel end)
    {
        Func<HexModel, HexModel, float> buildCost = (h,i) =>
        {
            var road = h.GetRoadToHex(i);
            if(road != null)
            {
                if(road.RoadType.Quality >= roadType.Quality)
                {
                    return 0f;
                }
            }
            return roadType.GetBuildCost(h, i);
        };
        return GenericPathFinder.FindPath<HexModel>(buildCost, HexNeighborFunc, HexHeuristicFunc, start, end);
    }
    public float FindPathCost(List<HexModel> path, UnitModel unit)
    {
        Func<HexModel, HexModel, float> costFunc = (h,i) => GetTotalEdgeCostForUnit(h, i, unit);
        float result = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            result += costFunc(path[i], path[i + 1]);
        }
        return result; 
    }
    public float FindPathCost(List<HexModel> path, MovementType moveType)
    {
        Func<HexModel, HexModel, float> costFunc = (h,i) => h.GetHexEdgeCost(i, moveType);
        float result = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            result += costFunc(path[i], path[i + 1]);
        }
        return result; 
    }
    public float FindPathCost(List<HexModel> path, Func<HexModel, HexModel, float> costFunc)
    {
        float result = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            result += costFunc(path[i], path[i + 1]);
        }
        return result; 
    }
    public List<HexModel> FindShortestPathHexwise(HexModel start, HexModel end)
    {
        Func<HexModel, HexModel, float> cost = (h1, h2) =>
        {
            if(h1.GetHexDistance(h2) == 1) return 1f;
            else return Mathf.Inf;
        };
        return _aStar.FindPath(cost, start, end);
    }
    public List<int> FindShortestPathEdgewise(int start, int end)
    {
        Func<int, int, float> cost = (e1, e2) =>
        {
            if(e1.HexPairsTouch(e2)) return 1f;
            else return Mathf.Inf;
        };
        
        return _edgeAStar.FindPath(cost, start, end);
    }
    public List<int> FindEdgePath(int start, int end, Func<int,int,float> cost)
    {
        return _edgeAStar.FindPath(cost, start, end);
    }
    public List<HexModel> FindMovementRadius(UnitModel unit)
    {
        float maxCost = TurnManager.APPerRound * TurnManager.NumRounds;
        return _hexAStar.FindRadius(unit, unit.Hex, maxCost * unit.ReadinessAPMultiplier);
    }
    public float GetTotalEdgeCostForUnit(HexModel from, HexModel to, UnitModel unit)
    {
        if(to.Units.Count >= Constants.MaxUnitsInHex) return Mathf.Inf; 
        var moveType = unit.UnitType.MoveType;

        var hostileMult = 1f;
        if(to.FactionID != unit.FactionID)
        {
            hostileMult = Constants.HostileFactionTerritoryMoveCostMult;
        }

        var river = from.GetRiverToHex(to);
        float riverCost = 0f;
        if(river != null) riverCost = moveType.RiverCosts[river.RiverType];

        var terrainCost = moveType.TerrainCosts[to.Terrain];
        float nonRoadCost = (riverCost + terrainCost) * hostileMult;

        var road = from.GetRoadToHex(to);
        if(road != null)
        {
            float roadCost = moveType.RoadCosts[road.RoadType] * hostileMult;
            if(roadCost < nonRoadCost) 
            {
                //GD.Print($"using road {from.Coords} to {to.Coords}");
                return roadCost; 
            }
        }
        return nonRoadCost;
    }
}
