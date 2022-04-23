using Godot;
using System;
using System.Collections.Generic;

public class BaseMesh : MultiMeshInstance2D
{
    private Func<HexModel, Color> _colorFunc; 
    public BaseMesh(Func<HexModel, Color> colorFunc)
    {
        _colorFunc = colorFunc; 
    }
    public void SetColorFunc(Func<HexModel, Color> colorFunc)
    {
        _colorFunc = colorFunc;
        UpdateHexes(Cache<HexModel>.GetModels());
    }
    public override void _Ready()
    {
        Multimesh = new MultiMesh();
        var hexMesh = new HexMesh();
        Multimesh.Mesh = hexMesh; 
        Multimesh.ColorFormat = MultiMesh.ColorFormatEnum.Float;
    }
    public void UpdateHex(HexModel hex)
    {
        Multimesh.SetInstanceColor(hex.ID - 1, _colorFunc(hex));
    }
    public void UpdateHexes(List<HexModel> hexes)
    {
        foreach (var hex in hexes)
        {
            Multimesh.SetInstanceColor(hex.ID - 1, _colorFunc(hex));
        }
    }
    public void Setup(List<HexModel> hexes)
    {
        Multimesh.InstanceCount = 0;
        Multimesh.InstanceCount = hexes.Count;
        foreach (var hex in hexes)
        {
            Vector2 pos = hex.WorldPos;
            Multimesh.SetInstanceTransform2d(hex.ID - 1, new Transform2D(0f, pos));
            Multimesh.SetInstanceColor(hex.ID - 1, _colorFunc(hex));
        }
        
        Cache<HexModel>.ModelsChanged += UpdateHexes; 
    }
}
