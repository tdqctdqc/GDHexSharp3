using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class Editor : IEditor
{
    public List<IEditorAction> PastActions {get; private set;}
    public List<IEditorAction> FutureActions {get; private set;}
    public IBrush CurrentBrush {get; private set;}
    public HexBrush<FactionModel> FactionBrush {get; private set;}
    public HexBrush<Terrain> TerrainBrush {get; private set;}
    public BoundaryBrush<RoadType> RoadBrush { get; private set; }
    public BoundaryBrush<RiverType> RiverBrush { get; private set; }
    public bool FillMode {get; set;}
    public int BrushRadius {get; set;}
    public void Setup()
    {
        PastActions = new List<IEditorAction>();
        FutureActions = new List<IEditorAction>();
        SetupBrushes();
    }

    private void SetupBrushes()
    {
        FactionBrush = Brush.GetFactionBrush();
        TerrainBrush = Brush.GetTerrainBrush();
        RoadBrush = Brush.GetRoadBrush();
        RiverBrush = Brush.GetRiverBrush();
        BrushRadius = 0;
    }
    public void SetBrush(IBrush brush)
    {
        CurrentBrush = brush; 
    }
    public void Stroke(Vector2 clickPos)
    {
        if(CurrentBrush == null) return; 

        if(CurrentBrush is IHexBrush) 
        {
            var hex = clickPos.FindHexFromWorldPos();
            if(hex == null) return;
            var hexBrush = CurrentBrush as IHexBrush;
            hexBrush.Stroke(GetStrokeHexes(clickPos));
        }
        else if(CurrentBrush is IBoundaryBrush)
        {
            var boundaryBrush = CurrentBrush as IBoundaryBrush;
            boundaryBrush.Stroke(GetStrokeHexes(clickPos));
        }
    }
    public List<HexModel> GetStrokeHexes(Vector2 clickPos)
    {
        var list = new List<HexModel>();
        if(CurrentBrush is IHexBrush)
        {
            var strokeHex = clickPos.FindHexFromWorldPos();
            if(FillMode)
            {
                
            }
            else
            {
                list.AddRange(strokeHex.GetHexesInRadius(BrushRadius));
            }
        }
        else if(CurrentBrush is IBoundaryBrush)
        {
            var hexes = clickPos.FindTwoClosestHexes();
            list.Add(hexes.Item1);
            list.Add(hexes.Item2);
        }
        return list; 
    }
    public void DoAction(IEditorAction action)
    {
        FutureActions = new List<IEditorAction>();
        PastActions.Add(action);
        action.DoAction();
    }
    public void Undo()
    {
        if(PastActions.Count == 0) return;
        var lastAction = PastActions[PastActions.Count - 1];
        PastActions.Remove(lastAction);
        var undoAction = lastAction.GetUndoAction();
        undoAction.DoAction();
        FutureActions.Insert(0, undoAction);
    }
    public void Redo()
    {
        if(FutureActions.Count == 0) return;
        var nextAction = FutureActions[0];
        FutureActions.Remove(nextAction);
        var redoAction = nextAction.GetUndoAction();
        redoAction.DoAction();
        PastActions.Add(redoAction);
    }
}
