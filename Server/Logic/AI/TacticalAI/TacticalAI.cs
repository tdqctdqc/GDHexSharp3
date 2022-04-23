using Godot;
using System;
using HexWargame;
using System.Collections.Generic;
using System.Linq;

public class TacticalAI 
{
    public static float GetHexDefenseScore(HexModel hex, Vector2 threatDir)
    {
        float score = 0f;
        if(hex.Terrain.IsWater) return Mathf.Inf;
        foreach (var n in hex.Neighbors)
        {
            var river = n.GetRiverToHex(hex);
            if(river == null) continue;
            Vector2 nDir = n.WorldPos - hex.WorldPos;
            float angle = threatDir.AngleTo(nDir);
            if(angle > Mathf.Pi / 2f) continue; 
            score += river.RiverType.AttackPenalty * .5f;
        }
        score += hex.Terrain.EvasionMod;
        return 10f - score; //lower return value means easier to defend
    }
    public static void FormationPlanDefense(FormationModel form)
    {
        var line = form.DefenseLine;
        if(line == null) return; 
        Vector2 threatDir = line.GetLineFacingVector();

        var units = form.Units.ToList();
        units.Remove(form.HQ);

        var lineHexes = new List<HexModel>();
        var hexesByDef = line.OrderByDescending(h => TacticalAI.GetHexDefenseScore(h, threatDir)).ToList();
        int count = 0;
        while(lineHexes.Count < units.Count)
        {
            lineHexes.Add( hexesByDef[count % hexesByDef.Count] );
            count++;
        }
        var assgn = AssignUnitsToLine(units, lineHexes);

        var orderManager = Game.I.Session.Server.StateInterface.OrderManager;
        var pathFinder = Game.I.Session.Utility.PathFinder;
        foreach (var unit in units)
        {
            var dest = assgn[unit];
            var path = pathFinder.FindUnitPath(unit, unit.Hex, dest);
            var order = new DefendOrder(unit, dest, path.Select(h => h.ID).ToList());
            orderManager.SetOrder(unit, order);
        }
    }
    public static float GetHexAttackScore(HexModel hex, Vector2 threatDir, FactionModel attacker)
    {
        float score = 0f;
        if(hex.Terrain.IsWater) return Mathf.Inf;

        foreach (var n in hex.Neighbors)
        {
            var river = n.GetRiverToHex(hex);
            if(river == null) continue;
            Vector2 nDir = n.WorldPos - hex.WorldPos;
            float angle = threatDir.AngleTo(nDir);
            if(angle > Mathf.Pi / 2f) continue; 
            score += river.RiverType.AttackPenalty;
        }

        foreach (var u in hex.Units)
        {
            if(u.Faction.CheckIfFactionHostile(attacker))
            {
                score += (u.AttackPower + u.DefensePower * 2f) / 500f;
            }
        }
        score += 5f * (1f - hex.Terrain.EvasionMod);
        return score; 
    }

    public static List<HexModel> GetDefenseLine(HexModel start, HexModel end)
    {
        var pathFinder = Game.I.Session.Utility.PathFinder;
        Vector2 threatDir = (start.WorldPos - end.WorldPos).Rotated(Mathf.Pi / 2f);
        Func<HexModel, HexModel, float> costFunc = (f,t) => 
        {
            return GetHexDefenseScore(t, threatDir);
        };
        var line = pathFinder.FindPath(start, end, costFunc);
        return line; 
    }
    public static List<HexModel> GetAttackAxis(FormationModel form, HexModel obj)
    {
        var pathFinder = Game.I.Session.Utility.PathFinder;
        Func<HexModel, HexModel, float> costFunc = (f,t) => 
        {
            Vector2 threatDir = t.WorldPos - f.WorldPos;
            return GetHexAttackScore(t, threatDir, form.Faction);
        };
        var axis = pathFinder.FindPath(form.DefenseLine.GetMiddleElement(), obj, costFunc).Where(h => h.FactionID != form.FactionID);
        return axis.ToList(); 
    }

    public static Dictionary<UnitModel, HexModel> AssignUnitsToLine(List<UnitModel> units, List<HexModel> line)
    {
        var pathFinder = Game.I.Session.Utility.PathFinder;
        Func<UnitModel, HexModel, int> costFunc = (u,h) => (int)pathFinder.FindPathCost(pathFinder.FindUnitPath(u, u.Hex, h), u);
        return HungarianAlgorithmWrapper.GetAssignment<UnitModel, HexModel>(units, line, costFunc);
    }
    public static void FormationPlanAttack2(FormationModel form)
    {
        //see if any enemy blobs
            //if so get avenues between / around, assign maneuver units to those
                //assign line units to frontally attack/pin
            //if not try to find a good defensive line ahead at min move radius of units
                //take intersection of hexes in attack axis and hexes in move radius. 
                //farthest away is center of new defense line
                //add out to flanks depending on atk frontage, assign units to line
    }

    public static void FormationPlanAttack(FormationModel form)
    {
        var pathFinder = Game.I.Session.Utility.PathFinder;
        var axis = form.AttackAxis;
        if(axis == null || axis.Count == 0) return; 
        Vector2 threatDir = axis.GetLineFacingVector();
        int atkFrontage = 5;
        var units = form.Units.ToList();
        units.Remove(form.HQ);
        var orders = new List<GoToOrder>();
        foreach (var unit in units)
        {
            var order = new GoToOrder(new List<int>(), unit.ID);
            orders.Add(order);
        }
        int atkDist = axis.Count - 1;
        for (int i = 0; i < atkDist; i++)
        {
            var from = axis[i];
            var to = axis[i + 1];
            var slice = GetAttackSlice(from, to, atkFrontage);
            var atkHexes = new List<HexModel>();
            var hexesByAtkDif = slice.OrderByDescending(h => TacticalAI.GetHexAttackScore(h, threatDir, form.Faction)).ToList();
            int count = 0;
            while(atkHexes.Count < units.Count)
            {
                atkHexes.Add( hexesByAtkDif[count % hexesByAtkDif.Count] );
                count++;
            }
            var assgn = AssignUnitsToLine(units, atkHexes);
            for (int j = 0; j < units.Count; j++)
            {
                var unit = units[j];
                if(i == 0)
                {
                    var path = pathFinder.FindUnitPath(unit, unit.Hex, assgn[unit]);
                    orders[j].AddToPath(path.Select(p => p.ID).ToList());
                }
                else
                {
                    var path = pathFinder.FindUnitPath(unit, orders[j].Destination, assgn[unit]);
                    path.RemoveAt(0);
                    orders[j].AddToPath(path.Select(p => p.ID).ToList());
                }
            }
        }

        var orderManager = Game.I.Session.Server.StateInterface.OrderManager;
        orderManager.SetOrders(units, new List<IOrder>(orders));
    }

    public static List<HexModel> GetAttackSlice(HexModel from, HexModel to, int frontage)
    {
        var list = new List<HexModel>();
        list.Add(to);
        if(frontage < 3) return list;
        var atkDir = to.CubeCoords - from.CubeCoords;
        var dirs = Constants.HexDirs;
        int atkDirIndex = dirs.IndexOf(atkDir);
        var leftFlankDir = dirs[(atkDirIndex + 4) % 6];
        var rightFlankDir = dirs[(atkDirIndex + 2) % 6];
        int flankLength = (frontage - 1) / 2;
        for (int i = 1; i < flankLength; i++)
        {
            var left = (to.CubeCoords + (leftFlankDir * i)).CubeToOffset().GetHexIDFromCoords();
            var leftHex = Cache<HexModel>.GetModel(left);
            if(leftHex != null) list.Add(leftHex);

            var right = (to.CubeCoords + (rightFlankDir * i)).CubeToOffset().GetHexIDFromCoords();
            var rightHex = Cache<HexModel>.GetModel(right);
            if(rightHex != null) list.Add(rightHex);
        }
        return list; 
    }
}
