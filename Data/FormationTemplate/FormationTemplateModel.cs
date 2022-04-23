using Godot;
using System;
using System.Collections.Generic;

public class FormationTemplateModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "FormationTemplate";
    public BackingModel Backing { get; private set; }
    public int ID {get; set;}
    public int NumFields => 3; 
    public int Rank
    { get => _rank; 
        set { _rank = value; Backing.Fields[0] = value.ToString(); }
    }
    private int _rank;
    public string Name
    { get => _name; 
        set { _name = value; Backing.Fields[1] = value.ToString(); }
    }
    private string _name;
    public string UnitsJSON
    { get => _unitsJSON; 
        set { _unitsJSON = value; Backing.Fields[2] = value.ToString(); }
    }
    private string _unitsJSON;
    public Godot.Collections.Array Units => (Godot.Collections.Array)JSON.Parse(UnitsJSON).Result;
    public FormationTemplateModel()
    {
        Backing = new BackingModel(this);
    }
    public void SyncFromBacking()
    {
        Rank = Backing.Fields[0].ToInt();
        Name = Backing.Fields[1];
        UnitsJSON = Backing.Fields[2];
    }
}