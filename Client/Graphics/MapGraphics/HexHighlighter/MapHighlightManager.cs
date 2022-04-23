using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class MapHighlightManager 
{
    private HexHighlighter _highlightHexes => Game.I.Session.Client.Graphics.MapGraphics.HexHighlighter;
    private HexBorderHighlighter _highlightBorder => Game.I.Session.Client.Graphics.MapGraphics.HexBorderHighlighter;
    private Color _highlightColor = new Color(0f, 0f, .5f, .25f); 
    //MAKE ASYNC
    public void HighlightSelectedUnitsMoveRadius(List<UnitModel> units)
    {
        _highlightBorder.Clear();
        
        var h = _highlightHexes;
        var toHighlight = CacheManager.AI.GetUnitsMoveRadius(units);
        h.ClearAndHighlightHexes(toHighlight, (h) => _highlightColor);
    }
    public void HighlightBrushHexes(Vector2 mousePos)
    {
        _highlightBorder.Clear();

        var hexes = Game.I.Session.Editor.GetStrokeHexes(mousePos);
        _highlightHexes.ClearAndHighlightHexes(hexes, (h) => _highlightColor);
    }
    public void HighlightHexes(List<HexModel> hexes)
    {
        _highlightBorder.Clear();
        _highlightHexes.ClearAndHighlightHexes(hexes, (h) => _highlightColor);

    }
    public void HighlightHexBorders(List<int> hexPairIDs)
    {
        _highlightHexes.Clear();
        _highlightBorder.ClearAndHighlightHexBorders(hexPairIDs, _highlightColor);
    }
    public void Clear()
    {
        _highlightHexes.Clear();
        _highlightBorder.Clear();
    }


    public void HighlightByCell()
    {
        var hexes = Cache<HexModel>.GetModels();
        Func<HexModel,Color> colorFunc = (h) => 
        {
            return h.GetCell().Color;
        };
        _highlightHexes.ClearAndHighlightHexes(hexes, colorFunc);
    }
    public void HighlightByPlate()
    {
        var hexes = Cache<HexModel>.GetModels();
        Func<HexModel,Color> colorFunc = (h) => 
        {
            return h.GetCell().Plate.Color;
        };
        _highlightHexes.ClearAndHighlightHexes(hexes, colorFunc);
    }
    public void HighlightByContinent()
    {
        var hexes = Cache<HexModel>.GetModels();
        Func<HexModel,Color> colorFunc = (h) => 
        {
            return h.GetCell().Plate.Continent.Color;
        };
        _highlightHexes.ClearAndHighlightHexes(hexes, colorFunc);
    }


    public void HighlightByNeighbors<T>(Func<HexModel, T> getType, Func<HexModel, List<T>> getNeighbors) where T : IGeologyNode
    {
        var hex = Game.I.Session.Client.UI.HexSelector.SelectedHex;
        if(hex == null) return; 
        var hexes = Cache<HexModel>.GetModels();
        var neighbors = getNeighbors(hex);

        Func<HexModel,Color> colorFunc = (h) => 
        {
            var hType = getType(h);
            if(hType.Equals(getType(hex))) return Colors.Black;
            else if(neighbors.Contains(hType)) return hType.SelectColor;
            else return hType.Color; 
        };
        _highlightHexes.ClearAndHighlightHexes(hexes, colorFunc);
    }
}
