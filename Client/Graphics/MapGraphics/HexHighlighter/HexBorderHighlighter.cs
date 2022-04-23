using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class HexBorderHighlighter : MultiMeshInstance2D
{
    private Dictionary<int, int> _pairIDToInstance;
    private List<int> _highlightedInstances; 
    public void Setup(List<HexModel> hexes)
    {
        _highlightedInstances = new List<int>();
        _pairIDToInstance = new Dictionary<int, int>();
        Multimesh = new MultiMesh();
        var borderMesh = new QuadMesh();
        borderMesh.Size = new Vector2(Constants.HexRadius, 40f);
        Multimesh.Mesh = borderMesh;
        Multimesh.ColorFormat = MultiMesh.ColorFormatEnum.Float;
        Multimesh.InstanceCount = 0;

        Multimesh.InstanceCount = hexes.Count * 6;
        int hexIDiter = 1;
        List<int> pairIDs = new List<int>();
        int numPairsIter = 0;
        bool go = true; 
        while(go)
        {
            var hex = Cache<HexModel>.GetModel(hexIDiter);
            hexIDiter++;
            if(hex == null)
            {
                break;
            }
            var neighbors = hex.GetNeighbors();
            foreach (var n in neighbors)
            {
                int pairID = hex.GetHexPairIndex(n);
                if(_pairIDToInstance.ContainsKey(pairID) == false)
                {
                    _pairIDToInstance.Add(pairID, numPairsIter);
                    numPairsIter++;
                    pairIDs.Add(pairID);
                }
            }
        }
        Multimesh.InstanceCount = numPairsIter;
        for (int i = 0; i < numPairsIter; i++)
        {
            var pairID = pairIDs[i];
            var pairHexes = pairID.GetHexesFromPairIndex();
            var angle = pairHexes.Item1.GetHexAngle(pairHexes.Item2) + Mathf.Pi / 2f;
            var pos = pairID.GetPosForPairID();
            Multimesh.SetInstanceTransform2d(i, new Transform2D(angle, pos));
            Multimesh.SetInstanceColor(i, Colors.Transparent);
        }
    }
    public void ClearAndHighlightHexBorders(List<int> hexPairsToHighlight, Color color)
    {
        foreach (var highlighted in _highlightedInstances)
        {
            Multimesh.SetInstanceColor(highlighted, Colors.Transparent);
        }
        foreach (var toHighlight in hexPairsToHighlight)
        {
            int instance = _pairIDToInstance[toHighlight];
            Multimesh.SetInstanceColor(instance, color);
        }
        _highlightedInstances = hexPairsToHighlight.Select(h => _pairIDToInstance[h]).ToList();
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
