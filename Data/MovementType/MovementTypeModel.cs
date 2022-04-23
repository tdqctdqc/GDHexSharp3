using Godot;
using System;
using System.Collections.Generic;

public class MovementTypeModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "MovementType"; 

    public BackingModel Backing {get; private set; }
    
    public int ID { get; set; }
    public int NumFields => 4; 
    public string Name
    { get => _name; 
        set { _name = value; Backing.Fields[0] = value.ToString(); }
    }
    private string _name;
    public string TerrainCostsJSON
    { get => _terrainCostsJSON; 
        set { _terrainCostsJSON = value; Backing.Fields[1] = value.ToString(); }
    }
    private string _terrainCostsJSON;
    public string RoadCostsJSON
    { get => _roadCostsJSON; 
        set { _roadCostsJSON = value; Backing.Fields[2] = value.ToString(); }
    }
    private string _roadCostsJSON;
    public string RiverCostsJSON
    { get => _riverCostsJSON; 
        set { _riverCostsJSON = value; Backing.Fields[3] = value.ToString(); }
    }
    private string _riverCostsJSON;

    public Godot.Collections.Dictionary TerrainCosts => (Godot.Collections.Dictionary)JSON.Parse(TerrainCostsJSON).Result;
    public Godot.Collections.Dictionary RoadCosts => (Godot.Collections.Dictionary)JSON.Parse(RoadCostsJSON).Result;
    public Godot.Collections.Dictionary RiverCosts => (Godot.Collections.Dictionary)JSON.Parse(RiverCostsJSON).Result;
   
    public MovementTypeModel()
    {
        Backing = new BackingModel(this);
    }
    public List<string> GetColumnValuesList()
    {
        var list = new List<string>();
        list.Add(Name);
        list.Add(TerrainCostsJSON);
        list.Add(RoadCostsJSON);
        list.Add(RiverCostsJSON);
        return list;
    }
    public void SyncFromBacking()
    {
        Name = Backing.Fields[0];
        TerrainCostsJSON = Backing.Fields[1];
        RoadCostsJSON = Backing.Fields[2];
        RiverCostsJSON = Backing.Fields[3];
    }
}
