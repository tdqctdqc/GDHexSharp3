using Godot;
using System;

public class FormationInterface
{
    private Logic _logic; 
    public FormationInterface(Logic logic)
    {
        _logic = logic; 
    }
    public void FormationDefendLine(FormationModel form, HexModel start, HexModel end)
    {
        _logic.Formation.SetFormationDefenseLine(form, start, end);
    }
    public void FormationAttackObjective(FormationModel form, HexModel obj)
    {
        _logic.Formation.SetFormationAttackObjective(form, obj);
    }
}
