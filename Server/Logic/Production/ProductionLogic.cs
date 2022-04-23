using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class ProductionLogic
{
    private StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    public void DoProduction()
    {
        var factions = Cache<FactionModel>.GetModels();
        var locations = Cache<LocationModel>.GetModels();
        
        foreach (var faction in factions)
        {
            float industry = 0f;
            float recruits = 0f;
            var locs = locations.Where(l => l.Faction.ID == faction.ID);
            foreach (var loc in locs)
            {
                industry += loc.ProdPoints;
                recruits += loc.Recruits;
            }
            faction.IndustrialPoints += industry;
            faction.Recruits += recruits; 
        }
    }
}
