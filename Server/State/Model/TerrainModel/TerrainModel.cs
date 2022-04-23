using Godot;
using System;
using System.Collections.Generic;

public class TerrainModel : IModel
{
    //make data model
    public string TableNameI => TableName; 
    public static string TableName = "Terrain";     
    public BackingModel Backing {get; private set;}
    public int ID {get; set;}
    public int NumFields => 6;
    public string Name 
    { get => _name; 
        set { _name = value; Backing.Fields[0] = value; }
    }
    private string _name;
    public string BaseColor 
    { get => _baseColor; 
        set { _baseColor = value; Backing.Fields[1] = value; }
    } 
    private string _baseColor;

    public int IsWater 
    { get => _isWater; 
        set { _isWater = value; Backing.Fields[2] = value.ToString(); }
    }
    private int _isWater;
    public float EvasionMod 
    { get => _evasionMod; 
        set { _evasionMod = value; Backing.Fields[3] = value.ToString(); }
    }
    private float _evasionMod;
    public float BuildCostMultiplier 
    { get => _buildCostMultiplier; 
        set { _buildCostMultiplier = value; Backing.Fields[4] = value.ToString(); }
    }
    private float _buildCostMultiplier;
    public int ElevatedInt 
    { get => _elevatedInt; 
        set { _elevatedInt = value; Backing.Fields[5] = value.ToString(); }
    }
    private int _elevatedInt;

    public bool Elevated => ElevatedInt == 1;
    public TerrainModel()
    {
        Backing = new BackingModel(this);
    }

    public void SyncFromBacking()
    {
        Name = Backing.Fields[0];
        BaseColor = Backing.Fields[1];
        IsWater = Backing.Fields[2].ToInt();
        EvasionMod = Backing.Fields[3].ToFloat();
        BuildCostMultiplier = Backing.Fields[4].ToFloat();
        ElevatedInt = Backing.Fields[5].ToInt();
    }
}
