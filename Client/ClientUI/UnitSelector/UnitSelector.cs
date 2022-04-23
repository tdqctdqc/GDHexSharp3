using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class UnitSelector
{
    public List<int> SelectedUnitsIDs { get; private set; }
    public UnitModel Current => GetCurrent();
    public List<UnitModel> CurrentList => Cache<UnitModel>.GetModels(SelectedUnitsIDs);
    private UIEvents _events => Game.I.Session.Client.Events.UI;
    private UnitGraphics _graphics => Game.I.Session.Client.Graphics.MapGraphics.UnitGraphics;
    public void Setup()
    {
        SelectedUnitsIDs = new List<int>();
    }
    private UnitModel GetCurrent()
    {
        if(SelectedUnitsIDs.Count == 0) return null;

        return Cache<UnitModel>.GetModel(SelectedUnitsIDs[0]);
    }
    public void TrySelectUnit()
    {
        DeselectUnits();
        Vector2 clickPos = Game.I.Session.Client.Graphics.GetGlobalMousePosition();
        var unit = _graphics.GetUnitFromClickPos(clickPos);
        
        if(unit != null) SelectedUnitsIDs.Add(unit.ID);
        
        var units = Cache<UnitModel>.GetModels(SelectedUnitsIDs);
        _events.SelectedUnits?.Invoke(units);
    }

    public void TryMultiSelectUnit()
    {
        Vector2 clickPos = Game.I.Session.Client.Graphics.GetGlobalMousePosition();
        var unit = _graphics.GetUnitFromClickPos(clickPos);
        if(unit == null) return;

        if(SelectedUnitsIDs.Contains(unit.ID) == false) SelectedUnitsIDs.Add(unit.ID);
        else 
        {
            SelectedUnitsIDs.Remove(unit.ID);
            _events.DeselectedUnits?.Invoke(new List<UnitModel>(){unit});
        }

        var units = Cache<UnitModel>.GetModels(SelectedUnitsIDs);
        _events.SelectedUnits?.Invoke(units);
    }

    private void DeselectUnits()
    {
        var units = Cache<UnitModel>.GetModels(SelectedUnitsIDs);
        _events.DeselectedUnits?.Invoke(units);
        SelectedUnitsIDs = new List<int>();
    }

    public void DeselectUnit(UnitModel unit)
    {
        SelectedUnitsIDs.Remove(unit.ID);
        var units = Cache<UnitModel>.GetModels(SelectedUnitsIDs);
        _events.SelectedUnits?.Invoke(units);
        _events.DeselectedUnits?.Invoke(new List<UnitModel>(){unit});
    }
}
