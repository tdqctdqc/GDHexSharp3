using Godot;
using System;
using System.Collections.Generic;
namespace HexWargame
{
public static class RoadModelExt
{
    public static RoadModel GetRoadToHex(this HexModel h1, HexModel h2)
    {
        if(h1.GetHexDistance(h2) != 1) return null;
        int id = h1.GetHexPairIndex(h2);
        return CacheManager.Roads[id];
    }
    public static List<RoadModel> GetHexRoads(this HexModel h)
    {
        var list = new List<RoadModel>();
        foreach (var n in h.Neighbors)
        {
            var r = n.GetRoadToHex(h);
            if(r != null) list.Add(r);
        }
        return list;
    }
}
}
