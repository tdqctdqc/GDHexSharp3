using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class UnitLogic
{
    private Logic _logic; 
    private StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    public UnitLogic(Logic logic)
    {
        _logic = logic; 
    }
    public void UnitDoMoveLogic(UnitModel unit, List<int> path, ref float storedAP)
    {
        var pathFinder = Game.I.Session.Utility.PathFinder;
        var dest = Cache<HexModel>.GetModel(path.Last());

        bool go = true; 
        while(go == true)
        {
            if(path.Count == 1 || unit.Hex.ID == path.Last() )
            {
                Cache<UnitModel>.UpdateModel(unit);
                return; 
            }
            var h1 = Cache<HexModel>.GetModel(path[0]);
            var h2 = Cache<HexModel>.GetModel(path[1]);
            if(unit.Faction.CheckIfFactionHostile(h2.Faction) 
                    && h2.Units.Count > 0)
            {
                _logic.Combat.RegisterForCombat(unit, h2);
                Cache<UnitModel>.UpdateModel(unit);
                return; 
            }

            float cost = pathFinder.GetTotalEdgeCostForUnit(h1, h2, unit);
            if(cost <= storedAP)
            {
                storedAP -= cost;
                TryMoveUnit(unit, h2);
                path.RemoveAt(0);
            }
            else if(float.IsInfinity(cost))
            {
                var newPath = pathFinder.FindUnitPath(unit, unit.Hex, dest);
                if(newPath == null || float.IsInfinity(pathFinder.GetTotalEdgeCostForUnit(newPath[0], newPath[1], unit)))
                {
                    path.Clear();
                    path.Add(unit.Hex.ID);
                    go = false; 
                }
                else
                {
                    path.Clear();
                    path.AddRange(newPath.Select(p => p.ID).ToList());
                }
            }
            else
            {
                go = false;
            }
        }
        Cache<UnitModel>.UpdateModel(unit);
    }
    public void DoBuildRoadLogic(UnitModel unit, ref float storedAP, 
                                ref float storedEP, RoadType roadType,
                                List<int> roadPath, Logic logic)
    {
        var from = Cache<HexModel>.GetModel(roadPath[0]);
        var to = Cache<HexModel>.GetModel(roadPath[1]);
        var pathFinder = Game.I.Session.Utility.PathFinder;

        if(unit.Hex != from)
        {
            storedEP = 0f;
            var path = pathFinder.FindUnitPath(unit, unit.Hex, from);
            logic.Unit.UnitDoMoveLogic(unit, path.Select(h => h.ID).ToList(), ref storedAP);
            return;
        }
        
        var oldRoad = CacheManager.Roads.GetRoadByHexes(from, to);

        if(oldRoad != null)
        {
            if(oldRoad.RoadType.Quality >= roadType.Quality)
            {
                float moveCost = pathFinder.GetTotalEdgeCostForUnit(from, to, unit);
                if(storedAP >= moveCost)
                {
                    roadPath.RemoveAt(0);
                    storedAP -= moveCost;
                    logic.Unit.TryMoveUnit(unit, to);
                }   
                return;
            }
        }
        //check river cost
        var eng = Game.I.Session.Data.UnitAbilities.Engineer;
        var ability = unit.UnitType.Abilities[eng] * unit.Strength * storedAP;
        var epCost = roadType.GetBuildCost(from, to);
        storedAP = 0f; 

        storedEP += ability;
        if(storedEP >= epCost)
        {
            storedEP = 0f; 
            logic.Road.BuildRoad(from, to, roadType.ID);
            roadPath.RemoveAt(0);
            if(roadPath.Count == 1) roadPath.Clear();
            logic.Unit.TryMoveUnit(unit, to);
        }
    }
    public void UnitDoReserveLogic(UnitModel unit, HexModel reserveHex, ref float storedAP, List<int> path)
    {
        if(unit.Formation == null) return;
        var pendingBattles = _logic.Combat.PendingRoundCombats;
        var defenderLocs = unit.Formation.Units.Where(n => n.Order is DefendOrder).Select(n => n.HexID);
        var battleLocs = new List<int>();
        foreach (var l in defenderLocs)
        {
            if(pendingBattles.ContainsKey(l))
            {
                battleLocs.Add(l);
            }
        }
        if(battleLocs.Count == 0) 
        {
            if(unit.Hex != reserveHex)
            {
                path.Clear();
                var reinforcePath = Game.I.Session.Utility.PathFinder.FindUnitPath(unit, unit.Hex, reserveHex);
                path.AddRange(reinforcePath.Select(h => h.ID));
            }
            else
            {
                storedAP = 0f; 
                return; 
            }
        }
        else 
        {
            battleLocs.OrderBy(l => pendingBattles[l].Count);
            var reinforceLoc = Cache<HexModel>.GetModel(battleLocs[0]);
            var reinforcePath = Game.I.Session.Utility.PathFinder.FindUnitPath(unit, unit.Hex, reinforceLoc);
            path.Clear();
            path.AddRange(reinforcePath.Select(p => p.ID));
        }
        UnitDoMoveLogic(unit, path, ref storedAP);
    }



    public bool TryMoveUnit(UnitModel unit, HexModel dest)
    {
        var oldHex = unit.Hex;
        if(dest.Units.Count >= Constants.MaxUnitsInHex) return false;
        unit.HexID = dest.ID;
        _logic.Faction.ChangeAdjacentHexesFaction(new List<HexModel>(){dest}, unit.Faction);
        Cache<UnitModel>.UpdateModel(unit);

        return true; 
    }
    public void KillUnit(UnitModel unit)
    {
        unit.IsAlive = 0;
        
        Cache<UnitModel>.QueueModelDeletion(unit.ID);
    }
    public void KillUnits(List<UnitModel> units)
    {
        foreach (var unit in units)
        {
            unit.IsAlive = 0;
        }
        Cache<UnitModel>.QueueModelsDeletion(units.Select(u => u.ID).ToList());
    }
}
