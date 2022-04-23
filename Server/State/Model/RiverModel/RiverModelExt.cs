using Godot;
using System;
using System.Collections.Generic;
namespace HexWargame
{
public static class RiverModelExt
{
    public static RiverModel GetRiverToHex(this HexModel h1, HexModel h2)
    {
        if(h1.GetHexDistance(h2) != 1) return null;

        int id = h1.GetHexPairIndex(h2);
        return CacheManager.Rivers[id];
    }
    public static bool BetweenSameHexes(this RiverModel r1, RiverModel r2)
    {
        if(r1.Hex1 == r2.Hex1 && r1.Hex2 == r2.Hex2) return true;
        return false; 
    }
}
}
