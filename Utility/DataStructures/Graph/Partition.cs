using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using KdTree;
using KdTree.Math;
using HexWargame;
using Priority_Queue;

public class Partition<T>
{
    public Graph<T> Graph { get; private set; }
    public Partition(Graph<T> graph)
    {
        Graph = graph;
    }
    public Dictionary<T,List<T>> DoNearestFill(List<T> seeds, Action<T,T> aggAction, Func<T,Vector2> posFunc)
    {
        var partitions = new Dictionary<T, List<T>>();
        foreach (var seed in seeds)
        {
            partitions.Add(seed, new List<T>());
        }
        var kd = new KdTree<float, T>(2, new FloatMath());
        foreach (var s in seeds)
        {
            kd.Add(posFunc(s).ToArray(), s);
        }
        foreach (var e in Graph.Elements)
        {
            var closeSeed = kd.GetNearestNeighbours(posFunc(e).ToArray(), 1)[0].Value;
            partitions[closeSeed].Add(e);
        }
        return partitions; 
    }
    public Dictionary<T,List<T>> DoFloodfill(List<T> seeds, Action<T,T> aggAction)
    {
        var partitions = new Dictionary<T, PartitionCell<T>>();
        var seedNodes = seeds.Select(s => Graph.GetNode(s)).ToList();
        
        foreach (var seed in seeds)
        {
            var seedNode = Graph.GetNode(seed);
            var p = new PartitionCell<T>(Graph, seedNode);
            partitions.Add(seed, p);
        }
        var unclaimedNodes = Graph.Nodes.Except(seedNodes).ToList();
        var activeSeedNodes = seedNodes.ToList();
        int iter = 0;
        while(activeSeedNodes.Count > 0)
        {
            var seedNode = seedNodes[iter % seedNodes.Count];
            iter++;
            if(activeSeedNodes.Contains(seedNode) == false) continue; 
            var partition = partitions[seedNode.Element];
            partition.FloodFillStep(unclaimedNodes, activeSeedNodes, aggAction);
        }

        var elements = new Dictionary<T, List<T>>();
        foreach (var entry in partitions)
        {
            elements.Add(entry.Key, entry.Value.Elements);
        }
        return elements; 
    }
    public Dictionary<T,List<T>> DoFloodfillRatio(List<List<T>> seedLists, List<float> weights, Action<T,T> aggAction)
    {
        var partitions = new Dictionary<T, PartitionCell<T>>();

        float totalWeight = weights.Sum();
        List<float> categoryRatios = weights.Select(w => w / totalWeight).ToList();
        var categories = new SimplePriorityQueue<PartitionCategory<T>>();//List<PartitionCategory<T>>();
        for (int i = 0; i < seedLists.Count; i++)
        {
            var cat = new PartitionCategory<T>(Graph, seedLists[i], categoryRatios[i]);
            categories.Enqueue(cat, categoryRatios[i]);
        }

        var seeds = seedLists.SelectMany(s => s).ToList();
        var seedNodes = seeds.Select(s => Graph.GetNode(s)).ToList();
        var unclaimedNodes = Graph.Nodes.Except(seedNodes).ToList();

        while(unclaimedNodes.Count > 0)
        {
            var cat = categories.First; 
            cat.DoFloodfillStep(unclaimedNodes, aggAction);
            categories.UpdatePriority(cat, cat.PercentFulfilledRatio);
        }

        var elements = new Dictionary<T, List<T>>();
        foreach (var cat in categories)
        {
            foreach (var cell in cat.PartitionCells)
            {
                elements.Add(cell.Seed, cell.Elements);
            }
        }

        return elements; 
    }




    public void DoFloodfillWithBlankAction(List<T> seeds, Action<T, T> action, Action<T> blankAction)
    {
        var partitions = DoFloodfill(seeds, action);
        var handledElements = new List<T>();
        foreach (var item in partitions)
        {
            var list = item.Value;
            handledElements.AddRange(list);
            foreach (var t in list)
            {
                action(item.Key, t);
            }
        }
        var unhandledElements = Graph.Elements.Except(handledElements);
        foreach (var e in unhandledElements)
        {
            blankAction(e);
        }
    }
}
