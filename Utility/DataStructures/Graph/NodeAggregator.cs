using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class NodeAggregator<T> : IGraphNodeAggregation<NodeAggregator<T>, T> 
{
    public List<T> Children { get; private set; }
    public List<NodeAggregator<T>> Neighbors { get; private set; }
    public Dictionary<NodeAggregator<T>, float> EdgeCosts { get; private set; }
    private Func<T, List<T>> _elementNeighborFunc;
    private Func<T,T,float> _elementEdgeCostFunc;
    public Vector2 WorldPos {get; set;}
    public NodeAggregator(List<T> elements, 
                            Func<T, List<T>> elementNeighborFunc, 
                            Func<T,T,float> elementEdgeCostFunc)
    {
        _elementNeighborFunc = elementNeighborFunc;
        _elementEdgeCostFunc = elementEdgeCostFunc;
        Children = elements.ToList();
    }
    public void SetNeighbors(List<NodeAggregator<T>> potentialNeighbors)
    {
        Neighbors = new List<NodeAggregator<T>>();
        EdgeCosts = new Dictionary<NodeAggregator<T>, float>();
        foreach (var neighbor in potentialNeighbors)
        {
            if(neighbor == this) continue; 
            var neighborElements = Children.SelectMany(n => _elementNeighborFunc(n))
                                            .Where(n => neighbor.Children.Contains(n));
            if(neighborElements.Count() > 0)
            {
                Neighbors.Add(neighbor);
            }
        }
    }
}
