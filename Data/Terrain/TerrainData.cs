using Godot;
using System;
using System.Collections.Generic;

public class TerrainData 
{
    public Dictionary<int, Terrain> Terrains {get; private set;}
    public Dictionary<string, Terrain> TerrainsByName {get; private set;}
    public Terrain this[int id] => Terrains[id];
    public Terrain this[string name] => TerrainsByName[name];
    public TerrainData(List<TerrainModel> models)
    {
        Terrains = new Dictionary<int, Terrain>();
        TerrainsByName = new Dictionary<string, Terrain>();
        foreach (var model in models)
        {
            var terrain = new Terrain(model);
            Terrains.Add(model.ID, terrain);
            TerrainsByName.Add(model.Name, terrain);
        }
    }
}
