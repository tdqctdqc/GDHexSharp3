using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class RiversCache
{
    public RiverModel this[int i] => GetRiverByHexPairID(i);
    private Dictionary<int, RiverModel> _riversByHexPairID;
    private Dictionary<int, RiverModel> _riversByID;
    private StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    public RiversCache()
    {
        _riversByHexPairID = new Dictionary<int, RiverModel>();
        _riversByID = new Dictionary<int, RiverModel>();
        Cache<RiverModel>.ModelsAdded += AddRivers; 
        Cache<RiverModel>.ModelsDeleted += RemoveRivers;
        CacheManager.LoadedState += SetupRivers;
    }
    public RiverModel GetRiverByHexPairID(int hexPair)
    {
        if(_riversByHexPairID.ContainsKey(hexPair)) return _riversByHexPairID[hexPair];
        return null;
    }
    public RiverModel GetRiverByHexes(HexModel h1, HexModel h2)
    {
        if(h1.ID == h2.ID) return null;

        int id = h1.GetHexPairIndex(h2);

        if(_riversByHexPairID.ContainsKey(id)) return _riversByHexPairID[id];
        return null;
    }
    public RiverModel GetRiverByHexes(int h1, int h2)
    {
        if(h1 == h2) return null;
        
        var hex1 = Cache<HexModel>.GetModel(h1);
        var hex2 = Cache<HexModel>.GetModel(h2);

        int id = hex1.GetHexPairIndex(hex2);

        if(_riversByHexPairID.ContainsKey(id)) return _riversByHexPairID[id];
        return null;
    }
    public void SetupRivers()
    {
        var models = Cache<RiverModel>.GetModels();
        AddRivers(models);
    }
    public void AddRivers(List<RiverModel> models)
    {
        foreach (var model in models)
        {
            if(_riversByHexPairID.ContainsKey(model.HexPairID))
            {
                var oldRiver = _riversByHexPairID[model.HexPairID];
                _riversByID.Remove(oldRiver.ID);
                _riversByID.Add(model.ID, model);
                _riversByHexPairID[model.HexPairID] = model;
            }
            else
            {
                _riversByHexPairID.Add(model.HexPairID, model);
                _riversByID.Add(model.ID, model);
            }
        }
    }
    public void RemoveRivers(List<int> ids)
    {
        foreach (var id in ids)
        {
            if(_riversByID.ContainsKey(id))
            {
                var river = _riversByID[id];
                int hexPair = river.HexPairID;
                _riversByHexPairID.Remove(hexPair);
                _riversByID.Remove(id);
            }
        }
    }
}
