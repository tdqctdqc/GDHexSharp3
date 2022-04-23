using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class HexModel : IModel, IGraphNode<HexModel>
{
    public string TableNameI => TableName; 
    public static string TableName = "Hex";
    public BackingModel Backing { get; private set; }

    public int ID { get; set; }

    public int NumFields => 4;
    public int X
    { get => _x; 
        set { _x = value; Backing.Fields[0] = value.ToString(); }
    }
    private int _x;
    public int Y
    { get => _y; 
        set { _y = value; Backing.Fields[1] = value.ToString(); }
    }
    private int _y;
    public int TerrainID
    { get => _terrainID; 
        set { _terrainID = value; Backing.Fields[2] = value.ToString(); }
    }
    private int _terrainID;
    public int FactionID
    { get => _factionID; 
        set { _factionID = value; Backing.Fields[3] = value.ToString(); }
    }
    private int _factionID;

    public Vector2 Coords => new Vector2(X,Y);
    public Vector3 CubeCoords => Coords.OffsetToCube();
    public string Name => Coords.ToString();
    public Vector2 WorldPos => Coords.GetWorldPosFromOffset();
    public Terrain Terrain => Game.I.Session.Data.Terrain[TerrainID];
    public FactionModel Faction => Cache<FactionModel>.GetModel(FactionID);
    public List<UnitModel> Units => Cache<UnitModel>.GetModels(u => u.HexID == ID && u.Alive);
    public List<HexModel> Neighbors => this.GetNeighbors();
    public HexModel()
    {
        Backing = new BackingModel(this);
    }
    public HexModel(int id, int x, int y, int factionID, int terrainID)
    {
        ID = id;
        Backing = new BackingModel(this);
        X = x;
        Y = y;
        FactionID = factionID;
        TerrainID = terrainID;
    }

    public void SyncFromBacking()
    {
        X = Backing.Fields[0].ToInt();
        Y = Backing.Fields[1].ToInt();
        TerrainID = Backing.Fields[2].ToInt();
        FactionID = Backing.Fields[3].ToInt();
    }
}
