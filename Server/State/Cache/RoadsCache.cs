using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class RoadsCache
{
    public RoadModel this[int i] => GetRoadByHexPairID(i);

    private Dictionary<int, RoadModel> _roadsByHexPairID;
    private Dictionary<int, RoadModel> _roadsByID;
    private StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    public RoadsCache()
    {
        _roadsByHexPairID = new Dictionary<int, RoadModel>();
        _roadsByID = new Dictionary<int, RoadModel>();
        Cache<RoadModel>.ModelsAdded += AddRoads; 
        Cache<RoadModel>.ModelsDeleted += RemoveRoads;
        CacheManager.LoadedState += SetupRoads;
    }
    public void Reset()
    {
        _roadsByHexPairID = new Dictionary<int, RoadModel>();
        _roadsByID = new Dictionary<int, RoadModel>();
    }
    public RoadModel GetRoadByHexPairID(int hexPair)
    {
        if(_roadsByHexPairID.ContainsKey(hexPair)) return _roadsByHexPairID[hexPair];
        return null;
    }
    public RoadModel GetRoadByHexes(HexModel h1, HexModel h2)
    {
        if(h1.ID == h2.ID) return null;

        int id = h1.GetHexPairIndex(h2);

        if(_roadsByHexPairID.ContainsKey(id)) return _roadsByHexPairID[id];
        return null;
    }
    public RoadModel GetRoadByHexes(int h1, int h2)
    {
        if(h1 == h2) return null;
        
        var hex1 = Cache<HexModel>.GetModel(h1);
        var hex2 = Cache<HexModel>.GetModel(h2);

        int id = hex1.GetHexPairIndex(hex2);

        if(_roadsByHexPairID.ContainsKey(id)) return _roadsByHexPairID[id];
        return null;
    }
    public void SetupRoads()
    {
        var models = Cache<RoadModel>.GetModels();
        AddRoads(models);
    }
    public void AddRoads(List<RoadModel> models)
    {
        foreach (var model in models)
        {
            if(_roadsByHexPairID.ContainsKey(model.HexPairID))
            {
                var oldRoad = _roadsByHexPairID[model.HexPairID];
                _roadsByID.Remove(oldRoad.ID);
                _roadsByID.Add(model.ID, model);
                _roadsByHexPairID[model.HexPairID] = model;
            }
            else
            {
                _roadsByHexPairID.Add(model.HexPairID, model);
                _roadsByID.Add(model.ID, model);
            }
        }
    }
    public void AddRoadsHot(List<RoadModel> models)
    {
        int index = _roadsByID.Count;
        for (int i = 0; i < models.Count; i++)
        {
            models[i].ID = i + index + 1; 
        }
        foreach (var model in models)
        {
            if(_roadsByID.ContainsKey(model.ID)) continue; 

            if(_roadsByHexPairID.ContainsKey(model.HexPairID) )
            {
                var oldRoad = _roadsByHexPairID[model.HexPairID];
                _roadsByID.Remove(oldRoad.ID);
                _roadsByID.Add(model.ID, model);
                _roadsByHexPairID[model.HexPairID] = model;
            }
            else
            {
                _roadsByHexPairID.Add(model.HexPairID, model);
                _roadsByID.Add(model.ID, model);
            }
        }
    }

    public void RemoveRoads(List<int> ids)
    {
        foreach (var id in ids)
        {
            if(_roadsByID.ContainsKey(id))
            {
                var road = _roadsByID[id];
                int hexPair = road.HexPairID;
                _roadsByHexPairID.Remove(hexPair);
                _roadsByID.Remove(id);
            }
        }
    }
}
