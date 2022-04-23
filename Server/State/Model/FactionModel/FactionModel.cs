using Godot;
using System;
using System.Collections.Generic;

public class FactionModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "Faction";

    public int ID {get; set;}
    public int NumFields => 5;
    public string Name
    { get => _name; 
        set { _name = value; Backing.Fields[0] = value; }
    }
    private string _name;
    public string PrimaryColorString 
    { get => _primaryColorString; 
        set { _primaryColorString = value; Backing.Fields[1] = value; }
    }
    private string _primaryColorString;
    public string SecondaryColorString
    { get => _secondaryColorString; 
        set { _secondaryColorString = value; Backing.Fields[2] = value; }
    }
    private string _secondaryColorString;
    public float IndustrialPoints
    { get => _industrialPoints; 
        set { _industrialPoints = value; Backing.Fields[3] = value.ToString(); }
    }
    private float _industrialPoints;
    public float Recruits
    { get => _recruits; 
        set { _recruits = value; Backing.Fields[4] = value.ToString(); }
    }
    private float _recruits;


    public BackingModel Backing { get; private set; }


    public List<HexModel> Hexes => Cache<HexModel>.GetModels(m => m.FactionID == ID);
    public Color PrimaryColor => new Color(PrimaryColorString);
    public Color SecondaryColor => new Color(SecondaryColorString);
    public FactionModel()
    {
        Backing = new BackingModel(this);
    }
    public FactionModel(int id, string name, Color baseColor, Color secondaryColor, float indPoints = 10000f, float recruits = 10000f)
    {
        ID = id;
        Backing = new BackingModel(this);
        Name = name;
        PrimaryColorString = baseColor.ToHtml();
        SecondaryColorString = secondaryColor.ToHtml();
        IndustrialPoints = indPoints;
        Recruits = recruits;
    }
    public void SyncFromBacking()
    {
        Name = Backing.Fields[0];
        PrimaryColorString = Backing.Fields[1];
        SecondaryColorString = Backing.Fields[2];
        IndustrialPoints = Backing.Fields[3].ToFloat();
        Recruits = Backing.Fields[4].ToFloat();  
    }
}
