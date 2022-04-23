using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HexWargame;
public class FormationLogic 
{
    
    public void SetFormationDefenseLine(FormationModel form, HexModel start, HexModel end)
    {
        form.AttackAxis = null;
        var line = TacticalAI.GetDefenseLine(start, end);

        form.DefenseLine = line; 

        TacticalAI.FormationPlanDefense(form);
    }
    public void SetFormationAttackObjective(FormationModel form, HexModel end)
    {
        if (form.DefenseLine == null) return;
        var axis = TacticalAI.GetAttackAxis(form, end);
        form.AttackAxis = axis; 
        TacticalAI.FormationPlanAttack(form);
    }
}
