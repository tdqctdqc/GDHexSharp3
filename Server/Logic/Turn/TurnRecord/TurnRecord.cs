using Godot;
using System;
using System.Collections.Generic;

public class TurnRecord 
{
    public List<UnitTurnRecord> UnitRecords { get; private set; }
    public TurnRecord()
    {
        UnitRecords = new List<UnitTurnRecord>();
        int numUnits = Cache<UnitModel>.Size;

        var units = Cache<UnitModel>.GetModels(); 
        for (int i = 0; i < numUnits; i++)
        {
            var unit = units[i];
            UnitRecords.Add(new UnitTurnRecord(unit));
        }
    }
    public void WriteRound(int i)
    {
        foreach (var unit in UnitRecords)
        {
            if(unit.Unit.Alive) unit.WriteRound(i);
        }
    }
}
