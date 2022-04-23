using Godot;
using System;
using System.Collections.Generic;

public class UnitTypeModel : IModel
{
    public string TableNameI => TableName; 
    public static string TableName = "UnitType";

    public int ID { get; set; }
    public BackingModel Backing { get; private set; }
    public int NumFields => 16;
    public string Name
    { get => _name; 
        set { _name = value; Backing.Fields[0] = value.ToString(); }
    }
    private string _name;
    public float SoftAttack
    { get => _softAttack; 
        set { _softAttack = value; Backing.Fields[1] = value.ToString(); }
    }
    private float _softAttack;
    public float HardAttack
    { get => _hardAttack; 
        set { _hardAttack = value; Backing.Fields[2] = value.ToString(); }
    }
    private float _hardAttack;
    public float HP
    { get => _hp; 
        set { _hp = value; Backing.Fields[3] = value.ToString(); }
    }
    private float _hp;
    public float Armor
    { get => _armor; 
        set { _armor = value; Backing.Fields[4] = value.ToString(); }
    }
    private float _armor;
    public float Hardness
    { get => _hardness; 
        set { _hardness = value; Backing.Fields[5] = value.ToString(); }
    }
    private float _hardness; 
    public float Accuracy
    { get => _accuracy; 
        set { _accuracy = value; Backing.Fields[6] = value.ToString(); }
    }
    private float _accuracy;
    public float Evasion
    { get => _evasion; 
        set { _evasion = value; Backing.Fields[7] = value.ToString(); }
    }
    private float _evasion;
    public float Weight
    { get => _weight; 
        set { _weight = value; Backing.Fields[8] = value.ToString(); }
    }
    private float _weight;
    public float IndustrialCost
    { get => _industrialCost; 
        set { _industrialCost = value; Backing.Fields[9] = value.ToString(); }
    }
    private float _industrialCost;
    public float RecruitCost
    { get => _recruitCost; 
        set { _recruitCost = value; Backing.Fields[10] = value.ToString(); }
    }
    private float _recruitCost;
    public int MoveTypeID
    { get => _moveTypeID; 
        set { _moveTypeID = value; Backing.Fields[11] = value.ToString(); }
    }
    private int _moveTypeID;
    public string NATOIconPath
    { get => _natoIconPath; 
        set { _natoIconPath = value; Backing.Fields[12] = value.ToString(); }
    }
    private string _natoIconPath;
    public int IsHQ
    { get => _isHQ; 
        set { _isHQ = value; Backing.Fields[13] = value.ToString(); }
    }
    private int _isHQ;
    public int IsSupply
    { get => _isSupply; 
        set { _isSupply = value; Backing.Fields[14] = value.ToString(); }
    }
    private int _isSupply;
    public string AbilityString
    { get => _abilityString; 
        set { _abilityString = value; Backing.Fields[15] = value.ToString(); }
    }
    private string _abilityString;


    public Godot.Collections.Dictionary Abilities => AbilityString == null ? new Godot.Collections.Dictionary() : (Godot.Collections.Dictionary)JSON.Parse(AbilityString).Result;
    public UnitTypeModel()
    {
        Backing = new BackingModel(this);
    }
    public void SyncFromBacking()
    {
        Name = Backing.Fields[0];
        SoftAttack = Backing.Fields[1].ToFloat();
        HardAttack = Backing.Fields[2].ToFloat();
        HP = Backing.Fields[3].ToFloat();
        Armor = Backing.Fields[4].ToFloat();
        Hardness = Backing.Fields[5].ToFloat();
        Accuracy = Backing.Fields[6].ToFloat();
        Evasion = Backing.Fields[7].ToFloat();
        Weight = Backing.Fields[8].ToFloat();
        IndustrialCost = Backing.Fields[9].ToFloat();
        RecruitCost = Backing.Fields[10].ToFloat();
        MoveTypeID = Backing.Fields[11].ToInt();
        NATOIconPath = Backing.Fields[12];
        IsHQ = Backing.Fields[13].ToInt();
        IsSupply = Backing.Fields[14].ToInt();
        AbilityString = Backing.Fields[15];
    }
}
