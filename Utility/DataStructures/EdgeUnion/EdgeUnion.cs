using Godot;
using System;
using System.Collections.Generic;
using HexWargame;
using System.Linq;

public class EdgeUnion
{
    private Dictionary<int, int> _keyDic;
    private Dictionary<int, List<int>> _unions; 

    public EdgeUnion()
    {
        _keyDic = new Dictionary<int, int>();
        _unions = new Dictionary<int, List<int>>();
    }

    public void AddEdge(int edge)
    {
        if(_keyDic.ContainsKey(edge))
        {
            throw new Exception("already have this edge");
        }

        var neighbors = edge.GetEdgeNeighbors().Where(n => _keyDic.ContainsKey(n)).ToList();
        if(neighbors.Count == 0)
        {
            _keyDic.Add(edge, edge);
            _unions.Add(edge, new List<int>(){edge});
        }
        else if(neighbors.Count == 1)
        {
            int prime = neighbors[0];
            var primeUnion = _keyDic[prime];
            var primeElements = _unions[primeUnion];

            _keyDic.Add(edge, primeUnion);
            primeElements.Add(edge);
        }
        else
        {
            int prime = neighbors[0];
            var primeUnion = _keyDic[prime];
            var primeElements = _unions[primeUnion];

            _keyDic.Add(edge, primeUnion);
            primeElements.Add(edge);

            for (int i = 1; i < neighbors.Count; i++)
            {
                int n = neighbors[i];
                var nUnion = _keyDic[n];
                if(nUnion == primeUnion) continue; 
                var nElements = _unions[nUnion];

                foreach (var nElement in nElements)
                {
                    _keyDic[nElement] = primeUnion;
                }
                _unions.Remove(nUnion);
                primeElements.AddRange(nElements);
            }
        }
    }
    public Dictionary<int, List<int>> GetUnions()
    {
        return _unions; 
    }
}
