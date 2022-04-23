using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class RoadModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "Road";

    public int ID {get; set;}
    public BackingModel Backing { get; private set; }
    public int NumFields => 4;
    public int RoadTypeID 
    { get => _roadTypeID; 
        set { _roadTypeID = value; Backing.Fields[0] = value.ToString(); }
    }
    private int _roadTypeID;
    public int FromID 
    { get => _fromID; 
        set { _fromID = value; Backing.Fields[1] = value.ToString(); }
    }
    private int _fromID;
    public int ToID 
    { get => _toID; 
        set { _toID = value; Backing.Fields[2] = value.ToString(); }
    }
    private int _toID;
    public int HexPairID 
    { get => _hexPairID; 
        set { _hexPairID = value; Backing.Fields[3] = value.ToString(); }
    }
    private int _hexPairID;


    public string Name => "Road " + ID;
    public Vector2 Coords1 => Hex1.Coords;
    public Vector2 Coords2 => Hex2.Coords;
    public HexModel Hex1 => Cache<HexModel>.GetModel( FromID);
    public HexModel Hex2 => Cache<HexModel>.GetModel( ToID );
    public Vector2 Position => (Hex1.WorldPos + Hex2.WorldPos) / 2f;
    public RoadType RoadType => Game.I.Session.Data.RoadTypes[RoadTypeID];
    public float Angle => Hex1.GetHexAngle(Hex2);
    public RoadModel(){}
    public RoadModel(int id, int hexPairID, int roadTypeID, int from, int to)
    {
        ID = id;
        Backing = new BackingModel(this);
        HexPairID = hexPairID;
        RoadTypeID = roadTypeID;
        FromID = from;
        ToID = to;
    }

    public void SyncFromBacking()
    {
        RoadTypeID = Backing.Fields[0].ToInt();
        FromID = Backing.Fields[1].ToInt();
        ToID = Backing.Fields[2].ToInt();
        HexPairID = Backing.Fields[3].ToInt();
    }
}
