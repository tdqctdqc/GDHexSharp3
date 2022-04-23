using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class HexBorderInput 
{
    private IEditor _editor => Game.I.Session.Editor;
    private UIEvents _events => Game.I.Session.Client.Events.UI;
    private MapHighlightManager _highlight => Game.I.Session.Client.Graphics.MapGraphics.MapHighlightManager;
    private MouseController _mouse => Game.I.Session.Client.UI.Mouse;
    private List<int> _dragPath;
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
            var hexes = new List<HexModel>();
            for (int i = 0; i < _dragPath.Count; i++)
            {
                var pairIndex = _dragPath[i];
                var hexTuple = pairIndex.GetHexesFromPairIndex();
                hexes.Add(hexTuple.Item1);
                hexes.Add(hexTuple.Item2);
            }
            brush.Stroke(hexes);
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
            var mousePos = Game.I.Session.Client.UI.Mouse.MousePos;
            var hexes = mousePos.FindTwoClosestHexes();
            var mouseHex = hexes.Item1;
            var secondClosest = hexes.Item2;
            _dragPath = new List<int>(){mouseHex.GetHexPairIndex(secondClosest)};
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
        int pairID = hexes.Item1.GetHexPairIndex(hexes.Item2);
        var list = new List<int>(){pairID};
        _highlight.HighlightHexBorders(list);
    }
    private void DrawBrushPath()
    {
        var mouseOver = Game.I.Session.Client.UI.HexSelector.MouseOverHex;
        var secondClosest = Game.I.Session.Client.UI.HexSelector.SecondClosestHex;
        int pairID = mouseOver.GetHexPairIndex(secondClosest);
        var last = _dragPath.Last();
        if(_dragPath.Contains(pairID))
        {
            int i = _dragPath.IndexOf(pairID);
            _dragPath = _dragPath.GetRange(0, i + 1);
        }
        else if(pairID.HexPairsTouch(last) == true)
        {
            if(_dragPath.Count > 1)
            {
                var secondLast = _dragPath[_dragPath.Count - 2];
                if(pairID.HexPairsTouch(secondLast))
                {
                    _dragPath.Remove(last);
                    _dragPath.Add(pairID);
                }
                else
                {
                    _dragPath.Add(pairID);
                }
            }
            else
            {
                _dragPath.Add(pairID);
            }
        }
        else
        {
            var path1 = Game.I.Session.Utility.PathFinder.FindShortestPathEdgewise(last, pairID);
            path1.RemoveAt(0);
            _dragPath.AddRange(path1);
        }
        if(_dragPath != null) _highlight.HighlightHexBorders(_dragPath);
    }
}
