using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class PartitionCell<T>
{
    public Graph<T> Graph { get; private set; }
    public T Seed { get; private set; }
    public GraphNode<T> SeedNode { get; private set; }
    public List<T> Elements { get; private set; }
    public List<GraphNode<T>> Nodes { get; private set; }
    public bool Open { get; private set; }
    public PartitionCell(Graph<T> graph, GraphNode<T> seedNode)
    {
        Graph = graph;
        Seed = seedNode.Element;
        SeedNode = seedNode;
        Elements = new List<T>(){Seed};
        Nodes = new List<GraphNode<T>>(){seedNode};
        Open = true; 
    }

    public void FloodFillStep(List<GraphNode<T>> unclaimedNodes,
                                List<GraphNode<T>> activeSeedNodes,
                                Action<T,T> aggAction
                                )
    {
        var openNodes = new HashSet<GraphNode<T>>();

            foreach (var memberNode in Nodes)
            {
                foreach (var n in memberNode.Neighbors)
                {
                    var nNode = Graph.GetNode(n);
                    if(Nodes.Contains(nNode)) continue;
                    if(unclaimedNodes.Contains(nNode) == false) continue; 
                    if(openNodes.Contains(nNode)) continue; 
                    openNodes.Add(nNode);
                }
            }

            if(openNodes.Count == 0)
            {
                activeSeedNodes.Remove(SeedNode);
                Open = false; 
                return;
            }

            var newNode = openNodes.ElementAt(0);//.GetRandomElement();
            unclaimedNodes.Remove(newNode);
            Nodes.Add(newNode);
            Elements.Add(newNode.Element);
            if(aggAction != null)
            {
                aggAction(Seed, newNode.Element);
            }
    }
}
