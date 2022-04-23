using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class ReplayUnitGraphics : Node2D
{
    private List<UnitTurnRecord> _records;
    private Dictionary<UnitTurnRecord, UnitGraphic> _graphics; 
    public override void _Ready()
    {
    }

    public void Set(List<UnitTurnRecord> records)
    {
        Clear();
        _records = records;
        _graphics = new Dictionary<UnitTurnRecord, UnitGraphic>();
        foreach (var record in records)
        {
            var graphic = Scenes.UnitGraphic;
            AddChild(graphic);

            graphic.Setup(record.Unit);
            _graphics.Add(record, graphic);
        }
    }
    public void ShowRound(int i)
    {
        foreach (var item in _graphics)
        {
            var graphic = item.Value;
            var record = item.Key;
            if(record.Positions.Count < i + 1) 
            {
                Visible = false; 
            }
            else
            {
                graphic.SetStats(record.Strengths[i], record.Readinesses[i], record.Supplies[i]);
                var coords = record.Positions[i];
                graphic.Position = coords.GetWorldPosFromOffset() + record.OffsetsInHex[i];
            }
        }
    }
    private void Clear()
    {
        if(_graphics == null) return; 
        foreach (var item in _graphics)
        {
            item.Value.Free();
        }
    }
}
