using Godot;
using System;
using System.Collections.Generic;

public class UnitTurnRecord : IModelRecord<UnitModel>
{
    public int ID { get; private set; }
    public Color Color { get; private set; }
    public List<Vector2> Positions { get; private set; }
    public List<Vector2> OffsetsInHex { get; private set; }
    public List<float> Strengths { get; private set; }
    public List<float> Readinesses { get; private set; }
    public List<float> Supplies { get; private set; }
    public UnitModel Unit => Cache<UnitModel>.GetModel(ID);
    public UnitTurnRecord(UnitModel unit)
    {
        ID = unit.ID;
        Color = unit.Faction.PrimaryColor;
        Positions = new List<Vector2>();
        OffsetsInHex = new List<Vector2>();
        Strengths = new List<float>();
        Readinesses = new List<float>();
        Supplies = new List<float>();
    }
    public void WriteRound(int i)
    {
        var unit = Unit; 
        if(unit.Alive == false) return; 
        Positions.Add(unit.Hex.Coords);
        var units = unit.Hex.Units;
        int index = units.IndexOf(unit);
        OffsetsInHex.Add(GraphicsUtility.UnitIconOffsets[units.Count][index]);
        Strengths.Add(unit.Strength);
        Readinesses.Add(unit.Readiness);
        Supplies.Add(unit.Supply);
    }
}
