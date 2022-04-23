using Godot;
using System;

public class MapMouseInput : IMouseInputModule
{
    public void HandleDeltaInput(float delta)
    {
    }
    public void Activate()
    {
        
    }
    public void Deactivate()
    {
        
    }
    public void HandleInput(InputEventMouse input)
    {
        var camera = Game.I.Session.Client.Graphics.Camera;
        if(input is InputEventMouseButton)
        {
            var b = input as InputEventMouseButton;
            if(b.ButtonIndex == (int)ButtonList.Left)
            {
                Game.I.Session.Client.UI.HexSelector.TrySelectHex();
            }
            if(b.ButtonIndex == (int)ButtonList.WheelUp)
            {
                camera.ZoomIn();
            }
            else if(b.ButtonIndex == (int)ButtonList.WheelDown)
            {
                camera.ZoomOut();
            }
        }
    }
}
