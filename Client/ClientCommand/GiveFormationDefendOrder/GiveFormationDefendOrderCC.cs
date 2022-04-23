using Godot;
using System;

public class GiveFormationDefendOrderCC : IClientCommand
{
    private HexSelector _hexSelector => Game.I.Session.Client.UI.HexSelector;
    public string Hint => "Select hex for start of line and mouseover for end";
    private FormationModel _formation;
    public GiveFormationDefendOrderCC(FormationModel formation)
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
        var end = _hexSelector.MouseOverHex;
        if(end == null) 
        {
            warning = "No mouse over hex";
            return false;
        }
        return true;
    }
    public void Do()
    {
        var start = _hexSelector.SelectedHex;
        if(start == null) return;
        var end = _hexSelector.MouseOverHex;
        if(end == null) return;
        Game.I.Session.Server.LogicInterface.Formation.FormationDefendLine(_formation, start, end);
    }
}
