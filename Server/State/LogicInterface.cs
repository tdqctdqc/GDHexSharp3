using Godot;
using System;

public class LogicInterface
{
    private Logic _logic;
    public RoadInterface Road { get; private set; }
    public RiverInterface River { get; private set; }
    public UnitInterface Unit { get; private set; }
    public FormationInterface Formation { get; private set; }
    public LogicInterface(Logic logic)
    {
        _logic = logic;
        Road = new RoadInterface(logic);
        River = new RiverInterface(logic);
        Formation = new FormationInterface(logic);
        Unit = new UnitInterface();
    }
}
