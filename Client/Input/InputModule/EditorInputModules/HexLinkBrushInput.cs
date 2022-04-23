using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class HexLinkBrushInput 
{
    private IEditor _editor => Game.I.Session.Editor;
    private UIEvents _events => Game.I.Session.Client.Events.UI;
    private MapHighlightManager _highlight => Game.I.Session.Client.Graphics.MapGraphics.MapHighlightManager;
    private MouseController _mouse => Game.I.Session.Client.UI.Mouse;
    
    
    private int _rightClickHexID;
    private List<HexModel> _dragPath;
    public void Activate()
    {
        
    }
    public void Deactivate()
    {
        
    }
    public void HandleRMB()
    {
        var brush = _editor.CurrentBrush as IBoundaryBrush;
        if(_mouse.HoldingRMB)
        {
            brush.Stroke(_dragPath);
        }
        else
        {
            var hexSel = Game.I.Session.Client.UI.HexSelector;
            var h1 = hexSel.MouseOverHex;
            var h2 = hexSel.SecondClosestHex;
            brush.Stroke(h1, h2);
        }
    }
    public void HandleDeltaInput(float delta)
    {
        if(Input.IsActionJustPressed("rightClick"))
        {
            var mouseHex = Game.I.Session.Client.UI.HexSelector.MouseOverHex;
            if(mouseHex != null) _rightClickHexID = mouseHex.ID;
            _dragPath = new List<HexModel>(){mouseHex};
        }
        if(Input.IsActionJustReleased("rightClick"))
        {
        }
        if(Input.IsActionPressed("rightClick"))
        {
        }
        else
        {
        }
    }
    public void DrawBrushFootprint(Vector2 mousePos)
    {
        if(_mouse.HoldingRMB)
        {
            DrawBrushPath();
            return; 
        }
        var hexes = mousePos.FindTwoClosestHexes();
        if(hexes.Item1 == null || hexes.Item2 == null) return;
        var list = new List<HexModel>(){hexes.Item1 , hexes.Item2};
        _highlight.HighlightHexes(list);
    }
    private void DrawBrushPath()
    {
        var mouseOver = Game.I.Session.Client.UI.HexSelector.MouseOverHex;

        var last = _dragPath.Last();
        if(_dragPath.Contains(mouseOver))
        {
            int i = _dragPath.IndexOf(mouseOver);
            _dragPath = _dragPath.GetRange(0, i + 1);
        }
        else if(mouseOver.GetHexDistance(last) == 1)
        {
            _dragPath.Add(mouseOver);
        }
        else
        {
            var path1 = Game.I.Session.Utility.PathFinder.FindShortestPathHexwise(last, mouseOver);
            path1.RemoveAt(0);
            _dragPath.AddRange(path1);
        }
        
        if(_dragPath != null) _highlight.HighlightHexes(_dragPath);
    }
}
