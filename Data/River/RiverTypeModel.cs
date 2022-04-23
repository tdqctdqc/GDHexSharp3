using Godot;
using System;
using System.Collections.Generic;

public class RiverTypeModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "RiverType";
    public BackingModel Backing {get; private set;}
    public int ID {get; set;}
    public int NumFields => 6;
    public string Name
    { get => _name; 
        set { _name = value; Backing.Fields[0] = value.ToString(); }
    }
    private string _name;
    public int Width
    { get => _width; 
        set { _width = value; Backing.Fields[1] = value.ToString(); }
    }
    private int _width;
    public string TexturePath
    { get => _texturePath; 
        set { _texturePath = value; Backing.Fields[2] = value.ToString(); }
    }
    private string _texturePath;
    public float BuildCost
    { get => _buildCost; 
        set { _buildCost = value; Backing.Fields[3] = value.ToString(); }
    }
    private float _buildCost;
    public float AttackPenalty
    { get => _attackPenalty; 
        set { _attackPenalty = value; Backing.Fields[4] = value.ToString(); }
    }
    private float _attackPenalty;
    public float MinFlow
    { get => _minFlow; 
        set { _minFlow = value; Backing.Fields[5] = value.ToString(); }
    }
    private float _minFlow;
    public RiverTypeModel()
    {
        Backing = new BackingModel(this);
    }
    public void SyncFromBacking()
    {
        Name = Backing.Fields[0];
        Width = Backing.Fields[1].ToInt();
        TexturePath = Backing.Fields[2];
        BuildCost = Backing.Fields[3].ToFloat();
        AttackPenalty = Backing.Fields[4].ToFloat();
        MinFlow = Backing.Fields[5].ToFloat();
    }
}
