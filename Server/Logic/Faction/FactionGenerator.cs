using Godot;
using System;
using System.Collections.Generic;

public class FactionGenerator 
{
    private static StateLogicInterface _interface => Game.I.Session.Server.StateInterface;

    public static List<FactionModel> GenerateFactions(int numFactions = 0)
    {
        var defaults = Game.I.Session.Data.DefaultFactions.Factions;
        int maxFactions = defaults.Count;
        if(numFactions == 0 || numFactions > maxFactions) numFactions = maxFactions;
        var neu = GenerateFaction(1, "Neutral", Colors.Transparent, Colors.Transparent);
        neu.ID = 1;
        var factions = new List<FactionModel>(){neu};

        for (int i = 0; i < numFactions; i++)
        {
            var defaultFac = defaults[i];
            var fac = GenerateFaction(i + 2, defaultFac.Name, defaultFac.PrimaryColor, defaultFac.SecondaryColor);
            factions.Add(fac);
        }
        
        _interface.AddModels<FactionModel>(factions);
        return factions; 
    }
    private static FactionModel GenerateFaction(int id, string name, Color baseColor, Color secondaryColor, float indPoints = 10000f, float recruits = 10000f)
    {
        var fac = new FactionModel(id,name, baseColor, secondaryColor);
        return fac; 
    }
}