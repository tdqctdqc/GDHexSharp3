using Godot;
using System;

public class UnitRank : INamed
{
    public int ID { get; private set; }
    public int Rank { get; private set; }
    public string Name { get; private set; }
    public string Marker { get; private set; }
    public float SizeMultiplier { get; private set; }
    public UnitRank(UnitRankModel model)
    {
        ID = model.ID;
        Rank = model.Rank;
        Name = model.Name;
        Marker = model.Marker;
        SizeMultiplier = Mathf.Pow(3f, Rank);
    }
}
