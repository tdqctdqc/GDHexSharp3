using Godot;
using System;
using HexWargame;
using System.Collections.Generic;

public class UnitGenerator
{
    private static Logic _logic; 
    private static StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    public static void Setup(Logic logic)
    {
        _logic = logic; 
    }
    public static List<UnitModel> GenerateUnits(List<FactionModel> factions, List<HexModel> hexes)
    {
        var units = new List<UnitModel>();
        var locs = Cache<LocationModel>.GetModels();
        foreach (var loc in locs)
        {
            var fac = loc.Faction;
            if(fac.ID == 1) continue; 
            var type = Game.I.Session.Data.SessionConstants.GarrisonUnitTypeID;
            var rank = Game.I.Session.Data.SessionConstants.GarrisonUnitRankID;
            var unit = GenerateUnit(loc.Hex, fac.ID, type, rank);
            units.Add(unit);
        }
        // for (int i = 1; i < factions.Count; i++)
        // {
        //     var fac = factions[i];
        //     if(fac == null)
        //     {
        //         GD.Print("null fac");
        //         continue; 
        //     }
        //     var unit = GenerateUnit(hexes[i], fac.ID, 2, 1);
        //     units.Add(unit);
        // }
        _interface.AddModels<UnitModel>(units);

        return units; 
    }
    public static UnitModel AddUnit(HexModel hex, FactionModel faction, 
                                    UnitType type, UnitRank rank)
    {
        float moveCost = type.MoveType.TerrainCosts[hex.Terrain];
        if(float.IsInfinity(moveCost)) return null; 

        if(hex.Units.Count > 6) return null;
        if(hex.Faction.ID != faction.ID) return null;
        var unit = GenerateUnit(hex, faction.ID, type.ID, rank.ID);
        _interface.AddModel(unit);
        return unit;
    }
    public static UnitModel BuildUnit(HexModel hex, FactionModel faction, 
                                        UnitType type, UnitRank rank)
    {
        float indCost = type.IndustrialCost * rank.SizeMultiplier;
        float recruitCost = type.RecruitCost * rank.SizeMultiplier;
        if(indCost > faction.IndustrialPoints) return null;
        if(recruitCost > faction.Recruits) return null; 

        float moveCost = type.MoveType.TerrainCosts[hex.Terrain];
        if(float.IsInfinity(moveCost)) return null; 
        if(hex.Faction.ID != faction.ID) return null;

        if(hex.Units.Count > 6) return null;

        _logic.Faction.ConsumeIndustrialPoints(faction.ID, indCost);
        _logic.Faction.ConsumeRecruits(faction.ID, recruitCost);
        var unit = GenerateUnit(hex, faction.ID, type.ID, rank.ID);
        _interface.AddModel(unit);
        return unit;
    }
    public static UnitModel BuildFormation(HexModel hex, FactionModel faction, 
                                        UnitType type, UnitRank rank,
                                        Color primary, Color secondary)
    {
        float indCost = type.IndustrialCost * rank.SizeMultiplier;
        float recruitCost = type.RecruitCost * rank.SizeMultiplier;
        if(indCost > faction.IndustrialPoints) return null;
        if(recruitCost > faction.Recruits) return null; 

        float moveCost = type.MoveType.TerrainCosts[hex.Terrain];
        if(float.IsInfinity(moveCost)) return null; 
        if(hex.Faction.ID != faction.ID) return null;

        if(hex.Units.Count > 6) return null;

        _logic.Faction.ConsumeIndustrialPoints(faction.ID, indCost);
        _logic.Faction.ConsumeRecruits(faction.ID, recruitCost);
        var unit = GenerateUnit(hex, faction.ID, type.ID, rank.ID);
        var formation = GenerateFormation(rank.Rank, faction.ID, primary, secondary);
        _interface.AddModel(formation);
        
        unit.FormationID = formation.ID;

        _interface.AddModel(unit);

        return unit;
    }
    public static UnitModel GenerateUnit(HexModel hex,
                                            int faction, int type, int rank)
    {
        int hexID = hex.Coords.GetHexIDFromCoords();
        int id = Cache<UnitModel>.TakeID();
        var unit = new UnitModel(id, type, rank, "Dooters", faction, hexID);
                    
        return unit; 
    }
    public static UnitModel CreateMockUnit(int type, int rank)
    {
        var unit = new UnitModel()
                    {
                        UnitTypeID = type,
                        UnitRankID = rank,
                        Name = "Dooters",
                        FormationID = -1,
                        Readiness = 1f,
                        Strength = 1f,
                        Supply = 1f,
                        IsAlive = 1
                    };
        return unit; 
    }
    private static FormationModel GenerateFormation(int rank, 
                                                    int faction, 
                                                    Color primary,
                                                    Color secondary)
    {

        var formation = new FormationModel(Cache<FormationModel>.TakeID(), "Formation", rank, faction, primary.ToHtml(), secondary.ToHtml());
        return formation;
    }
}
