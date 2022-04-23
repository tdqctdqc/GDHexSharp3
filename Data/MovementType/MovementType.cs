using Godot;
using System;
using System.Collections.Generic;

public class MovementType 
{
    public Dictionary<Terrain, float> TerrainCosts { get; private set; } 
    public Dictionary<RoadType, float> RoadCosts { get; private set; } 
    public Dictionary<RiverType, float> RiverCosts { get; private set; } 
    public MovementType(MovementTypeModel model)
    {
        BuildTerrainCosts(model);
        BuildRoadCosts(model);
        BuildRiverCosts(model);
    }

    private void BuildTerrainCosts(MovementTypeModel model)
    {
        TerrainCosts = new Dictionary<Terrain, float>();

        var costsDic = model.TerrainCosts;
        var terrains = Game.I.Session.Data.Terrain.Terrains;
        foreach (var item in terrains)
        {
            Terrain terrain = item.Value; 
            if(costsDic.Contains(terrain.Name) == false)
            {
                throw new ArgumentException($"{model.Name} model does not include terrain cost for {terrain.Name}");
            }
            float cost = (float)costsDic[terrain.Name];
            if(cost == -1.0f) cost = Mathf.Inf;
            TerrainCosts.Add(terrain, cost);
        }
    }
    private void BuildRoadCosts(MovementTypeModel model)
    {
        RoadCosts = new Dictionary<RoadType, float>();

        var costsDic = model.RoadCosts;
        var roads = Game.I.Session.Data.RoadTypes.RoadTypes;
        foreach (var item in roads)
        {
            RoadType road = item.Value; 
            if(costsDic.Contains(road.Name) == false)
            {
                throw new ArgumentException($"{model.Name} model does not include road cost for {road.Name}");
            }
            float cost = (float)costsDic[road.Name];
            if(cost == -1.0f) cost = Mathf.Inf;
            RoadCosts.Add(road, cost);
        }
    }
    private void BuildRiverCosts(MovementTypeModel model)
    {
        RiverCosts = new Dictionary<RiverType, float>();

        var costsDic = model.RiverCosts;
        var rivers = Game.I.Session.Data.RiverTypes.RiverTypes;
        foreach (var item in rivers)
        {
            RiverType river = item.Value; 
            if(costsDic.Contains(river.Name) == false)
            {
                throw new ArgumentException($"{model.Name} model does not include river cost for {river.Name}");
            }
            float cost = (float)costsDic[river.Name];
            if(cost == -1.0f) cost = Mathf.Inf;
            RiverCosts.Add(river, cost);
        }
    }
}
