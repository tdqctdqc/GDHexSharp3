using Godot;
using System;

public class SelectedHexPanel : Node
{
    private HexPanel _selected, _mouseOver; 
    public override void _Ready()
    {
        _selected = GetNode<HexPanel>("Selected");
        _mouseOver = GetNode<HexPanel>("MouseOver");

    }

    public void Setup()
    {
        Game.I.Session.Client.Events.UI.MouseOverHexChanged += _mouseOver.Setup;
        Game.I.Session.Client.Events.UI.SelectedHexChanged += _selected.Setup;
    }
}
