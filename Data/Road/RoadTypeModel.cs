using Godot;
using System;
using System.Collections.Generic;

public class RoadTypeModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "RoadType";
    public BackingModel Backing { get; private set; }
    public int ID {get; set;}
    public int NumFields => 4;
    public string Name
    { get => _name; 
        set { _name = value; Backing.Fields[0] = value.ToString(); }
    }
    private string _name;
    public string TexturePath
    { get => _texturePath; 
        set { _texturePath = value; Backing.Fields[1] = value.ToString(); }
    }
    private string _texturePath;
    public float BaseBuildCost
    { get => _baseBuildCost; 
        set { _baseBuildCost = value; Backing.Fields[2] = value.ToString(); }
    }
    private float _baseBuildCost;
    public int Quality
    { get => _quality; 
        set { _quality = value; Backing.Fields[3] = value.ToString(); }
    }
    private int _quality; 
    public RoadTypeModel()
    {
        Backing = new BackingModel(this);
    }
    public void SyncFromBacking()
    {
        Name = Backing.Fields[0];
        TexturePath = Backing.Fields[0];
        BaseBuildCost = Backing.Fields[0].ToFloat();
        Quality = Backing.Fields[0].ToInt();
    }
}
