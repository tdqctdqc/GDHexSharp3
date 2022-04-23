using Godot;
using System;
using System.Collections.Generic;
using HexWargame;
using System.Threading.Tasks;
using System.Linq;

public class NodeAggregation<T>
{
    public Graph<T> Graph { get; private set; }
    private Func<T, List<T>> _neighborFunc;
    private Func<T,T,float> _elementEdgeCostFunc;
    public List<NodeAggregator<T>> Aggregators { get; private set; }
    public Dictionary<T, NodeAggregator<T>> Dic { get; private set; }
    public Dictionary<NodeAggregator<T>, T> BackDic { get; private set; }
    public NodeAggregation( List<T> elements,  
                            Func<T, List<T>> neighborFunc, 
                            Func<T,T,float> elementEdgeCostFunc)
    {
        Graph = new Graph<T>(elements, neighborFunc, elementEdgeCostFunc);
        _neighborFunc = neighborFunc;
        _elementEdgeCostFunc = elementEdgeCostFunc;
    }
    public Dictionary<T, List<T>> NearestFill(List<T> seeds, Func<T,Vector2> posFunc, Action<T, T> aggAction = null)
    {
        var partition = new Partition<T>(Graph);
        var partitions = partition.DoNearestFill(seeds, aggAction, posFunc);
        Dic = new Dictionary<T, NodeAggregator<T>>();
        BackDic = new Dictionary<NodeAggregator<T>, T>();
        Aggregators = new List<NodeAggregator<T>>();
        foreach (var p in partitions)
        {
            var agg = new NodeAggregator<T>(p.Value, _neighborFunc, _elementEdgeCostFunc);
            Aggregators.Add(agg);
            Dic.Add(p.Key, agg);
            BackDic.Add(agg, p.Key);
        }
        foreach (var agg in Aggregators)
        {
            agg.SetNeighbors(Aggregators);
        }
        return partitions;
    }

    public Dictionary<T, List<T>> Floodfill(List<T> seeds, Action<T, T> aggAction = null)
    {
        var partition = new Partition<T>(Graph);
        var partitions = partition.DoFloodfill(seeds, aggAction);
        Dic = new Dictionary<T, NodeAggregator<T>>();
        BackDic = new Dictionary<NodeAggregator<T>, T>();
        Aggregators = new List<NodeAggregator<T>>();
        foreach (var p in partitions)
        {
            var agg = new NodeAggregator<T>(p.Value, _neighborFunc, _elementEdgeCostFunc);
            Aggregators.Add(agg);
            Dic.Add(p.Key, agg);
            BackDic.Add(agg, p.Key);
        }
        foreach (var agg in Aggregators)
        {
            agg.SetNeighbors(Aggregators);
        }
        return partitions;
    }
    
    public Dictionary<T, List<T>> FloodfillRatio(List<List<T>> seedLists, List<float> weights, Action<T,T> aggAction = null)
    {
        var partition = new Partition<T>(Graph);

        var partitions = partition.DoFloodfillRatio(seedLists, weights, aggAction);

        Dic = new Dictionary<T, NodeAggregator<T>>();
        BackDic = new Dictionary<NodeAggregator<T>, T>();
        Aggregators = new List<NodeAggregator<T>>();
        foreach (var p in partitions)
        {
            var agg = new NodeAggregator<T>(p.Value, _neighborFunc, _elementEdgeCostFunc);
            Aggregators.Add(agg);
            Dic.Add(p.Key, agg);
            BackDic.Add(agg, p.Key);
        }
        foreach (var agg in Aggregators)
        {
            agg.SetNeighbors(Aggregators);
        }
        return partitions;
    }

  

    public void DoNeighborActionConditionalUndirected(Func<T,T, bool> neighborAction)
    {
        var check = new int[Aggregators.Count, Aggregators.Count];

        for (int i = 0; i < Aggregators.Count; i++)
        {
            var agg = Aggregators[i];
            foreach (var nAgg in agg.Neighbors)
            {
                int nIndex = Aggregators.IndexOf(nAgg);
                if(check[i, nIndex] == 1 || check[nIndex, i] == 1)
                {
                    continue;
                }
                var success = neighborAction(BackDic[agg], BackDic[nAgg]);
                if(success)
                {
                    check[i, nIndex] = 1;
                    check[nIndex, i] = 1;
                }
            }
        }
        // var range = Enumerable.Range(0, Aggregators.Count - 1);
        // Action<int> action = (i) =>
        // {
        //     var agg = Aggregators[i];
        //     foreach (var nAgg in agg.Neighbors)
        //     {
        //         int nIndex = Aggregators.IndexOf(nAgg);
        //         if(check[i, nIndex] == 1 || check[nIndex, i] == 1)
        //         {
        //             continue;
        //         }
        //         var success = neighborAction(BackDic[agg], BackDic[nAgg]);
        //         if(success)
        //         {
        //             check[i, nIndex] = 1;
        //             check[nIndex, i] = 1;
        //         }
        //     }
        // };
        // Parallel.ForEach(range, action);
    }
}
