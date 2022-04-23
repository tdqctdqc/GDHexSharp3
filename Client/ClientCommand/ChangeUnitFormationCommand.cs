using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class ChangeUnitFormationCommand : IClientCommand
{
    private List<int> _unitIDs; 
    private int _factionID;
    private int _maxRank; 
    private StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    private List<UnitModel> _units => Cache<UnitModel>.GetModels(_unitIDs);
    public string Hint => "Select HQ Unit for new formation and press enter";
    public ChangeUnitFormationCommand(List<UnitModel> units)
    {
        if(units.Count == 0) return; 
        _unitIDs = units.Select(u => u.ID).ToList(); 
        _factionID = _units[0].FactionID;
        _maxRank = _units.Select(u => u.UnitRank.Rank).OrderBy(r => r).Last();
    }
    public bool Condition(out string warning)
    {
        var selectedUnit = Game.I.Session.Client.UI.UnitSelector.Current;
        
        if(selectedUnit == null)
        {
            warning = "No unit selected";
            return false;
        }
        if(selectedUnit.Faction.ID != _factionID)
        {
            warning = $"HQ is not of the right faction, {selectedUnit.Faction.ID} {_factionID}";
            return false;
        }
        if(selectedUnit.UnitType.IsHQ == false)
        {
            warning = "Unit selected is not an HQ";
            return false;
        }
        if(selectedUnit.UnitRank.Rank <= _maxRank)
        {
            warning = "HQ must be of a higher rank than the unit";
            return false;
        }
        
        warning = "";
        return true; 
    }

    public void Do()
    {
        if(Condition(out _))
        {
            var newFormID = Game.I.Session.Client.UI.UnitSelector.Current.FormationID;
            foreach(var unit in _units)
            {
                if(unit.UnitType.IsHQ) unit.Formation.ParentFormationID = newFormID;
                else unit.FormationID = newFormID;
            }
            _interface.UpdateModels(_units);
        }
    }
}
