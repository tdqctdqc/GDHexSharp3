using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class SupplyLogic 
{
    private Logic _logic;
    public SupplyLogic(Logic logic)
    {
        _logic = logic; 
    }

    public void DoSupply()
    {
        var partitions = HexSieve.GetPartitionsByFaction(Cache<HexModel>.GetModels());
        var units = Cache<UnitModel>.GetModels();
        var supplyLocs = Cache<LocationModel>.GetModels()
                            .Where(m => m.SupplyProd > 0f)
                            .Select(m => m.Hex.ID);
        var partitionsWithSupply = new List<int>();
        foreach (var partition in partitions)
        {
            if(partition.Value.Intersect(supplyLocs).Count() > 0)
            {
                partitionsWithSupply.Add(partition.Key);
            }
        }
        foreach (var unit in units)
        {
            var hex = unit.Hex; 
            var partitionIndex = partitions.Where(l => l.Value.Contains(hex.ID)).First().Key;
            if( partitionsWithSupply.Contains(partitionIndex))
            {
                unit.Supply = 1f; 
            }
            else
            {
                unit.Supply = 0f; 
            }
            if(unit.Supply == 0f)
            {
                unit.Readiness = Mathf.Max(0f, unit.Readiness - .25f);
            }
            else 
            {
                unit.Readiness = Mathf.Min(1f, unit.Readiness + unit.Supply * .25f);
            }
        }
    }
}
