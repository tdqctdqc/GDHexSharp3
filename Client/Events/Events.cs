using Godot;
using System;
using System.Collections.Generic;

public class Events
{
    public MapEvents Map { get; private set; }
    public UIEvents UI { get; private set; }
    public TurnEvents Turn { get; private set; }
    public OrderEvents Order { get; private set; }
    public Events()
    {
        Map = new MapEvents();
        UI = new UIEvents();
        Turn = new TurnEvents();
        Order = new OrderEvents();
    }
}
