using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HexWargame;

public class BoundaryBrush<T> : IBoundaryBrush
{
    public T Current {get; set;}
    public bool LinkNotBorder { get; private set; }
    public Func<List<HexModel>, IEditorAction> StrokeFunc {get; set;}
    public Func<List<HexModel>, IEditorAction> PathStrokeFunc {get; set;}
    public Func<HexModel, bool> Condition { get; private set; }
    public BoundaryBrush(Func<List<HexModel>, IEditorAction> strokeFunc, 
                            Func<List<HexModel>, IEditorAction> pathStrokeFunc, 
                            Func<HexModel, bool> condition, bool linkNotBorder)
    {
        LinkNotBorder = linkNotBorder;
        Condition = condition; 
        StrokeFunc = strokeFunc;
        PathStrokeFunc = pathStrokeFunc;
    }

    public void Stroke(HexModel h1, HexModel h2)
    {
        if(h1 == null) return;
        if(h2 == null) return;
        if(Condition(h1) == false) return;
        if(Condition(h2) == false) return;
        if(h1.GetHexDistance(h2) != 1) return;
        var hexes = new List<HexModel>(){h1,h2};
        Game.I.Session.Editor.DoAction(StrokeFunc(hexes));
    }
    public void Stroke(List<HexModel> path)
    {
        Game.I.Session.Editor.DoAction(PathStrokeFunc(path));
    }

    public void FillStroke(HexModel hex)
    {
        return; 
    }
}
