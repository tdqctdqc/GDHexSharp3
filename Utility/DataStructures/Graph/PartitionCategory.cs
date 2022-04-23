using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class PartitionCategory <T>
{
    public List<PartitionCell<T>> PartitionCells { get; private set; }
    private List<GraphNode<T>> _activeSeeds;
    public Graph<T> Graph { get; private set; }
    public float FulfilledRatio => (float)PartitionCells.Sum(p => p.Elements.Count) / (float)Graph.Elements.Count;
    public float DesiredRatio { get; private set; }
    public float PercentFulfilledRatio => FulfilledRatio / DesiredRatio;
    private int _index; 
    public PartitionCategory(Graph<T> graph, List<T> seeds, float desiredRatio)
    {
        DesiredRatio = desiredRatio;
        _index = 0;
        Graph = graph;
        _activeSeeds = new List<GraphNode<T>>();
        PartitionCells = new List<PartitionCell<T>>();
        foreach (var seed in seeds)
        {
            var seedNode = Graph.GetNode(seed);
            _activeSeeds.Add(seedNode);
            var p = new PartitionCell<T>(Graph, seedNode);
            PartitionCells.Add(p);
        }
    }

    public void DoFloodfillStep(List<GraphNode<T>> unclaimed, Action<T,T> aggAction)
    {
        if(PartitionCells.Count == 0) return; 
        var cell = PartitionCells[_index % PartitionCells.Count];
        _index++;
        while(_activeSeeds.Contains(cell.SeedNode) == false)
        {
            cell = PartitionCells[_index % PartitionCells.Count];
            _index++;
        }
        cell.FloodFillStep(unclaimed, _activeSeeds, aggAction);
    }
}
