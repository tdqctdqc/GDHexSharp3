using Godot;
using System;

public class MinCostFlow 
{
    // Stores the found edges
    bool[] found;
 
    // Stores the number of nodes
    int N;
 
    // Stores the capacity
    // of each edge
    int[][] cap;
    int[][] flow;
 
    // Stores the cost per
    // unit flow of each edge
    int[][] cost;
 
    // Stores the distance from each node
    // and picked edges for each node
    int[] dad, dist, pi;
 
    static int INF = int.MaxValue / 2 - 1;
 
    // Function to check if it is possible to
    // have a flow from the src to sink
    bool Search(int src, int sink)
    {
 
        // Initialise found[] to false
        for (int i = 0; i < found.Length; i++)
        {
            found[i] = false;
        }
 
        // Initialise the dist[] to INF
        for (int i = 0; i < dist.Length; i++)
        {
            dist[i] = INF;
        }
 
        // Distance from the source node
        dist[src] = 0;
 
        // Iterate untill src reaches N
        while (src != N) {
 
            int best = N;
            found[src] = true;
 
            for (int k = 0; k < N; k++) {
 
                // If already found
                if (found[k])
                    continue;
 
                // Evaluate while flow
                // is still in supply
                if (flow[k][src] != 0) {
 
                    // Obtain the total value
                    int val
                        = dist[src] + pi[src]
                          - pi[k] - cost[k][src];
 
                    // If dist[k] is > minimum value
                    if (dist[k] > val) {
 
                        // Update
                        dist[k] = val;
                        dad[k] = src;
                    }
                }
 
                if (flow[src][k] < cap[src][k]) {
 
                    int val = dist[src] + pi[src]
                              - pi[k] + cost[src][k];
 
                    // If dist[k] is > minimum value
                    if (dist[k] > val) {
 
                        // Update
                        dist[k] = val;
                        dad[k] = src;
                    }
                }
 
                if (dist[k] < dist[best])
                    best = k;
            }
 
            // Update src to best for
            // next iteration
            src = best;
        }
 
        for (int k = 0; k < N; k++)
            pi[k]
                = Math.Min(pi[k] + dist[k], INF);
 
        // Return the value obtained at sink
        return found[sink];
    }
 
    // Function to obtain the maximum Flow
    int[] GetMaxFlow(int[][] cap, int[][] cost,
                     int src, int sink)
    {
 
        this.cap = cap;
        this.cost = cost;
 
        N = cap.Length;
        found = new bool[N];
        flow = new int[N][];
        for (int i = 0; i < N; i++)
        {
            flow[i] = new int[N];
        }
        dist = new int[N + 1];
        dad = new int[N];
        pi = new int[N];
 
        int totflow = 0, totcost = 0;
 
        // If a path exist from src to sink
        while (Search(src, sink)) {
 
            // Set the default amount
            int amt = INF;
            for (int x = sink; x != src; x = dad[x])
 
                amt = Math.Min(amt,
                               flow[x][dad[x]] != 0
                                   ? flow[x][dad[x]]
                                   : cap[dad[x]][x]
                                         - flow[dad[x]][x]);
 
            for (int x = sink; x != src; x = dad[x]) {
 
                if (flow[x][dad[x]] != 0) {
                    flow[x][dad[x]] -= amt;
                    totcost -= amt * cost[x][dad[x]];
                }
                else {
                    flow[dad[x]][x] += amt;
                    totcost += amt * cost[dad[x]][x];
                }
            }
            totflow += amt;
        }
 
        // Return pair total cost and sink
        return new int[] { totflow, totcost };
    }
}
