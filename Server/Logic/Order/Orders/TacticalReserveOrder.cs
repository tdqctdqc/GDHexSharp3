using Godot;
using System;
using System.Collections.Generic;

public class TacticalReserveOrder : IOrder
{
    public int ID {get; private set;}
    public string Name => "Tactical Reserve";
    public UnitModel Unit { get; private set; }
    public HexModel ReserveHex { get; private set; }
    private List<int> _path; 
    public bool Completed => false;
    private float _storedAP; 
    public void Do(float ap, Logic logic)
    {
        _storedAP += ap;
        logic.Unit.UnitDoReserveLogic(Unit, ReserveHex, ref _storedAP, _path);
    }
}
