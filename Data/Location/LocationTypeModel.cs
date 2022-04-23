using Godot;
using System;
using System.Collections.Generic;

public class LocationTypeModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "LocationType";
    public BackingModel Backing { get; private set; }
    public int ID {get; set;}
    public int NumFields => 5;
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
    public float ProdPoints
    { get => _prodPoints; 
        set { _prodPoints = value; Backing.Fields[2] = value.ToString(); }
    }
    private float _prodPoints;
    public float Recruits
    { get => _recruits; 
        set { _recruits = value; Backing.Fields[3] = value.ToString(); }
    }
    private float _recruits;
    public float SupplyProd
    { get => _supplyProd; 
        set { _supplyProd = value; Backing.Fields[4] = value.ToString(); }
    }
    private float _supplyProd;
    public LocationTypeModel()
    {
        Backing = new BackingModel(this);
    }
    public void SyncFromBacking()
    {
        Name = Backing.Fields[0];
        TexturePath = Backing.Fields[1];
        ProdPoints = Backing.Fields[2].ToFloat();
        Recruits = Backing.Fields[3].ToFloat();
        SupplyProd = Backing.Fields[4].ToFloat();
    }
}
