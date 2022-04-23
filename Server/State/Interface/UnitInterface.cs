using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class UnitInterface 
{
    private StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    public void SplitUnit(UnitModel unit1)
    {
        if(unit1.Hex.Units.Count > Constants.MaxUnitsInHex - 2) 
        {
            return;
        }
        if(unit1.UnitRank.Rank < 2) 
        {
            return;
        }
        var newRank = Game.I.Session.Data.UnitRanks[unit1.UnitRank.Rank - 1];
        var unit2 = UnitGenerator.GenerateUnit(unit1.Hex, unit1.FactionID, unit1.UnitTypeID, newRank.Rank);
        var unit3 = UnitGenerator.GenerateUnit(unit1.Hex, unit1.FactionID, unit1.UnitTypeID, newRank.Rank);
        unit1.UnitRankID = newRank.ID;
        unit2.FormationID = unit1.FormationID;
        unit3.FormationID = unit1.FormationID;
        unit2.Readiness = unit1.Readiness; 
        unit2.Strength = unit1.Strength;
        unit2.Supply = unit1.Supply;
        unit3.Readiness = unit1.Readiness; 
        unit3.Strength = unit1.Strength;
        unit3.Supply = unit1.Supply;
        _interface.UpdateModel(unit1);
        _interface.AddModels(new List<UnitModel>(){unit2,unit3});
    }
    public void MergeUnits(List<UnitModel> units)
    {
        if(units.Count != 3) return;
        var unit1 = units[0];
        var ranks = Game.I.Session.Data.UnitRanks.Ranks;
        var rank = unit1.UnitRank;
        var type = unit1.UnitType;
        var hex = unit1.Hex;
        if(rank.Rank >= ranks.Count - 1) return;
        float newStrength = 0f; 
        float newSupply = 0f;
        float newReady = 0f; 
        foreach (var unit in units)
        {
            if(unit.UnitRank != rank || unit.UnitType != type || unit.Hex != hex)
            {
                return; 
            }
            newStrength += unit.Strength;
            newSupply += unit.Supply;
            newReady += unit.Readiness;
        }
        newStrength /= (float)units.Count; 
        newSupply /= (float)units.Count; 
        newReady /= (float)units.Count; 
        var newRank = ranks[rank.Rank + 1];

        unit1.Strength = newStrength;
        unit1.Readiness = newReady;
        unit1.Supply = newSupply;
        unit1.UnitRankID = newRank.ID;
        _interface.UpdateModel(unit1);
        units.Remove(unit1);
        _interface.DeleteModels(units);
    }
}
