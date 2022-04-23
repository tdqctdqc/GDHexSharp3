using Godot;
using HexWargame;
using System;
using System.Linq;
using System.Collections.Generic;

public class CombatLogic
{
    public static float MinDefReadiness = .25f, MinAtkReadiness = .5f;
    private Logic _logic; 
    private HashSet<int> _defendingHexes; 
    public Dictionary<int, List<int>> PendingRoundCombats; 
    private Dictionary<int, List<int>> _pendingRoundBombards; 

    public CombatLogic(Logic logic)
    {
        _logic = logic; 
        PendingRoundCombats = new Dictionary<int, List<int>>();
        _pendingRoundBombards = new Dictionary<int, List<int>>();
        _defendingHexes = new HashSet<int>();
    }
    public void DoRoundCombats()
    {
        foreach (var entry in _pendingRoundBombards)
        {
            var hex = Cache<HexModel>.GetModel(entry.Key);

            var bombarderIDs = entry.Value;
            var bombardAbility = Game.I.Session.Data.UnitAbilities.Bombard;
            var defenders = hex.Units;
            var bombarders = Cache<UnitModel>.GetModels(bombarderIDs)
                            .Where(u => u.Readiness >= MinAtkReadiness)
                            .Where(u => u.Order is BombardOrder)
                            .Where(u => u.UnitType.Abilities[bombardAbility] >= u.Hex.GetHexDistance(hex))
                            .ToList();
            DoBombards(defenders, bombarders, hex.Terrain);
        }
        foreach (var entry in PendingRoundCombats)
        {
            var attackerIDs = entry.Value; 
            var hex = Cache<HexModel>.GetModel(entry.Key);
            var nIDs = hex.ID.GetNeighborIDs();
            var defenders = hex.Units;
            var attackers = Cache<UnitModel>.GetModels(attackerIDs)
                            .Where(u => nIDs.Contains(u.Hex.ID))
                            .Where(u => u.Readiness >= MinAtkReadiness)
                            .ToList();
            
            //if no defenders let them advance
            Func<UnitModel,UnitModel,RiverType> riverFunc = (a,d) =>
            {
                var rModel = a.Hex.GetRiverToHex(d.Hex);
                if(rModel != null) return rModel.RiverType;
                return null;
            };
            DoGroundAttack(defenders, attackers, hex.Terrain, riverFunc, KillUnit);

            var retreaters = CheckRetreats(defenders, hex);
            DoRetreats(retreaters, hex);
        }
        //check dead units
        ClearRound();
    }

    private void DoBombards(List<UnitModel> defenders, List<UnitModel> bombarders, Terrain defTerrain)
    {
        if(defenders.Count == 0) return; 
        if(bombarders.Count == 0) return; 
        foreach (var bmb in bombarders)
        {
            var def = defenders[Game.I.Random.RandiRange(0, defenders.Count - 1)];
            Action<UnitModel> killFunc = (u) => _logic.Unit.KillUnit(u);
            DoHit(bmb, def, 1f, 1f, 1f, def.Hex.Terrain, null, killFunc);
        }
    }
    public static void DoGroundAttack(List<UnitModel> defenders, 
                                List<UnitModel> attackers, 
                                Terrain defTerrain,
                                Func<UnitModel,UnitModel,RiverType> getRiver,
                                Action<UnitModel> killFunc,
                                Action<UnitModel, UnitModel, Vector3> damageReport = null)
    {
        if(defenders.Count == 0) return; 
        if(attackers.Count == 0) return; 
        foreach (var atk in attackers)
        {
            var def = defenders[Game.I.Random.RandiRange(0, defenders.Count - 1)];
            var river = getRiver(atk,def);
            DoUnitGroundAttack(def, atk, river, defTerrain, killFunc, damageReport);
        }
    }
    private static void DoUnitGroundAttack(UnitModel def, 
                                            UnitModel atk, 
                                            RiverType river, 
                                            Terrain defTerrain, 
                                            Action<UnitModel> killFunc,
                                            Action<UnitModel, UnitModel, Vector3> damageReport = null)
    {
        float riverMod = 1f;
        if(river != null) riverMod = river.AttackPenalty;
        float atkDamage = DoHit(def, atk, riverMod, 1f, 2f, null, "def", killFunc, damageReport);
        //GD.Print($"attacker took {atkDamage} dmg");

        float defDamage = DoHit(atk, def, 1f, 1f, 1f, defTerrain, "atk", killFunc, damageReport);
        //GD.Print($"defender took {defDamage} dmg");
    }
    private static float DoHit( 
                        UnitModel hitter, 
                        UnitModel target, 
                        float riverMod, 
                        float flankingMod,
                        float dmgMod, 
                        Terrain defTerrain = null,
                        string hitterTag = "",
                        Action<UnitModel> killFunc = null,
                        Action<UnitModel, UnitModel, Vector3> damageReport = null)
    {
        float terrainEvasionMod;
        if(defTerrain == null) terrainEvasionMod = 0f;
        else terrainEvasionMod = defTerrain.EvasionMod;
        float chanceToHit = hitter.UnitType.Accuracy * (1f - terrainEvasionMod) * (1f - target.UnitType.Evasion);
        chanceToHit = Mathf.Min(chanceToHit, 1f);
        chanceToHit = Mathf.Max(chanceToHit, 0f);
        float hitRoll = Game.I.Random.RandfRange(0f, 1f);
        //GD.Print($"{hitterTag} chance to hit: {chanceToHit}");
        //GD.Print($"{hitterTag} hit roll: {hitRoll}");
        if(hitRoll >= (1f - chanceToHit) )
        {
            //GD.Print($"{hitterTag} did a hit!");
            float hitDamage = hitter.UnitRank.SizeMultiplier * hitter.UnitType.SoftAttack * (1f - target.UnitType.Hardness);
            float armorMod = 1f / target.UnitType.Armor;
            armorMod = Mathf.Min(armorMod, 1f);
            armorMod = Mathf.Max(armorMod, 0f);
            hitDamage += hitter.UnitRank.SizeMultiplier * hitter.UnitType.HardAttack * target.UnitType.Hardness * armorMod;
            hitDamage *= flankingMod * riverMod * hitter.Strength * dmgMod; 

            var strengthHit = hitDamage / (target.UnitType.HP * target.UnitRank.SizeMultiplier);
            target.Strength -= strengthHit;
            target.Strength = Mathf.Max(0f, target.Strength);
            if(target.Strength == 0f) killFunc?.Invoke(target);
            target.Readiness -= 3f * strengthHit;
            target.Readiness = Mathf.Max(0f, target.Readiness);

            float ipDamage = strengthHit * target.UnitType.IndustrialCost;
            float recDamage = strengthHit * target.UnitType.RecruitCost;
            var dmgInfo = new Vector3(hitDamage, ipDamage, recDamage);
            damageReport?.Invoke(hitter, target, dmgInfo);
            return hitDamage;
        }
        damageReport?.Invoke(hitter, target, Vector3.Zero);

        return 0f;
    }
    private void ClearRound()
    {
        PendingRoundCombats = new Dictionary<int, List<int>>();
        _pendingRoundBombards = new Dictionary<int, List<int>>();
        _defendingHexes = new HashSet<int>();
    }
    public void RegisterForCombat(UnitModel unit, HexModel hex)
    {
        if(PendingRoundCombats.ContainsKey(hex.ID))
        {
            PendingRoundCombats[hex.ID].Add(unit.ID);
        }
        else
        {
            PendingRoundCombats.Add(hex.ID, new List<int>(){unit.ID});
        }
        _defendingHexes.Add(hex.ID);
    }
    public void RegisterForBombard(UnitModel unit, HexModel hex)
    {
        if(unit.UnitType.Abilities.ContainsKey(Game.I.Session.Data.UnitAbilities.Bombard) == false) return;
        if(_pendingRoundBombards.ContainsKey(hex.ID))
        {
            _pendingRoundBombards[hex.ID].Add(unit.ID);
        }
        else
        {
            _pendingRoundBombards.Add(hex.ID, new List<int>(){unit.ID});
        }
        _defendingHexes.Add(hex.ID);
    }
    private List<UnitModel> CheckRetreats(List<UnitModel> defenders, HexModel hex)
    {
        var retreaters = new List<UnitModel>();
        foreach (var def in defenders)
        {
            if(def.Alive == false) continue;
            if(def.Readiness < MinDefReadiness)
            {
                retreaters.Add(def);
            }
        }
        return retreaters;
    }
    private void DoRetreats(List<UnitModel> retreaters, HexModel hex)
    {
        if(retreaters.Count == 0) return; 
        foreach (var retreater in retreaters)
        {
            var neighbors = hex.GetNeighbors();
            var retreatHexes = new List<HexModel>();
            var retreatScores = new List<float>();
            foreach (var n in neighbors)
            {
                if(retreater.Faction.CheckIfFactionHostile(n.Faction)) continue;
                if(n.Units.Count > 6) continue;

                retreatHexes.Add(n);
                var hostileUnitCount = n.GetNeighbors().SelectMany(m => m.Units)
                                        .Where(u => hex.Faction.CheckIfFactionHostile(u.Faction))
                                        .Count();
                retreatScores.Add(hostileUnitCount);
            }
            var retreatHex = retreatHexes
                            .OrderBy(h => retreatScores[retreatHexes.IndexOf(h)] )
                            .FirstOrDefault();
            if(retreatHex == null)
            {
                _logic.Unit.KillUnit(retreater);
            }
            var success = _logic.Unit.TryMoveUnit(retreater, retreatHex);
            if(success == false) 
            {
                _logic.Unit.KillUnit(retreater);
            } 
        }
    }
    private void KillUnit(UnitModel unit)
    {
       _logic.Unit.KillUnit(unit);
    }
}
