using Godot;
using System;
using HexWargame;
using System.Collections.Generic;

public class FactionLogic 
{
    private StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    private void ChangeHexFaction(HexModel hex, FactionModel newFaction)
    {
        if(hex.Terrain.IsWater) return; 
        var oldFaction = hex.Faction; 
        hex.FactionID = newFaction.ID;
        Cache<HexModel>.ModelsChanged(new List<HexModel>(){hex});
    }
    private void ChangeHexesFaction(List<HexModel> hexes, FactionModel newFaction)
    {
        foreach(var hex in hexes)
        {
            ChangeHexFaction(hex, newFaction);
        }
        Game.I.Session.Client.Events.Map.ChangedHexesFaction?.Invoke(hexes, newFaction);
    }
    public void ConsumeIndustrialPoints(int factionID, float ip)
    {
        var faction = Cache<FactionModel>.GetModel(factionID);
        faction.IndustrialPoints -= ip;
        _interface.UpdateModel(faction);
    }
    public void ConsumeRecruits(int factionID, float rec)
    {
        var faction = Cache<FactionModel>.GetModel(factionID);
        faction.Recruits -= rec;
        _interface.UpdateModel(faction);
    }
    public void ChangeAdjacentHexesFaction(List<HexModel> hexes, FactionModel newFaction)
    {
        var adjacentHexes = new HashSet<HexModel>();
        var hexesToChange = new List<HexModel>(hexes);
        foreach(var hex in hexes)
        {
            ChangeHexFaction(hex, newFaction);
            foreach (var nHex in hex.GetNeighbors())
            {
                if(hexes.Contains(nHex) == false)
                {
                    adjacentHexes.Add(nHex);
                }
            }
        }
        foreach (var hex in adjacentHexes)
        {
            if(hex.Units.Count == 0)
            {
                bool hasNeighboringHostileUnits = false;
                
                int adjacentHexesOfNewFaction = 0;
                foreach (var nHex in hex.GetNeighbors())
                {
                    if(nHex.Faction != newFaction)
                    {
                        if(nHex.Units.Count > 0)
                        {
                            hasNeighboringHostileUnits = true; 
                            break; 
                        }
                    }
                    else 
                    {
                        adjacentHexesOfNewFaction++;
                    }
                }

                if(hasNeighboringHostileUnits == false && adjacentHexesOfNewFaction > 2)
                {
                    hexesToChange.Add(hex);
                }
            }
        }
        ChangeHexesFaction(hexesToChange, newFaction);
    }

}
