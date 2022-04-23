using Godot;
using System;

public class HexBrushInput : Node
{
    private MapHighlightManager _highlight => Game.I.Session.Client.Graphics.MapGraphics.MapHighlightManager;
    public void Activate()
    {

    }
    public void Deactivate()
    {

    }
    public void DrawBrushFootprint(Vector2 mousePos)
    {
        _highlight.HighlightBrushHexes(mousePos);
    }
    public void HandleDeltaInput(float delta)
    {

    }
    public void HandleRMB()
    {
        var mousePos = Game.I.Session.Client.UI.Mouse.MousePos;
        Game.I.Session.Editor.Stroke(mousePos);
    }
}
