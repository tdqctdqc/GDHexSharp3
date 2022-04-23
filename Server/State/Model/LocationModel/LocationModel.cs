using Godot;
using System;
using System.Collections.Generic;

public class LocationModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "Location";
    public int ID { get; set; }
    public BackingModel Backing { get; private set; }
    public int NumFields => 6;

    public string Name 
    { get => _name; 
        set { _name = value; Backing.Fields[0] = value.ToString(); }
    }
    private string _name;
    public int HexID
    { get => _hexID; 
        set { _hexID = value; Backing.Fields[1] = value.ToString(); }
    }
    private int _hexID;
    public int LocationTypeID
    { get => _locationTypeID; 
        set { _locationTypeID = value; Backing.Fields[2] = value.ToString(); }
    }
    private int _locationTypeID;
    public float ProdPoints
    { get => _prodPoints; 
        set { _prodPoints = value; Backing.Fields[3] = value.ToString(); }
    }
    private float _prodPoints;
    public float Recruits
    { get => _recruits; 
        set { _recruits = value; Backing.Fields[4] = value.ToString(); }
    }
    private float _recruits;
    public float SupplyProd
    { get => _supplyProd; 
        set { _supplyProd = value; Backing.Fields[5] = value.ToString(); }
    }
    private float _supplyProd;
  
    public HexModel Hex => Cache<HexModel>.GetModel(HexID);
    public FactionModel Faction => Hex.Faction;
    public LocationType LocationType => Game.I.Session.Data.LocationTypes[LocationTypeID];
    
    public LocationModel()
    {
        Backing = new BackingModel(this);
    }
    public LocationModel(int id, string name, int hexID, LocationType type)
    {
        ID = id;
        Backing = new BackingModel(this);
        Name = "Dootville";
        HexID = hexID;
        LocationTypeID = type.ID;
        ProdPoints = type.ProdPoints;
        Recruits = type.Recruits;
        SupplyProd = type.SupplyProd;
    }
    public void SyncFromBacking()
    {
        Name = Backing.Fields[0];
        HexID = Backing.Fields[1].ToInt();
        LocationTypeID = Backing.Fields[2].ToInt();
        ProdPoints = Backing.Fields[3].ToFloat();
        Recruits = Backing.Fields[4].ToFloat();
        SupplyProd = Backing.Fields[5].ToFloat();
    }
}
