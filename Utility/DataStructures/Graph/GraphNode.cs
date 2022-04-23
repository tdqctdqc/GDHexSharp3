using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class GraphNode<T> 
{
    public T Element { get; private set; }
    public List<T> Neighbors { get; private set; }
    public Dictionary<T, float> EdgeCosts { get; private set; }

    public GraphNode(T element, List<T> neighbors, List<float> edgeCosts)
    {
        Element = element;
        Neighbors = neighbors.ToList();
        EdgeCosts = new Dictionary<T, float>();
        for (int i = 0; i < Neighbors.Count; i++)
        {
            EdgeCosts.Add(neighbors[i], edgeCosts[i]);
        }
    }
}
