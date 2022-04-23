using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class AICache 
{
    public Dictionary<int, List<HexModel>> UnitMoveRadii { get; private set; }
    public InfluenceMap InfluenceMap { get; private set; }
    public AICache()
    {
        UnitMoveRadii = new Dictionary<int, List<HexModel>>();
        Cache<UnitModel>.ModelsDeleted += RemoveUnits;
        Cache<UnitModel>.ModelsAdded += AddUnits;
        InfluenceMap = new InfluenceMap();
    }
    public void SetCache()
    {
        var units = Cache<UnitModel>.GetModels();
        foreach (var u in units)
        {
            var moveRadius = Game.I.Session.Utility.PathFinder.FindMovementRadius(u);
            UnitMoveRadii[u.ID] = moveRadius;
        }

        InfluenceMap.SetPowerMap();
    }
    public List<HexModel> GetUnitsMoveRadius(List<UnitModel> units)
    {
        if (units.Count == 0) return new List<HexModel>();
        var radius = UnitMoveRadii[units[0].ID];
        if(units.Count == 1) return radius; 
        for (int i = 1; i < units.Count; i++)
        {
            radius = radius.Intersect(UnitMoveRadii[units[i].ID]).ToList();
        }
        return radius;
    }
    public void AddUnits(List<UnitModel> units)
    {
        foreach (var unit in units)
        {
            if(UnitMoveRadii.ContainsKey(unit.ID) == false) 
                    UnitMoveRadii.Add(unit.ID, new List<HexModel>());
        }
    }
    public void RemoveUnits(List<int> IDs)
    {
        IDs.ForEach(i => UnitMoveRadii.Remove(i));
    }
}
