using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexWargame 
{
public static class GraphExt 
{
    public static List<T> GetInnerBorder<T, R>(this R agg) where R : IGraphNodeAggregation<R, T> where T : IGraphNode<T>
    {
        return GetInnerBorder<T>(agg.Children);
    }
    public static List<T> GetInnerBorder<T>(this List<T> nodes) where T : IGraphNode<T>
    {
        var inner = new List<T>();
        foreach (var node in nodes)
        {   
            var ns = node.Neighbors;
            if(ns.Where(n => nodes.Contains(n) == false).Count() > 0)
            {
                inner.Add(node);
            }
        }
        return inner; 
    }
    public static List<T> GetOuterBorder<T, R>(this R agg, List<T> inner = null) where R : IGraphNodeAggregation<R, T> where T : IGraphNode<T>
    {
        return GetOuterBorder<T>(agg.Children, inner);
    }
    public static List<T> GetOuterBorder<T>(this List<T> nodes, List<T> inner = null) where T : IGraphNode<T>
    {
        if(inner == null) inner = GetInnerBorder(nodes);
        var outer = new List<T>();
        foreach (var i in inner)
        {   
            var ns = i.Neighbors;
            foreach (var n in ns)
            {
                if(nodes.Contains(n) == false) outer.Add(n);
            }
        }
        return outer; 
    }
    public static List<List<T>> GetBorderLayers<T, R>(this R agg, int iterations) where T : IGraphNode<T> where R : IGraphNodeAggregation<R, T> 
    {
        return GetBorderLayers<T>(agg.Children, iterations);
    }
    public static List<List<T>> GetBorderLayers<T>(this List<T> nodes, int iterations) where T : IGraphNode<T>
    {
        var layers = new List<List<T>>();
        var covered = new List<T>();
        var inner = nodes.GetInnerBorder();
        layers.Add(inner);
        covered.AddRange(nodes);
        var lastInner = inner; 
        for (int i = 0; i < iterations; i++)
        {   
            var outer = covered.GetOuterBorder(lastInner);
            layers.Add(outer);
            covered.AddRange(outer);
            lastInner = outer; 
        }


        return layers; 
    }

    public static void DoNeighborActionConditionalUndirected<T>(this List<T> aggs, Func<T,T, bool> neighborAction) where T : IGraphNode<T> 
    {
        var check = new int[aggs.Count, aggs.Count];
        for (int i = 0; i < aggs.Count; i++)
        {
            var agg = aggs[i];
            foreach (var nAgg in agg.Neighbors)
            {
                int nIndex = aggs.IndexOf(nAgg);
                if(check[i, nIndex] == 1 || check[nIndex, i] == 1)
                {
                    continue;
                }
                var success = neighborAction(agg, nAgg);
                if(success)
                {
                    check[i, nIndex] = 1;
                    check[nIndex, i] = 1;
                }
            }
        }
    }
}
}
