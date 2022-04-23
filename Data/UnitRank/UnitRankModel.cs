using Godot;
using System;
using System.Collections.Generic;

public class UnitRankModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "UnitRank";

    public int ID {get; set;}
    public int NumFields => 3;
    public BackingModel Backing { get; private set; }
    public string Name
    { get => _name; 
        set { _name = value; Backing.Fields[0] = value.ToString(); }
    }
    private string _name;
    public int Rank
    { get => _rank; 
        set { _rank = value; Backing.Fields[1] = value.ToString(); }
    }
    private int _rank;
    public string Marker
    { get => _marker; 
        set { _marker = value; Backing.Fields[2] = value.ToString(); }
    }
    private string _marker; 
    public UnitRankModel()
    {
        Backing = new BackingModel(this);
    }
    public void SyncFromBacking()
    {
        Name = Backing.Fields[0];
        Rank = Backing.Fields[0].ToInt();
        Marker = Backing.Fields[0];
    }
}
