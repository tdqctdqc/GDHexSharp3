using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HexWargame;

public class Graph<T> 
{
    public List<GraphNode<T>> Nodes { get; private set; }
    public List<T> Elements { get; private set; }
    private Dictionary<T, GraphNode<T>> _nodesDic;

    public Graph(List<T> elements, Func<T, List<T>> neighborFunc, Func<T,T,float> edgeCostFunc)
    {
        Nodes = new List<GraphNode<T>>();
        Elements = new List<T>();
        _nodesDic = new Dictionary<T, GraphNode<T>>();
        for (int i = 0; i < elements.Count; i++)
        {
            var element = elements[i];
            Elements.Add(element);
            var neighbors = neighborFunc(element);
            var edgeCosts = neighbors.Select(n => edgeCostFunc(element, n)).ToList();
            var node = new GraphNode<T>(element, neighbors, edgeCosts);
            _nodesDic.Add(element, node);
            Nodes.Add(node);
        }
    }
    public List<T> FindPath(T start, T end)
    {
        var result = new List<T>();

        return result; 
    }
    public GraphNode<T> GetNode(T element)
    {
        if(_nodesDic.ContainsKey(element)) return _nodesDic[element];
        return null;
    }
    public Dictionary<T,List<T>> DoUnionFind()
    {
        var unsorted = Nodes.ToList();
        var sort = new Dictionary<T, List<T>>();
        while(unsorted.Count > 0)
        {
            var seedNode = unsorted[0];
            var elements = new List<GraphNode<T>>(){seedNode};
            var open = new List<GraphNode<T>>(){seedNode};
            while(open.Count > 0)
            {
                var current = open[0];
                var ns = current.Neighbors;
                foreach (var n in ns)
                {
                    var nNode = GetNode(n);
                    if(elements.Contains(nNode) == false)
                    {
                        elements.Add(nNode);
                        open.Add(nNode);
                    }
                }
                open.Remove(current);
            }
            unsorted = unsorted.Except(elements).ToList();
            sort.Add(seedNode.Element, elements.Select(e => e.Element).ToList());
        }

        return sort; 
    }

    public static Dictionary<T,List<T>> DoUnionFindGen(List<T> elements, Func<T, List<T>> neighborFunc)
    {
        var unsorted = elements.ToList();
        var sort = new Dictionary<T, List<T>>();
        while(unsorted.Count > 0)
        {
            var seedNode = unsorted[0];
            unsorted.Remove(seedNode);
            var partitionElements = new List<T>(){seedNode};
            var open = new List<T>(){seedNode};
            while(open.Count > 0)
            {
                var current = open[0];

                var ns = neighborFunc(current);
                foreach (var n in ns)
                {
                    //var nNode = GetNode(n);
                    if(partitionElements.Contains(n) == false)
                    {
                        partitionElements.Add(n);
                        unsorted.Remove(n);
                        open.Add(n);
                    }
                }
                open.Remove(current);
            }
            //unsorted = unsorted.Except(partitionElements).ToList();
            sort.Add(seedNode, partitionElements);
        }

        return sort; 
    }


}
