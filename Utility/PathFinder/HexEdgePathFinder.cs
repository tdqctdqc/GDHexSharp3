using Godot;
using System;
using System.Collections.Generic;
using HexWargame;
using System.Linq;
using Priority_Queue;

public class HexEdgePathFinder 
{
    public List<int> FindPath(Func<int, int, float> edgeToEdgeCost, int start, int end)
    {
        var open = new SimplePriorityQueue<int, float>();//List<int>();
        var closed = new List<int>();
        var heuristicCosts = new Dictionary<int, float>();
        var costsFromStart = new Dictionary<int, float>();
        var nodes = new Dictionary<int, EdgePathFinderNode>();
        var endPos = end.GetPosForPairID();
        //add start node to open
        var startNode = new EdgePathFinderNode(start);
        open.Enqueue(start, 0f);
        costsFromStart.Add(start, 0f);
        heuristicCosts.Add(start, startNode.Position.DistanceTo(endPos));

        nodes.Add(start, new EdgePathFinderNode(start));

        while(open.Count > 0)
        {
            int current = open.Dequeue();
            if(current == end)
            {
                return BuildPathBackwards(nodes[current]);
            }

            //open.Remove(current);
            //open.OrderBy(h => costsFromStart[h] + heuristicCosts[h]);
            closed.Add(current);

            var neighbors = current.GetEdgeNeighbors();

            foreach (var n in neighbors)
            {
                if(closed.Contains(n)) continue; 

                if(open.Contains(n) == false)
                {
                    var nNode = new EdgePathFinderNode(n);
                    var costFromStart = edgeToEdgeCost(current, n) + costsFromStart[current];

                    if(float.IsInfinity(costFromStart) == false)
                    {
                        nodes.Add(n, nNode);
                        nNode.Parent = nodes[current];
                        var hCost = nNode.Position.DistanceTo(endPos);
                        heuristicCosts.Add(n, hCost);
                        costsFromStart.Add(n, costFromStart);
                        open.Enqueue(n, costFromStart + hCost); 
                    }
                }
                else
                {
                    float newCost = costsFromStart[current] + edgeToEdgeCost(current, n);
                    float oldCost = costsFromStart[n];
                    if(newCost < oldCost)
                    {
                        nodes[n].Parent = nodes[current];
                        costsFromStart[n] = newCost;
                        open.UpdatePriority(n, newCost + heuristicCosts[n]);
                    }
                }
            }
        }
        return null; 
    }

    private List<int> BuildPathBackwards(EdgePathFinderNode endNode)
    {
        var path = new List<int>();
        var current = endNode;
        while(current.Parent != null)
        {
            path.Add(current.HexPairID);
            current = current.Parent;
        }
        path.Add(current.HexPairID);
        path.Reverse();
        return path;
    }
}
