using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HexWargame;

public class EditorMouseInputModule : IMouseInputModule
{
    private UIEvents _events => Game.I.Session.Client.Events.UI;
    private IEditor _editor => Game.I.Session.Editor;
    private MapHighlightManager _highlight => Game.I.Session.Client.Graphics.MapGraphics.MapHighlightManager;
    private HexLinkBrushInput _linkInput;
    private HexBorderInput _borderInput; 
    private HexBrushInput _hexInput; 
    public EditorMouseInputModule()
    {
        _linkInput = new HexLinkBrushInput();
        _hexInput = new HexBrushInput();
        _borderInput = new HexBorderInput();
    }
    public void Activate()
    {
        _events.MouseOverHexChanged += DrawBrushFootprint;
        _events.SecondClosestHexChanged += DrawBrushFootprint;
        _linkInput.Activate();
        _hexInput.Activate();
        _borderInput.Activate();
        _highlight.Clear();
    }
    public void Deactivate()
    {
        _events.MouseOverHexChanged -= DrawBrushFootprint;
        _events.SecondClosestHexChanged -= DrawBrushFootprint;
        _linkInput.Deactivate();        
        _hexInput.Deactivate();
        _borderInput.Deactivate();
        _highlight.Clear();
    }
    public void HandleDeltaInput(float delta)
    {
        if(_editor.CurrentBrush is IBoundaryBrush)
        {
            var b = _editor.CurrentBrush as IBoundaryBrush;
            if(b.LinkNotBorder) _linkInput.HandleDeltaInput(delta);
            else _borderInput.HandleDeltaInput(delta);
        }
        else if(_editor.CurrentBrush is IHexBrush)
        {
            _hexInput.HandleDeltaInput(delta);
        }
    }
    private void DrawBrushFootprint(HexModel _)
    {
        if(_editor.CurrentBrush == null) return; 
        var mousePos = Game.I.Session.Client.UI.Mouse.MousePos;
        if(_editor.CurrentBrush is IHexBrush) _hexInput.DrawBrushFootprint(mousePos);
        else if(_editor.CurrentBrush is IBoundaryBrush) 
        {
            var e = _editor.CurrentBrush as IBoundaryBrush;
            if(e.LinkNotBorder) _linkInput.DrawBrushFootprint(mousePos);
            else _borderInput.DrawBrushFootprint(mousePos);
        }
    }
    public void HandleInput(InputEventMouse input)
    {
        var camera = Game.I.Session.Client.Graphics.Camera;
        if(input is InputEventMouseButton)
        {
            var b = input as InputEventMouseButton;
            if(b.ButtonIndex == (int)ButtonList.Left && b.Pressed == false)
            {
                Game.I.Session.Client.UI.HexSelector.TrySelectHex();
            }
            else if(b.ButtonIndex == (int)ButtonList.Right)
            {
                HandleRMB(b);
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

    private void HandleRMB(InputEventMouseButton b)
    {
        if(b.Pressed == false)
        {
            if(_editor.CurrentBrush is IBoundaryBrush)
            {
                var e = _editor.CurrentBrush as IBoundaryBrush;
                if(e.LinkNotBorder) _linkInput.HandleRMB();
                else _borderInput.HandleRMB();
               
            }
            else if(_editor.CurrentBrush is IHexBrush)
            {
                _hexInput.HandleRMB();
            }
        }
    }
}
