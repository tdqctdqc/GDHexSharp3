using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class UnitModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "Unit";
    public int ID  {get; set;}
    public BackingModel Backing { get; private set; }
    public int NumFields => 10;
    public int UnitTypeID 
    { get => _unitTypeID; 
        set { _unitTypeID = value; Backing.Fields[0] = value.ToString(); }
    }
    private int _unitTypeID;
    public int UnitRankID 
    { get => _unitRankID; 
        set { _unitRankID = value; Backing.Fields[1] = value.ToString(); }
    }
    private int _unitRankID;
    public string Name 
    { get => _name; 
        set { _name = value; Backing.Fields[2] = value.ToString(); }
    }
    private string _name;
    public int FactionID 
    { get => _factionID; 
        set { _factionID = value; Backing.Fields[3] = value.ToString(); }
    }
    private int _factionID;
    public int FormationID 
    { get => _formationID; 
        set { _formationID = value; Backing.Fields[4] = value.ToString(); }
    }
    private int _formationID;
    public float Readiness 
    { get => _readiness; 
        set { _readiness = value; Backing.Fields[5] = value.ToString(); }
    }
    private float _readiness;
    public float Strength 
    { get => _strength; 
        set { _strength = value; Backing.Fields[6] = value.ToString(); }
    }
    private float _strength; 
    public float Supply 
    { get => _supply; 
        set { _supply = value; Backing.Fields[7] = value.ToString(); }
    }
    private float _supply;
    public int HexID
    { get => _hexID; 
        set { _hexID = value; Backing.Fields[8] = value.ToString(); }
    }
    private int _hexID;
    public int IsAlive 
    { get => _isAlive; 
        set { _isAlive = value; Backing.Fields[9] = value.ToString(); }
    }
    private int _isAlive;

    public FactionModel Faction => Cache<FactionModel>.GetModel(FactionID);
    public FormationModel Formation => Cache<FormationModel>.GetModel(FormationID);
    public HexModel Hex => Cache<HexModel>.GetModel(HexID);
    public UnitType UnitType => Game.I.Session.Data.UnitTypes[UnitTypeID];
    public UnitRank UnitRank => Game.I.Session.Data.UnitRanks[UnitRankID];
    public IOrder Order {get; set;} //=> CacheManager.Orders.GetOrder(ID);
    public float ReadinessAPMultiplier => .5f + Readiness * .5f;
    public bool Alive => IsAlive == 1;
    public float AttackPower => UnitType.AttackValue * UnitRank.SizeMultiplier * Readiness * Strength;
    public float DefensePower => UnitType.DefenseValue * UnitRank.SizeMultiplier * Readiness * Strength;
    public UnitModel()
    {
        Backing = new BackingModel(this);
    }
    public UnitModel(int id, int type, int rank, string name, int faction, int hexID)
    {
        ID = id;
        Backing = new BackingModel(this);

        UnitTypeID = type;
        UnitRankID = rank;
        Name = "Dooters";
        FactionID = faction;
        FormationID = -1;
        Readiness = 1f;
        Strength = 1f;
        Supply = 1f;
        HexID = hexID;
        IsAlive = 1;
    }

    public void SyncFromBacking()
    {
        UnitTypeID = Backing.Fields[0].ToInt();
        UnitRankID = Backing.Fields[1].ToInt();
        Name = Backing.Fields[2];
        FactionID = Backing.Fields[3].ToInt();
        FormationID = Backing.Fields[4].ToInt();
        Readiness = Backing.Fields[5].ToFloat();
        Strength = Backing.Fields[6].ToFloat();
        Supply = Backing.Fields[7].ToFloat();
        HexID = Backing.Fields[8].ToInt();
        IsAlive = Backing.Fields[9].ToInt();
    }
}
