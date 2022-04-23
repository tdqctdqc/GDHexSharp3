using Godot;
using System;
using System.Collections.Generic;
using Priority_Queue;

public class GenericPathFinder 
{
    public static List<T> FindPath<T>(Func<T, T, float> elementEdgeCost, 
                                        Func<T,List<T>> neighborFunc,
                                        Func<T,T,float> heuristicFunc,
                                        T start, 
                                        T end) 
    {
        var open = new SimplePriorityQueue<T, float>();//List<T>();
        var closed = new HashSet<T>();
        var nodeDic = new Dictionary<T, GenericPathFinderNode<T>>();
        var startNode = new GenericPathFinderNode<T>(start, default(T), 0f, heuristicFunc(start, end));
        open.Enqueue(start, 0f + heuristicFunc(start, end));
        nodeDic.Add(start, startNode);

        while(open.Count > 0)
        {
            T current = open.Dequeue();
            var currentNode = nodeDic[current];
            closed.Add(current);
            if(current.Equals(end))
            {
                var path = BuildPathBackwards(currentNode, nodeDic);
                return path;
            }

            var neighbors = neighborFunc(current);

            foreach (var n in neighbors)
            {
                if(closed.Contains(n)) continue; 
                var edgeCost = elementEdgeCost(current, n);
                if(float.IsInfinity(edgeCost)) continue; 
                if(open.Contains(n) == false)
                {
                    var costFromStart = edgeCost + currentNode.CostFromStart;

                    if(float.IsInfinity(costFromStart) == false)
                    {
                        var nNode = new GenericPathFinderNode<T>(n, current, costFromStart, heuristicFunc(n,end));
                        nodeDic.Add(n, nNode);
                        open.Enqueue(n, costFromStart + heuristicFunc(n,end)); 
                    }
                }
                else
                {
                    var node = nodeDic[n];
                    float newCost = currentNode.CostFromStart + edgeCost;
                    float oldCost = node.CostFromStart;
                    if(newCost < oldCost)
                    {
                        node.Parent = current;
                        node.CostFromStart = newCost;
                        open.UpdatePriority(n, newCost + heuristicFunc(n, end));
                    }
                }
            }
        }
        return null; 
    }

    private static List<T> BuildPathBackwards<T>(GenericPathFinderNode<T> endNode, Dictionary<T, GenericPathFinderNode<T>> nodeDic)
    {
        var path = new List<T>();
        var current = endNode;
        while(current.Parent != null)
        {
            path.Add(current.Element);
            current = nodeDic[current.Parent];
        }
        path.Add(current.Element);
        path.Reverse();
        return path;
    }
    public static float GetPathCost<T>(List<T> path, Func<T,T,float> edgeCost)
    {
        if(path.Count < 2) return 0f;
        float cost = 0f;
        var from = path[0];
        var to = path[1];
        for (int i = 0; i < path.Count - 1; i++)
        {
            cost += edgeCost(from,to);
        }
        return cost;
    }
}
public class GenericPathFinderNode<T> 
{
    public T Element { get; set; }
    public T Parent { get; set; }
    public float HeuristicCost { get; set; }
    public float CostFromStart { get; set; }
    public GenericPathFinderNode(T t, T parent, float fromStart, float heuristic)
    {
        CostFromStart = fromStart;
        HeuristicCost = heuristic;
        Parent = parent; 
        Element = t; 
    }
}
