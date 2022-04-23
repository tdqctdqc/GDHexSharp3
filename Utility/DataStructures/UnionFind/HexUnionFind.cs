using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class HexUnionFind
{
    public static List<List<HexModel>> SortHexes(List<HexModel> hexes, Func<HexModel, HexModel, bool> valueFunc)
    {
        var unsorted = new List<HexModel>(hexes);
        var sortedLists = new List<List<HexModel>>();
        while(unsorted.Count > 0)
        {
            var start = unsorted[0];
            unsorted.Remove(start);
            var open = new List<HexModel>(){start};
            var closed = new List<HexModel>();
            var currentNeighborhood = new List<HexModel>(){start};
            sortedLists.Add(currentNeighborhood);

            while(open.Count > 0)
            {
                var current = open[0];
                open.Remove(current);
                var neighbors = current.GetNeighbors();
                foreach (var n in neighbors)
                {
                    if(hexes.Contains(n) == false)
                    {
                        GD.Print("neighbor not in hexes");
                        return null;
                    }
                    if(closed.Contains(n)) continue; 
                    if(valueFunc(current, n) == false)
                    {
                        closed.Add(n);
                    }
                    if(currentNeighborhood.Contains(n) || open.Contains(n)) continue; 
                    if(valueFunc(current, n))
                    {
                        open.Add(n);
                        unsorted.Remove(n);
                        currentNeighborhood.Add(n);
                    }
                }
            }

        }
        return sortedLists; 
    }
}
