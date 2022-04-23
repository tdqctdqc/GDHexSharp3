using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class UnitGraphics : Node2D
{
    private Dictionary<int, UnitGraphic> _graphicsByID; 
    private Dictionary<int, List<UnitGraphic>> _hexes; 
    private Dictionary<UnitGraphic, int> _graphicsHexes; 
    private Dictionary<UnitGraphic, int> _graphicsUnits; 
    private Events _events => Game.I.Session.Client.Events;

    public override void _Ready()
    {
    }
    public void Setup()
    {
        CacheManager.LoadedState += LoadState;
        _events.Turn.NewTurn += UpdateAllModels;
    }
    public void LoadState()
    {
        _graphicsByID = new Dictionary<int, UnitGraphic>();
        _graphicsHexes = new Dictionary<UnitGraphic, int>();
        _graphicsUnits = new Dictionary<UnitGraphic, int>();
        _hexes = new Dictionary<int, List<UnitGraphic>>();
        while(GetChildCount() > 0) GetChild(0).Free();

        var units = Cache<UnitModel>.GetModels();
        foreach (var unit in units)
        {
            AddGraphic(unit, unit.Hex);
        }
        Cache<UnitModel>.ModelsAdded += ModelsChanged;
        Cache<UnitModel>.ModelsChanged += ModelsChanged;
        Cache<UnitModel>.ModelsDeleted += ModelsDeleted;
        _events.UI.SelectedUnits += SelectUnits;
        _events.UI.DeselectedUnits += DeselectUnits;
    }
    public void SelectUnits(List<UnitModel> units)
    {
        foreach (var unit in units)
        {
            _graphicsByID[unit.ID].Select();
        }
    }
    public void DeselectUnits(List<UnitModel> units)
    {
        foreach (var unit in units)
        {
            _graphicsByID[unit.ID].Deselect();
        }
    }
    public UnitModel GetUnitFromClickPos(Vector2 clickPos)
    {
        var hex = clickPos.FindHexFromWorldPos();
        if(hex == null) return null; 
        if(_hexes.ContainsKey(hex.ID) == false) return null; 
        if(_hexes[hex.ID].Count == 0) return null; 
        var graphics = _hexes[hex.ID];
        var closeGraphic = graphics.OrderBy(g => g.Position.DistanceTo(clickPos)).First();
        int id = _graphicsUnits[closeGraphic];

        return Cache<UnitModel>.GetModel(id);
    }
    private void AddGraphic(UnitModel model, HexModel hex)
    {
        var graphic = Scenes.UnitGraphic;
        AddChild(graphic);

        graphic.Setup(model);
        if(graphic == null) GD.Print("graphic");
        if(hex == null) GD.Print("hex");
        _graphicsHexes.Add(graphic, hex.ID);
        
        AddGraphicToHex(graphic, model.Hex);
        _graphicsByID.Add(model.ID, graphic);
        _graphicsUnits.Add(graphic, model.ID);
    }
    private void AddGraphicToHex(UnitGraphic graphic, HexModel hex)
    {
        var oldHex = _graphicsHexes[graphic];
        if(_hexes.ContainsKey(oldHex) == false) _hexes.Add(oldHex, new List<UnitGraphic>());
        _hexes[oldHex].Remove(graphic);

        if(_hexes.ContainsKey(hex.ID) == false) _hexes.Add(hex.ID, new List<UnitGraphic>());
        _hexes[hex.ID].Add(graphic);
        _graphicsHexes[graphic] = hex.ID; 

        ArrangeGraphicsInHex(Cache<HexModel>.GetModel(oldHex));
        ArrangeGraphicsInHex(hex);
    }
    private void ArrangeGraphicsInHex(HexModel hex)
    {
        var graphics = _hexes[hex.ID];
        var offsets = GraphicsUtility.UnitIconOffsets[graphics.Count];
        for(int i = 0; i < graphics.Count; i++)
        {
            var graphic = graphics[i];
            graphic.Position = hex.WorldPos + offsets[i];
        }
    }
    public void UpdateAllModels()
    {
        ModelsChanged(Cache<UnitModel>.GetModels());
    }
    public void ModelsChanged(List<UnitModel> unitModels)
    {
        foreach (var model in unitModels)
        {
            if(_graphicsByID.ContainsKey(model.ID) == false)
            {
                if(model.Alive) AddGraphic(model, model.Hex);
            }
            else 
            {
                var graphic = _graphicsByID[model.ID];
                if(model.Alive)
                {
                    graphic.Setup(model);
                    AddGraphicToHex(graphic, model.Hex);
                }
                else
                {
                    RemoveGraphic(model.ID);
                }
            }
        }
    }
    public void ModelsDeleted(List<int> modelIDs)
    {
        var hexesToUpdate = new HashSet<HexModel>();
        foreach (var id in modelIDs)
        {
            if(_graphicsByID.ContainsKey(id) == false) continue; 
            var graphic = _graphicsByID[id];
            var hexID = _graphicsHexes[graphic];
            var hex = Cache<HexModel>.GetModel(hexID);
            hexesToUpdate.Add(hex);
            RemoveGraphic(id);
        }
        foreach (var hex in hexesToUpdate)
        {
            ArrangeGraphicsInHex(hex);
        }
    }
    private void RemoveGraphic(int unitID)
    {
        var graphic = _graphicsByID[unitID];
        var hex = _graphicsHexes[graphic];
        _hexes[hex].Remove(graphic);
        _graphicsHexes.Remove(graphic);
        _graphicsByID.Remove(unitID);
        _graphicsUnits.Remove(graphic);
        graphic.Free();
    }
    public UnitGraphic GetUnitGraphic(UnitModel unit)
    {
        if(_graphicsByID.ContainsKey(unit.ID))
        {
            return _graphicsByID[unit.ID];
        }
        return null;
    }
}
