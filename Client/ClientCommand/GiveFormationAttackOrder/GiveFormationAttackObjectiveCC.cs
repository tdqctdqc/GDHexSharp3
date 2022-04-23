using Godot;
using System;

public class GiveFormationAttackObjectiveCC : IClientCommand
{
    private HexSelector _hexSelector => Game.I.Session.Client.UI.HexSelector;
    public string Hint => "Select objective hex and press enter";
    private FormationModel _formation;
    public GiveFormationAttackObjectiveCC(FormationModel formation)
    {
        _formation = formation;
    }

    public bool Condition(out string warning)
    {
        var start = _hexSelector.SelectedHex;
        warning = "";
        if(start == null) 
        {
            warning = "No hex selected";
            return false;
        }
        return true;
    }
    public void Do()
    {
        var obj = _hexSelector.SelectedHex;
        if(obj == null) return;
        Game.I.Session.Server.LogicInterface.Formation.FormationAttackObjective(_formation, obj);
    }
}
