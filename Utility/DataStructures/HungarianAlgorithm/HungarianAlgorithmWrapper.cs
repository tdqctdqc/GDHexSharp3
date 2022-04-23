using Godot;
using System;
using HungarianAlgorithm;
using System.Collections.Generic;

public class HungarianAlgorithmWrapper 
{
    public static Dictionary<T,V> GetAssignment<T,V>(List<T> agents, List<V> seats, Func<T,V,int> costFunc)
    {
        var result = new Dictionary<T,V>();
        var costs = new int[agents.Count, seats.Count];
        for (int i = 0; i < agents.Count; i++)
        {   
            for (int j = 0; j < seats.Count; j++)
            {
                var agent = agents[i];
                var seat = seats[j];
                costs[i,j] = costFunc(agent, seat);
            }
        }
        costs = SquareArray(costs);
        var assgn = HungarianAlgorithm.HungarianAlgorithm.FindAssignments(costs);
        for (int i = 0; i < seats.Count; i++)
        {
            T agent; 
            if(agents.Count > i) agent = agents[i];
            else continue;
            result.Add(agent, seats[i]);
        }
        return result; 
    }

    private static int[,] SquareArray(int[,] arr)
    {
        int dimX = arr.GetLength(0);
        int dimY = arr.GetLength(1);
        int dim = Math.Max(dimX, dimY);
        int[,] result = new int[dim,dim];
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if(i < dimX && j < dimY)
                {
                    result[i,j] = arr[i,j];
                }
                else result[i,j] = 0;
            }
        }
        return result; 
    }
}
