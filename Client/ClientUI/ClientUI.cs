using Godot;
using System;

public class ClientUI : Node2D
{
    public MouseController Mouse { get; private set; }
    public HexSelector HexSelector { get; private set; }
    public UnitSelector UnitSelector { get; private set; }
    public Ticker Ticker { get; set; }
    public MapUI MapUI { get; private set; }
    public UIModeManager UIModeManager { get; private set; }
    
    private float _tickPeriod = .2f;
    public override void _Ready()
    {
        Mouse = new MouseController();
        AddChild(Mouse);

        Ticker = new Ticker();
        Ticker.Setup(_tickPeriod);
        AddChild(Ticker);

        HexSelector = new HexSelector();
        UnitSelector = new UnitSelector();
        MapUI = Scenes.MapUI;
        AddChild(MapUI);

        UIModeManager = new UIModeManager();
    }

    public void Setup()
    {
        HexSelector.Setup();
        UnitSelector.Setup();
        MapUI.Setup();
    }
}
