using Godot;
using System;
using System.Collections.Generic;

public interface IEditor
{
    void Setup();
    void Stroke(Vector2 clickPos);
    IBrush CurrentBrush {get;}
    void SetBrush(IBrush brush);
    void DoAction(IEditorAction action);
    List<HexModel> GetStrokeHexes(Vector2 clickPos);
    HexBrush<FactionModel> FactionBrush {get;}
    HexBrush<Terrain> TerrainBrush {get;}
    BoundaryBrush<RoadType> RoadBrush {get;}
    BoundaryBrush<RiverType> RiverBrush {get;}
    bool FillMode {get; set;}
    int BrushRadius {get; set;}
    void Undo();
    void Redo();
}
