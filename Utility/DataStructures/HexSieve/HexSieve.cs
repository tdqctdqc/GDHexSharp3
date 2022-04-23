using Godot;
using HexWargame;
using System;
using System.Linq;
using System.Collections.Generic;

public class HexSieve 
{   
    public static Dictionary<int, List<int>> GetPartitionsByFaction(List<HexModel> hexes)
    {
        var partitionDic = new Dictionary<int, List<int>>();
        int partitionCount = 0;
        foreach (var hex in hexes)
        {
            var containing = partitionDic.Where(l => l.Value.Contains(hex.ID));
            if(containing.Count() > 0)
            {
                continue; 
            }
            else
            {
                var neighborhood = FindFactionNeighborhood(hex);
                partitionDic.Add(partitionCount, neighborhood.Select(h => h.ID).ToList());
                partitionCount++;
            }
        }
        return partitionDic; 
    }
    public static Dictionary<int, List<int>> GetPartitionsByContinent(List<HexModel> hexes)
    {

        var partitionDic = new Dictionary<int, List<int>>();
        int partitionCount = 0;
        foreach (var hex in hexes)
        {
            var containing = partitionDic.Where(l => l.Value.Contains(hex.ID));
            if(containing.Count() > 0)
            {
                continue; 
            }
            else
            {
                var neighborhood = FindContinentNeighborhood(hex);
                partitionDic.Add(partitionCount, neighborhood.Select(h => h.ID).ToList());
                partitionCount++;
            }
        }
        return partitionDic; 
    }

    public static bool CheckIfConnectedByFaction(HexModel start, HexModel dest)
    {
        Func<HexModel, HexModel, bool> facConnect = (h,g) => (h.FactionID == g.FactionID);
        return CheckIfConnected(start, dest, facConnect);
    }
    public static HashSet<HexModel> FindFactionNeighborhood(HexModel hex)
    {
        Func<HexModel, HexModel, bool> facConnect = (h,g) => (h.FactionID == g.FactionID);
        return FindNeighborhood(hex, facConnect);
    }
    public static HashSet<HexModel> FindContinentNeighborhood(HexModel hex)
    {
        Func<HexModel, HexModel, bool> contConnect = (h,g) => (h.Terrain.IsWater == g.Terrain.IsWater);
        return FindNeighborhood(hex, contConnect);
    }
    public static bool CheckIfConnected(HexModel start, HexModel dest, Func<HexModel, HexModel, bool> connectedFunc)
    {
        var list = new List<HexModel>();

        var open = new List<HexModel>();
        var closed = new List<HexModel>();

        open.Add(start);
        list.Add(start);

        while(open.Count > 0)
        {
            var openHex = open[0];
            foreach (var n in openHex.GetNeighbors())
            {
                if(n.ID == dest.ID) return true; 
                if(closed.Contains(n) == false && open.Contains(n) == false )
                {
                    if(connectedFunc(openHex, n) == true)
                    {
                        open.Add(n);
                        list.Add(n);
                    }
                }
            }
            open.Remove(openHex);
            closed.Add(openHex);
        }
        return false; 
    }
    public static HashSet<HexModel> FindNeighborhood(HexModel hex, Func<HexModel, HexModel, bool> connectedFunc)
    {
        var list = new HashSet<HexModel>();

        var open = new List<HexModel>();
        var closed = new List<HexModel>();

        open.Add(hex);
        list.Add(hex);

        while(open.Count > 0)
        {
            var openHex = open[0];
            foreach (var n in openHex.GetNeighbors())
            {
                if(closed.Contains(n) == false && open.Contains(n) == false )
                {
                    if(connectedFunc(openHex, n) == true)
                    {
                        open.Add(n);
                        list.Add(n);
                    }
                }
            }
            open.Remove(openHex);
            closed.Add(openHex);
        }
        return list; 
    }
}
