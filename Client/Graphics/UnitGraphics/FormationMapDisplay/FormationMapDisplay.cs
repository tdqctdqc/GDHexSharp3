using Godot;
using System;
using System.Collections.Generic;

public class FormationMapDisplay : Node2D
{
    private Line2D _units, _parent;
    private Events _events => Game.I.Session.Client.Events;
    private UnitGraphics _unitGraphics => Game.I.Session.Client.Graphics.MapGraphics.UnitGraphics;
    public override void _Ready()
    {
    }
    public void Setup()
    {
        _units = new Line2D();
        _units.Width = 10f; 
        _units.DefaultColor = new Color(0f,1f,1f,.5f);
        AddChild(_units);

        _parent = new Line2D();
        _parent.Width = 10f;
        _parent.DefaultColor = new Color(Colors.Orange,.5f);
        AddChild(_parent);
        
        _events.UI.SelectedUnits += (u) => DrawLines();
        _events.Turn.TurnDone += DrawLines;
    }
    public void DrawLines()
    {
        _units.ClearPoints();
        _parent.ClearPoints();
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;
        if (units.Count == 0) return;
        var formation = units[0].Formation;
        if (formation == null) return;
        foreach (var unit in units)
        {
            if (unit.FormationID != formation.ID) return;
        }
        var formUnits = formation.Units;
        var hq = formation.HQ;
        var graphic = _unitGraphics.GetUnitGraphic(hq);
        if(graphic == null) return;
        var hqPos = graphic.Position;

        DrawSubUnitLines(formUnits, hqPos);
        DrawSubFormLines(formation, hqPos);
        DrawParentFormLines(ref formation, hqPos);
    }

    private void DrawParentFormLines(ref FormationModel formation, Vector2 hqPos)
    {
        var parent = formation.Parent;
        if (parent == null) return;
        _parent.AddPoint(hqPos);

        while (parent != null)
        {
            var graphic = _unitGraphics.GetUnitGraphic(parent.HQ);
            if(graphic == null) break;
            var parentHQPos = graphic.Position;
            _parent.AddPoint(parentHQPos);
            formation = parent;
            parent = parent.Parent;
        }
    }

    private void DrawSubFormLines(FormationModel formation, Vector2 hqPos)
    {
        var subForms = formation.Subformations;
        foreach (var sub in subForms)
        {
            var graphic = _unitGraphics.GetUnitGraphic(sub.HQ);
            if(graphic == null) continue;
            var subHQpos = graphic.Position;
            _parent.AddPoint(subHQpos);
            _parent.AddPoint(hqPos);
        }
    }

    private void DrawSubUnitLines(List<UnitModel> formUnits, Vector2 hqPos)
    {
        foreach (var unit in formUnits)
        {
            var graphic = _unitGraphics.GetUnitGraphic(unit);
            if(graphic == null) continue;
            _units.AddPoint(hqPos);
            var unitPos = graphic.Position;
            _units.AddPoint(unitPos);
        }
    }
}
