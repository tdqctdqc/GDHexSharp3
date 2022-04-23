using Godot;
using System;

public class UnitMouseInput : IMouseInputModule
{
    private UIEvents _events => Game.I.Session.Client.Events.UI;
    private MapHighlightManager _highlight => Game.I.Session.Client.Graphics.MapGraphics.MapHighlightManager;
    private bool _ctrl => Input.IsKeyPressed((int)KeyList.Control);
    public void HandleDeltaInput(float delta)
    {
        
    }
    public void Activate()
    {
        _events.MouseOverHexChanged += DrawUnitMovePath;
        _events.SelectedHexChanged += DrawUnitMovePath;
        _events.SelectedUnits += _highlight.HighlightSelectedUnitsMoveRadius;
    }
    public void Deactivate()
    {
        _events.MouseOverHexChanged -= DrawUnitMovePath;
        _events.SelectedHexChanged -= DrawUnitMovePath;
        _events.SelectedUnits -= _highlight.HighlightSelectedUnitsMoveRadius;
        _highlight.Clear();
        Game.I.Session.Client.Graphics.MapGraphics.PathDrawer.ClearPaths();
    }

    public void DrawUnitMovePath(HexModel h)
    {
        Game.I.Session.Client.Graphics.MapGraphics.PathDrawer.DrawUnitMovePath();
    }

    public void HandleInput(InputEventMouse input)
    {
        var mouse = Game.I.Session.Client.UI.Mouse;
        var camera = Game.I.Session.Client.Graphics.Camera;
        if(input is InputEventMouseButton)
        {
            var b = input as InputEventMouseButton;
            if(b.ButtonIndex == (int)ButtonList.Left && b.Pressed == false)
            {
                Game.I.Session.Client.UI.HexSelector.TrySelectHex();
                if(_ctrl)
                {
                    Game.I.Session.Client.UI.UnitSelector.TryMultiSelectUnit();
                }
                else
                {
                    Game.I.Session.Client.UI.UnitSelector.TrySelectUnit();
                }
            }
            if(b.ButtonIndex == (int)ButtonList.Right && b.Pressed == false)
            {
                Game.I.Session.Server.StateInterface.OrderManager.TryGiveGoToOrder();
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
