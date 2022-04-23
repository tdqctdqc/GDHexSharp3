using Godot;
using System;
using System.Collections.Generic;

public class UnitRankData
{
    public List<UnitRank> Ranks { get; private set; } 
    public UnitRank this[int i] => _ranks[i];
    private Dictionary<int, UnitRank> _ranks; 
    public UnitRankData(List<UnitRankModel> ranks)
    {
        Ranks = new List<UnitRank>();
        _ranks = new Dictionary<int, UnitRank>();
        foreach (var model in ranks)
        {
            var rank = new UnitRank(model);
            _ranks.Add(model.Rank, rank);
            Ranks.Add(rank);
        }
    }
}
