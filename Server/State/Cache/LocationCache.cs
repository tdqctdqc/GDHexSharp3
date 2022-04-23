using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class LocationCache
{
    public LocationModel this[int i] => GetLocByHexID(i);
    private Dictionary<int, LocationModel> _locsByHexID;
    private Dictionary<int, LocationModel> _locsByID;
    private StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    public LocationCache()
    {
        _locsByHexID = new Dictionary<int, LocationModel>();
        _locsByID = new Dictionary<int, LocationModel>();
        Cache<LocationModel>.ModelsAdded += AddLocations; 
        Cache<LocationModel>.ModelsDeleted += RemoveLocations;
        CacheManager.LoadedState += SetupLocations;
    }
    public LocationModel GetLocByHexID(int hexID)
    {
        if(_locsByHexID.ContainsKey(hexID)) return _locsByHexID[hexID];
        return null;
    }
    public LocationModel GetLocationByHex(HexModel hex)
    {
        if(_locsByHexID.ContainsKey(hex.ID)) return _locsByHexID[hex.ID];
        return null;
    }
    public void SetupLocations()
    {
        var models = Cache<LocationModel>.GetModels();
        AddLocations(models);
    }
    public void AddLocations(List<LocationModel> models)
    {
        foreach (var model in models)
        {
            if(_locsByHexID.ContainsKey(model.HexID))
            {
                var oldLoc = _locsByHexID[model.HexID];
                _locsByID.Remove(oldLoc.ID);
                _locsByID.Add(model.ID, model);
                _locsByHexID[model.HexID] = model;
            }
            else
            {
                _locsByHexID.Add(model.HexID, model);
                _locsByID.Add(model.ID, model);
            }
        }
    }
    public void RemoveLocations(List<int> ids)
    {
        foreach (var id in ids)
        {
            if(_locsByID.ContainsKey(id))
            {
                var loc = _locsByID[id];
                int hexPair = loc.HexID;
                _locsByHexID.Remove(hexPair);
                _locsByID.Remove(id);
            }
        }
    }    
}
