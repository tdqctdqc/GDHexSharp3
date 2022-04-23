using Godot;
using System;
namespace  HexWargame
{
public static class RoadTypeExt
{
    public static float GetBuildCost(this RoadType roadType, HexModel from, HexModel to)
    {
        if(from.Terrain.BuildCostMultiplier < 0f || to.Terrain.BuildCostMultiplier < 0f) return Mathf.Inf;
        float costMult = .5f * (from.Terrain.BuildCostMultiplier) + .5f * (to.Terrain.BuildCostMultiplier);
        float riverCost = 0f;
        var river = from.GetRiverToHex(to);
        
        if(river != null) riverCost = river.RiverType.BuildCost;
        return costMult * roadType.BaseBuildCost + riverCost;
    }
} 
}

