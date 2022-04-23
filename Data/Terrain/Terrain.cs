using Godot;
using System;

public class Terrain : INamed
{
    public int ID {get; private set;}
    public string Name { get; private set; }
    public Color BaseColor { get; private set; } 
    public bool IsWater { get; private set; }
    public float EvasionMod { get; private set; }
    public float BuildCostMultiplier { get; private set; }
    public bool Elevated { get; private set; }
    public Terrain(TerrainModel model)
    {
        ID = model.ID;
        Name = model.Name;
        IsWater = model.IsWater == 0 ? false : true;
        BaseColor = new Color(model.BaseColor);
        EvasionMod = model.EvasionMod;
        if(model.BuildCostMultiplier == -1)
        {
            BuildCostMultiplier = Mathf.Inf;
        }
        else
        {
            BuildCostMultiplier = model.BuildCostMultiplier;
        }
        Elevated = model.Elevated;
    }

    public static Terrain GetTerrain(float altNoise, float wetNoise, float percentLand)
    {
        var terrains = Game.I.Session.Data.Terrain;
        if(1f - altNoise > percentLand) 
        {
            return terrains["Water"]; //water
        }
        if(altNoise > .7f)
        {
            if(altNoise > .9f)
            {
                return terrains["Mountain"];
            }
            return terrains["Hills"];
        }
        
        if(wetNoise < .2f) return terrains["Desert"];
        if(wetNoise < .5f) return terrains["Grassland"];
        if(wetNoise < .7f) return terrains["Forest"];
        return terrains["Swamp"];
    }
    public static Terrain GetLandTerrain(PreHex pre)
    {
        var terrains = Game.I.Session.Data.Terrain;
        
        if(pre.Roughness > 70f) return terrains["Mountain"];
        else if(pre.Roughness > 50f) return terrains["Hills"];
        else if(pre.Moisture > 90f) return terrains["Swamp"];
        else if(pre.Moisture > 50f) return terrains["Forest"];
        else if(pre.Moisture > 25f) return terrains["Grassland"];
        else return terrains["Desert"];
    }
}
