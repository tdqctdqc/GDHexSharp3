using Godot;
using System;
using System.Collections.Generic;

public class MaximumFlow 
{
 
    /* Returns true if there is a path
    from source 's' to sink 't' in residual
    graph. Also fills parent[] to store the
    path */
    private bool BreadthFirstSearch(int numVertices, int[,] rGraph, int s, int t, int[] parent)
    {
        // Create a visited array and mark
        // all vertices as not visited
        bool[] visited = new bool[numVertices];
        for (int i = 0; i < numVertices; ++i)
            visited[i] = false;
 
        // Create a queue, enqueue source vertex and mark
        // source vertex as visited
        List<int> queue = new List<int>();
        queue.Add(s);
        visited[s] = true;
        parent[s] = -1;
 
        // Standard BFS Loop
        while (queue.Count != 0) {
            int u = queue[0];
            queue.RemoveAt(0);
 
            for (int v = 0; v < numVertices; v++) {
                if (visited[v] == false
                    && rGraph[u, v] > 0) {
                    // If we find a connection to the sink
                    // node, then there is no point in BFS
                    // anymore We just have to set its parent
                    // and can return true
                    if (v == t) {
                        parent[v] = u;
                        return true;
                    }
                    queue.Add(v);
                    parent[v] = u;
                    visited[v] = true;
                }
            }
        }
 
        // We didn't reach sink in BFS starting from source,
        // so return false
        return false;
    }
 
    // Returns tne maximum flow
    // from s to t in the given graph
    public int FordFulkerson(int numVertices, int[,] graph, int s, int t)
    {
        int u, v;
 
        // Create a residual graph and fill
        // the residual graph with given
        // capacities in the original graph as
        // residual capacities in residual graph
 
        // Residual graph where rGraph[i,j]
        // indicates residual capacity of
        // edge from i to j (if there is an
        // edge. If rGraph[i,j] is 0, then
        // there is not)
        int[, ] rGraph = new int[numVertices, numVertices];
 
        for (u = 0; u < numVertices; u++)
            for (v = 0; v < numVertices; v++)
                rGraph[u, v] = graph[u, v];
 
        // This array is filled by BFS and to store path
        int[] parent = new int[numVertices];
 
        int max_flow = 0; // There is no flow initially
 
        // Augment the flow while there is path from source
        // to sink
        while (BreadthFirstSearch(numVertices, rGraph, s, t, parent)) {
            // Find minimum residual capacity of the edhes
            // along the path filled by BFS. Or we can say
            // find the maximum flow through the path found.
            int path_flow = int.MaxValue;
            for (v = t; v != s; v = parent[v]) {
                u = parent[v];
                path_flow
                    = Math.Min(path_flow, rGraph[u, v]);
            }
 
            // update residual capacities of the edges and
            // reverse edges along the path
            for (v = t; v != s; v = parent[v]) {
                u = parent[v];
                rGraph[u, v] -= path_flow;
                rGraph[v, u] += path_flow;
            }
 
            // Add path flow to overall flow
            max_flow += path_flow;
        }
 
        // Return the overall flow
        return max_flow;
    }
}
