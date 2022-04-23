using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class RoadLogic 
{
    private List<RoadModel> _roadBuildQueueForTurn;
    public RoadLogic(Logic logic)
    {
        _roadBuildQueueForTurn = new List<RoadModel>();
    }
    public List<RoadModel> BuildRoadPath(List<HexModel> path, int roadTypeID)
    {
        var result = new List<RoadModel>();
        var add = new List<RoadModel>();
        var update = new List<RoadModel>();
        for (int i = 0; i < path.Count - 1; i++)
        {
            var h1 = path[i];
            var h2 = path[i+1];
            var model = RoadGenerator.BuildRoad(roadTypeID, h1.ID, h2.ID);
            result.Add(model);
        }

        return result; 
    }
    public RoadModel BuildRoad(HexModel h1, HexModel h2, int roadTypeID)
    {
        var model = RoadGenerator.BuildRoad(roadTypeID, h1.ID, h2.ID);
        _roadBuildQueueForTurn.Add(model);
        return model;
    }
    public void AddRoadsToBuildQueue(List<RoadModel> roads)
    {
        _roadBuildQueueForTurn.AddRange(roads);
    }
    public void EndOfTurnBuildRoads()
    {
        PushRoadsToServer(_roadBuildQueueForTurn);
        _roadBuildQueueForTurn = new List<RoadModel>();
    }
    public void PushRoadsToServer(List<RoadModel> path)
    {
        var update = new List<RoadModel>();
        var updateHexPairIDs = new List<int>();
        var delete = new List<RoadModel>();
        var add = new List<RoadModel>();
        foreach (var r in path)
        {
            if(updateHexPairIDs.Contains(r.HexPairID)) continue; 
            updateHexPairIDs.Add(r.HexPairID);

            var old = CacheManager.Roads.GetRoadByHexPairID(r.HexPairID);
            if(old != null)
            {
                delete.Add(old);
            }
            add.Add(r);
        }
        Game.I.Session.Server.StateInterface.DeleteModels(delete);
        Game.I.Session.Server.StateInterface.AddModels(add);
    }
}
