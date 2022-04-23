using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class DefendOrder : IOrder
{
    public int ID {get; private set;}
    public string Name => "Defend";
    public UnitModel Unit {get; private set;}
    public bool Completed => false;
    public HexModel Target { get; private set; }
    private float _storedAP;
    private List<int> _path; 
    public List<HexModel> Path => Cache<HexModel>.GetModels(_path);

    public DefendOrder(UnitModel unit, HexModel defendTarget, List<int> path)
    {
        Target = defendTarget;
        Unit = unit; 
        ID = unit.ID;
        _path = path.ToList();
    }
    public void Do(float ap, Logic logic)
    {
        _storedAP += ap;
        if(Unit.Hex != Target)
        {
            logic.Unit.UnitDoMoveLogic(Unit, _path, ref _storedAP);
        }
    }
}
