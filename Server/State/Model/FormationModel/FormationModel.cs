using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class FormationModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "Formation";
    public BackingModel Backing { get; private set; }

    public int ID {get; set;}
    public int NumFields => 6;
    public int Rank 
    { get => _rank; 
        set { _rank = value; Backing.Fields[0] = value.ToString(); }
    }
    private int _rank;
    public string Name 
    { get => _name; 
        set { _name = value; Backing.Fields[1] = value; }
    }
    private string _name;
    public int FactionID 
    { get => _factionID; 
        set { _factionID = value; Backing.Fields[2] = value.ToString(); }
    }
    private int _factionID;
    public int ParentFormationID 
    { get => _parentFormationID; 
        set { _parentFormationID = value; Backing.Fields[3] = value.ToString(); }
    }
    private int _parentFormationID; 
    public string PrimaryColorString 
    { get => _primaryColorString; 
        set { _primaryColorString = value; Backing.Fields[4] = value; }
    }
    private string _primaryColorString;
    public string SecondaryColorString 
    { get => _secondaryColorString; 
        set { _secondaryColorString = value; Backing.Fields[5] = value; }
    }
    private string _secondaryColorString; 

    public List<HexModel> DefenseLine { get; set; }
    public List<HexModel> AttackFrontLine { get; set; }
    public List<HexModel> AttackAxis { get; set; }

    public Color PrimaryColor => new Color(PrimaryColorString);
    public Color SecondaryColor => new Color(SecondaryColorString);
    public List<UnitModel> Units => Cache<UnitModel>
                                    .GetModels()
                                    .Where(u => u.FormationID == ID)
                                    .ToList();

    public List<FormationModel> Subformations => Cache<FormationModel>
                                            .GetModels()
                                            .Where(u => u.ParentFormationID == ID)
                                            .ToList();
    public UnitModel HQ => Units.Where(u => u.UnitType.IsHQ).First();
    public FormationModel Parent => Cache<FormationModel>.GetModel(ParentFormationID);
    public FactionModel Faction => Cache<FactionModel>.GetModel(FactionID);
    public FormationModel()
    {

    }
    public FormationModel(int id, string name, int rank, int factionID, string primary, string secondary)
    {
        ID = id;
        Backing = new BackingModel(this);
        ParentFormationID = -1;
        Name = name;
        Rank = rank;
        FactionID = factionID; 
        PrimaryColorString = primary;
        SecondaryColorString = secondary;
    }
    public void SyncFromBacking()
    {
        Rank = Backing.Fields[0].ToInt();
        Name = Backing.Fields[1];
        FactionID = Backing.Fields[2].ToInt();
        ParentFormationID = Backing.Fields[3].ToInt();
        PrimaryColorString = Backing.Fields[4];
        SecondaryColorString = Backing.Fields[5];
    }
}
