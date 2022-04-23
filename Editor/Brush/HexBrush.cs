using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class HexBrush<T> : IHexBrush
{
    public T Current {get; set;}
    public Func<List<HexModel>, IEditorAction> StrokeFunc {get; set;}
    public Func<HexModel, bool> Condition { get; private set; }
    
    public HexBrush(Func<List<HexModel>, IEditorAction> strokeFunc, Func<HexModel, bool> condition)
    {
        Condition = condition; 
        StrokeFunc = strokeFunc;
    }

    public void Stroke(List<HexModel> strokeHexes)
    {
        var hexes = strokeHexes.Where(h => Condition(h)).ToList();

        Game.I.Session.Editor.DoAction(StrokeFunc(hexes));
    }

    public void FillStroke(HexModel hex)
    {

    }
}
