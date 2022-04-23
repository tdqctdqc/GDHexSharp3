using Godot;
using System;
using System.Collections.Generic;

public class UnitType : INamed
{

    public int ID { get; private set; }
    public string Name { get; private set; }
    public Texture NATOIcon { get; private set; }
    public float SoftAttack { get; private set; }
    public float HardAttack { get; private set; }
    public float HP { get; private set; }
    public float Armor { get; private set; }
    public float Hardness { get; private set; }
    public float Accuracy { get; private set; }
    public float Evasion { get; private set; }
    public float Weight { get; private set; }
    public float IndustrialCost { get; private set; }
    public float RecruitCost { get; private set; }
    public MovementType MoveType { get; private set; }
    public bool IsHQ { get; private set; }    
    public bool IsSupply { get; private set; }    
    public Dictionary<IUnitAbility, float> Abilities { get; private set; }
    public float AttackValue { get; private set; }
    public float DefenseValue { get; private set; }
    public UnitType(UnitTypeModel model)
    {
        ID = model.ID;
        Name = model.Name;
        NATOIcon = (Texture)GD.Load("res://"+model.NATOIconPath);
        SoftAttack = model.SoftAttack;
        HardAttack = model.HardAttack;
        HP = model.HP;
        Armor = model.Armor;
        Hardness = model.Hardness;
        Accuracy = model.Accuracy;
        Evasion = model.Evasion;
        Weight = model.Weight;
        IndustrialCost = model.IndustrialCost;
        RecruitCost = model.RecruitCost;
        MoveType = Game.I.Session.Data.MovementTypes[model.MoveTypeID];
        IsHQ = model.IsHQ == 1 ? true : false;
        IsSupply = model.IsSupply == 1 ? true : false;
        Abilities = new Dictionary<IUnitAbility, float>();
        var modelAb = model.Abilities;
        foreach (var entry in Game.I.Session.Data.UnitAbilities.Abilities)
        {
            var ability = entry.Value; 
            if(modelAb.Contains(ability.Name))
            {
                float strength = (float)modelAb[ability.Name];
                Abilities.Add(ability, strength);
            }
        }

        AttackValue = (SoftAttack + HardAttack * 1.5f) * Accuracy;  
        DefenseValue = HP * ( Evasion + Armor * Hardness * 2f); 
    }
}
