using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class HexHighlighter : MultiMeshInstance2D
{
    private Dictionary<int, int> _hexIDToInstance;
    private List<int> _highlightedInstances; 
    public void Setup(List<HexModel> hexes)
    {
        _highlightedInstances = new List<int>();
        _hexIDToInstance = new Dictionary<int, int>();
        Multimesh = new MultiMesh();
        var hexMesh = new HexMesh();
        Multimesh.Mesh = hexMesh;
        Multimesh.ColorFormat = MultiMesh.ColorFormatEnum.Float;
        Multimesh.InstanceCount = 0;

        Multimesh.InstanceCount = hexes.Count;

        for (int i = 0; i < hexes.Count; i++)
        {
            var hex = hexes[i];
            _hexIDToInstance.Add(hex.ID, i);
            Multimesh.SetInstanceTransform2d(i, new Transform2D(0f, hex.WorldPos));
            Multimesh.SetInstanceColor(i, Colors.Transparent);
        }
    }
    public void ClearAndHighlightHexes(Func<HexModel, bool> predicate, Func<HexModel, Color> colorFunc)
    {
        var hexes = Cache<HexModel>.GetModels();
        Multimesh.InstanceCount = 0;
        foreach (var hex in hexes)
        {
            int instanceID = _hexIDToInstance[hex.ID];

            if(predicate(hex))
            {
                Multimesh.SetInstanceColor(instanceID, colorFunc(hex));
            }
            else
            {
                Multimesh.SetInstanceColor(instanceID, Colors.Transparent);
            }
        }
    }
    public void ClearAndHighlightHexes(List<HexModel> hexesToHighlight, Func<HexModel, Color> colorFunc)
    {
        foreach (var highlighted in _highlightedInstances)
        {
            Multimesh.SetInstanceColor(highlighted, Colors.Transparent);
        }
        foreach (var toHighlight in hexesToHighlight)
        {
            int instance = _hexIDToInstance[toHighlight.ID];
            Multimesh.SetInstanceColor(instance, colorFunc(toHighlight));
        }
        _highlightedInstances = hexesToHighlight.Select(h => _hexIDToInstance[h.ID]).ToList();
    }

    public void Clear()
    {
        foreach (var highlighted in _highlightedInstances)
        {
            Multimesh.SetInstanceColor(highlighted, Colors.Transparent);
        }
        _highlightedInstances = new List<int>();
    }
}
