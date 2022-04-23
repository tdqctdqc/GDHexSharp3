using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class RoadGenerator 
{
    private static int iter = 0;
    private static StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    private static RoadType _cityConnectRoadType => Game.I.Session.Data.RoadTypes[2];
    private static RoadType _townConnectRoadType => Game.I.Session.Data.RoadTypes[1];
    public static List<RoadModel> GenerateRoadsAggreg(Logic logic, List<LocationModel> locations)
    {
        var list = BuildCityRoads(logic, locations);
        list.AddRange(BuildTownRoads(logic, locations));
        return list; 
    }

    private static List<RoadModel> BuildCityRoads(Logic logic, List<LocationModel> locations)
    {
        var sw2 = new System.Diagnostics.Stopwatch();
        var pathFinder = Game.I.Session.Utility.PathFinder;
        var list = new List<RoadModel>();
        var hexList = new List<HexModel>();
        var mapPack = Game.I.Session.MapGenPackage;
        var seedCells = new List<Cell>();
        var dic = new Dictionary<Cell, LocationModel>();
        var water = Game.I.Session.Data.Terrain["Water"];
        var city = Game.I.Session.Data.LocationTypes[1];
        
        for (int i = 0; i < locations.Count; i++)
        {
            var cell = locations[i].Hex.GetCell();
            if(seedCells.Contains(cell) == false)
            {
                if(locations[i].LocationType == city)
                {
                    seedCells.Add(cell);
                    dic.Add(cell, locations[i]);
                } 
            }
        }
        Func<Cell, List<Cell>> neighborFunc = (c) =>
        {
            return c.Neighbors.Where(n => n.Plate.Land).ToList();
        };
        Func<Cell,Cell,float> edgeCostFunc = (c,d) => c.WorldPos.DistanceTo(d.WorldPos);

        var landCells = mapPack.Cells.Where(c => c.Land).ToList();
        sw2.Start();

        var aggregation = new NodeAggregation<Cell>(landCells, neighborFunc, edgeCostFunc);
        sw2.Stop();
        GD.Print($"constructing road aggregation time: " + sw2.Elapsed);
        sw2.Reset();

        sw2.Start();
        aggregation.NearestFill(seedCells, c => c.WorldPos);
        sw2.Stop();
        GD.Print($"road floodfill time: " + sw2.Elapsed);
        sw2.Reset();

        var pathfind = new System.Diagnostics.Stopwatch();
        var moveType = Game.I.Session.Data.MovementTypes["Land"];

        Func<Cell, Cell, bool> act = (c1, c2) =>
        {
            var loc = dic[c1];
            var nLoc = dic[c2];

            pathfind.Start();
            var path = pathFinder.FindBuildRoadPath(_cityConnectRoadType, loc.Hex, nLoc.Hex);
            if(path == null) return true; 
            var roadPath = logic.Road.BuildRoadPath(path, _cityConnectRoadType.ID);
            
            bool interruptedPath = false; 
            for (int j = 1; j < path.Count - 1; j++)
            {
                var h = path[j];
                if(hexList.Contains(h))
                {
                    interruptedPath = true;
                    path = path.GetRange(0, j);
                    roadPath = roadPath.GetRange(0, j);
                    break;
                }
            }

            list.AddRange(roadPath);
            hexList.AddRange(path);
            //CacheManager.Roads.AddRoads(roadPath);
            if(interruptedPath)
            {
                return false;
            }
            return true; 
        };
        //CacheManager.Roads.Reset();
        
        sw2.Start();
        aggregation.DoNeighborActionConditionalUndirected(act);
        sw2.Stop();
        GD.Print("doing roads neighbor action time: " + sw2.Elapsed);
        sw2.Reset();

        GD.Print("doing roads pathfinding time: "+ pathfind.Elapsed);

        logic.Road.PushRoadsToServer(list);
        return list; 
    }
    private static List<RoadModel> BuildTownRoads(Logic logic, List<LocationModel> locations)
    {
        var sw2 = new System.Diagnostics.Stopwatch();
        var pathFinder = Game.I.Session.Utility.PathFinder;
        var list = new List<RoadModel>();
        var hexList = new List<HexModel>();
        var mapPack = Game.I.Session.MapGenPackage;
        var seedCells = new List<Cell>();
        var dic = new Dictionary<Cell, LocationModel>();
        var water = Game.I.Session.Data.Terrain["Water"];
        var town = Game.I.Session.Data.LocationTypes[2];
        
        for (int i = 0; i < locations.Count; i++)
        {
            var cell = locations[i].Hex.GetCell();
            if(seedCells.Contains(cell) == false)
            {
                    seedCells.Add(cell);
                    dic.Add(cell, locations[i]);
            }
        }
        Func<Cell, List<Cell>> neighborFunc = (c) =>
        {
            return c.Neighbors.Where(n => n.Plate.Land).ToList();
        };
        Func<Cell,Cell,float> edgeCostFunc = (c,d) => c.WorldPos.DistanceTo(d.WorldPos);

        var landCells = mapPack.Cells.Where(c => c.Land).ToList();
        sw2.Start();

        var aggregation = new NodeAggregation<Cell>(landCells, neighborFunc, edgeCostFunc);
        sw2.Stop();
        GD.Print($"constructing road aggregation time: " + sw2.Elapsed);
        sw2.Reset();

        sw2.Start();
        aggregation.NearestFill(seedCells, c => c.WorldPos);
        sw2.Stop();
        GD.Print($"road floodfill time: " + sw2.Elapsed);
        sw2.Reset();

        var pathfind = new System.Diagnostics.Stopwatch();
        var moveType = Game.I.Session.Data.MovementTypes["Land"];

        Func<Cell, Cell, bool> act = (c1, c2) =>
        {
            var loc = dic[c1];
            var nLoc = dic[c2];

            pathfind.Start();
            var path = pathFinder.FindBuildRoadPath(_townConnectRoadType, loc.Hex, nLoc.Hex);
            if(path == null) return true; 
            var roadPath = logic.Road.BuildRoadPath(path, _townConnectRoadType.ID);
            
            bool interruptedPath = false; 
            for (int j = 0; j < path.Count - 1; j++)
            {
                var h = path[j];
                if(h.GetRoadToHex(path[j+1]) != null)
                {
                    interruptedPath = true;
                    path = path.GetRange(0, j);
                    roadPath = roadPath.GetRange(0, j);
                    break;
                }
            }

            list.AddRange(roadPath);
            hexList.AddRange(path);
            //CacheManager.Roads.AddRoads(roadPath);
            if(interruptedPath)
            {
                return false;
            }
            return true; 
        };
        //CacheManager.Roads.Reset();
        
        sw2.Start();
        aggregation.DoNeighborActionConditionalUndirected(act);
        sw2.Stop();
        GD.Print("doing roads neighbor action time: " + sw2.Elapsed);
        sw2.Reset();

        GD.Print("doing roads pathfinding time: "+ pathfind.Elapsed);
        list = list.Where(r => r.Hex1.GetRoadToHex(r.Hex2) == null).ToList();
        logic.Road.PushRoadsToServer(list);
        return list; 
    }
    public static RoadModel BuildRoad(int typeID, int h1, int h2)
    {
        if(typeID == 0) return null;
        int lowID = h1 <= h2 ? h1 : h2;
        int highID = h1 <= h2 ? h2 : h1;
        var from = Cache<HexModel>.GetModel(lowID);
        var to = Cache<HexModel>.GetModel(highID);
        int id = from.GetHexPairIndex(to);
        var model = new RoadModel(Cache<RoadModel>.TakeID(), id, typeID, from.ID, to.ID);
                        
        return model; 
    }
}
