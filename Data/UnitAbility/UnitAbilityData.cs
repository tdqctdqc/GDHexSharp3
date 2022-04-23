using Godot;
using System;
using System.Collections.Generic;

public class UnitAbilityData
{
    public IUnitAbility Engineer {get; private set; }
    public IUnitAbility Bombard {get; private set; }
    public Dictionary<string, IUnitAbility> Abilities { get; private set; }
    public UnitAbilityData()
    {
        Abilities = new Dictionary<string, IUnitAbility>();
        Bombard = new BombardAbility();
        Abilities.Add(Bombard.Name, Bombard);
        Engineer = new EngineerAbility();
        Abilities.Add(Engineer.Name, Engineer);

    }
}
