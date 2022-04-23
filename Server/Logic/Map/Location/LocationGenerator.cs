using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class LocationGenerator 
{
    private static StateLogicInterface _interface => Game.I.Session.Server.StateInterface;

    public static List<LocationModel> GenerateLocations(List<HexModel> hexes)
    {
        var locList = new List<LocationModel>();

        var cityType = Game.I.Session.Data.LocationTypes[1];
        var townType = Game.I.Session.Data.LocationTypes[2];
        var water = Game.I.Session.Data.Terrain["Water"];
        var mtn = Game.I.Session.Data.Terrain["Mountain"];
        float genChance = .5f; 
        float cityChance = .2f;
        float townChance = .8f;
        foreach (var cell in Game.I.Session.MapGenPackage.Cells)
        {
            var hexID = cell.Children[0].ID;
            if(cell.Plate.Land == false) continue; 
            var hex = hexes.Where(h => h.ID == hexID).FirstOrDefault();
            float genRoll = Game.I.Random.RandfRange(0f, 1f);
            if(genRoll > genChance) continue; 
            if(hex != null && hex.TerrainID != water.ID && hex.TerrainID != mtn.ID)
            {
                float typeRoll = Game.I.Random.RandfRange(0f, 1f);
                var type = typeRoll > townChance ? cityType : townType;
                var loc = new LocationModel(Cache<LocationModel>.TakeID(), "Dootville", hexID, type);
                locList.Add(loc);
            }
        }
        _interface.AddModels<LocationModel>(locList);
        return locList; 
    }
}
