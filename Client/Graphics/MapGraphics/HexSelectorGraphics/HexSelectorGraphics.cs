using Godot;
using System;

public class HexSelectorGraphics : Sprite
{
    public override void _Ready()
    {
        Texture = (Texture)GD.Load("res://Client/Graphics/MapGraphics/HexSelectorGraphics/hexOutline.svg");
    }

    public void Setup()
    {
        Game.I.Session.Client.Events.UI.SelectedHexChanged += SelectHex;

    }

    public void SelectHex(HexModel hex)
    {
        Position = hex.WorldPos;
    }
}
