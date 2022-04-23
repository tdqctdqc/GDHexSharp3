using Godot;
using System;
using System.Collections.Generic;

public class RiverLogic : Node
{
    public RiverLogic(Logic logic)
    {
    }
    public List<RiverModel> BuildRiverPath(List<HexModel> path, int riverTypeID)
    {
        var result = new List<RiverModel>();
        var add = new List<RiverModel>();
        var update = new List<RiverModel>();
        for (int i = 0; i < path.Count - 1; i += 2)
        {
            var h1 = path[i];
            var h2 = path[i+1];
            var model = RiverGenerator.BuildRiver(riverTypeID, h1.ID, h2.ID);

            result.Add(model);
        }
        return result; 
    }
    public void PushRiversToServer(List<RiverModel> path)
    {
        var update = new List<RiverModel>();
        var updateHexPairIDs = new List<int>();
        var delete = new List<RiverModel>();
        var add = new List<RiverModel>();
        foreach (var r in path)
        {
            if(updateHexPairIDs.Contains(r.HexPairID)) continue; 
            updateHexPairIDs.Add(r.HexPairID);

            var old = CacheManager.Rivers.GetRiverByHexPairID(r.HexPairID);
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
