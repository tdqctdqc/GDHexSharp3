using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HexWargame;
using System.Threading.Tasks;

public class MapGenerator
{
    private static StateLogicInterface _interface => Game.I.Session.Server.StateInterface;
    public static List<HexModel> GenerateMap(GameGenerationParameters prms, List<FactionModel> factions)
    {
        var sw2 = new System.Diagnostics.Stopwatch();

        Game.I.Random.Seed = (ulong)prms.Seed; 

        sw2.Start();
        var mapGenPackage = new MapGenPackage();
        Game.I.Session.MapGenPackage = mapGenPackage;

        mapGenPackage.Generate(prms);
        sw2.Stop();
        GD.Print($"map gen package generating time: {sw2.Elapsed}");
        sw2.Reset();

        sw2.Start();
        FactionsPickCellsGraph2(factions, mapGenPackage.Cells);
        sw2.Stop();
        GD.Print($"factions picking territory time: {sw2.Elapsed}");
        sw2.Reset();

        sw2.Start();
        TectonicGenerator.DoTectonicsContinent(mapGenPackage);
        TectonicGenerator.DoTectonicsPlate(mapGenPackage);
        sw2.Stop();
        GD.Print($"tectonics time: {sw2.Elapsed}");
        sw2.Reset();


        sw2.Start();
        DoErosion(mapGenPackage);
        sw2.Stop();
        GD.Print($"erosion time: {sw2.Elapsed}");
        sw2.Reset();


        sw2.Start();
        CurrentGenerator.DoCurrents(mapGenPackage);
        sw2.Stop();
        GD.Print($"currents time: {sw2.Elapsed}");
        sw2.Reset();

        
        sw2.Start();
        AssignHexTerrains(mapGenPackage);
        sw2.Stop();
        GD.Print($"assign hex terrains time: {sw2.Elapsed}");
        sw2.Reset();


        sw2.Start();
        var hexes = GenerateHexes(mapGenPackage);
        sw2.Stop();
        GD.Print($"generate hex models time: {sw2.Elapsed}");
        sw2.Reset();
        

        for (int i = 0; i < factions.Count; i++)
        {
            hexes[i].FactionID = factions[i].ID;
        }

        

        sw2.Start();
        RiverGenerator.BuildRiversForPack(mapGenPackage);
        sw2.Stop();
        GD.Print($"rivers time: {sw2.Elapsed}");
        sw2.Reset();
        
        sw2.Start();
        _interface.AddModels<HexModel>(hexes);
        sw2.Stop();
        GD.Print($"adding hex models time: {sw2.Elapsed}");
        sw2.Reset();
        
        return hexes;
    }

    private static OpenSimplexNoise SetupNoise(NoiseParameters prms)
    {
        var noise = new OpenSimplexNoise();
        noise.Octaves = prms.Octaves;
        noise.Period = prms.Period;
        noise.Persistence = prms.Persistence;
        return noise; 
    }
    private static void FactionsPickCellsGraph(List<FactionModel> factions, List<Cell> cells)
    {
        var nonNeutralFactions = factions.Where(f => f.ID != 1).ToList();
        var landCells = cells.Where(c => c.Plate.Continent.Land).ToList();
        Func<Cell, List<Cell>> neighborFunc = (c) =>
        {
            if(c.Plate.Land == false) return new List<Cell>();
            return c.Neighbors.Where(n => n.Plate.Land).ToList();
        };
        Func<Cell, Cell, float> edgeCostFunc = (c,d) => 1f;
        var graph = new Graph<Cell>(landCells, neighborFunc, edgeCostFunc);
        
        var seedCellsDic = new Dictionary<Cell, FactionModel>();
        var seedCells = landCells.GetNRandomElements(nonNeutralFactions.Count);
        for (int i = 0; i < nonNeutralFactions.Count; i++)
        {
            seedCellsDic.Add(seedCells[i], nonNeutralFactions[i]);
        }
        Action<Cell, Cell> action = (c, d) =>
        {
            var fac = seedCellsDic[c];
            d.FactionID = fac.ID;
        };
        Action<Cell> blankAction = (c) => c.FactionID = 1;
        
        var partition = new Partition<Cell>(graph);
        partition.DoFloodfillWithBlankAction(seedCells, action, blankAction);
    }

    private static void FactionsPickCellsGraph2(List<FactionModel> factions, List<Cell> cells)
    {
        var nonNeutralFactions = factions.Where(f => f.ID != 1).ToList();
        var landCells = cells.Where(c => c.Plate.Land).ToList();
        Func<Cell, List<Cell>> neighborFunc = (c) =>
        {
            //if(c.Plate.Land == false) return new List<Cell>();
            return c.Neighbors.Where(n => n.Plate.Land).ToList();
        };
        Func<Cell, Cell, float> edgeCostFunc = (c,d) => 1f;
        var landUnionFind = new UnionFind<Cell>((c,d) => true, neighborFunc, c => c.ID);
        foreach (var c in landCells)
        {
            landUnionFind.AddElement(c);
        }
        var landUnions = landUnionFind.GetUnions().Values.ToList();

        var apportion = Apportioner<FactionModel, List<Cell>>.Apportion( landUnions, nonNeutralFactions, landUnions.Select(f => (float)f.Count).ToList(), 1);
        foreach (var a in apportion)
        {
            var landUnion = a.Key; 
            var facs = a.Value;
            var graph = new Graph<Cell>(landUnion, neighborFunc, edgeCostFunc);
            var seedCells = landUnion.GetNRandomElements(facs.Count);
            var seedCellsDic = new Dictionary<Cell, FactionModel>();
            for (int i = 0; i < facs.Count; i++)
            {
                seedCellsDic.Add(seedCells[i], facs[i]);
            }
            Action<Cell, Cell> action = (c, d) =>
            {
                var fac = seedCellsDic[c];
                d.FactionID = fac.ID;
            };
            Action<Cell> blankAction = (c) => c.FactionID = 1;
            
            var partition = new Partition<Cell>(graph);
            partition.DoFloodfillWithBlankAction(seedCells, action, blankAction);
        }


        
        //var seedCells = landCells.GetNRandomElements(nonNeutralFactions.Count);
        
    }

    private static List<HexModel> GenerateHexes(MapGenPackage pack)
    {
        var prms = pack.Parameters;
        var hexes = new List<HexModel>( pack.Parameters.Width * pack.Parameters.Height );
        var height = prms.Height;
        var width = prms.Width; 

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                int id = i + width * j;
                var pre = pack.Hexes[id];
                var hex = new HexModel(pre.ID, (int)pre.Coords.x, (int)pre.Coords.y, pre.FactionID, pre.TerrainID);
                hexes.Add(hex);
            }
        }
        return hexes;
    }
    private static void DoErosion(MapGenPackage pack)
    {
        var prms = pack.Parameters;
        var roughChange = new float[pack.Hexes.Count];
        var range = Enumerable.Range(0, pack.Hexes.Count);
        Parallel.ForEach(range, (i) => DoHexErosion(i, pack, roughChange));
        for (int i = 0; i < pack.Hexes.Count; i++)
        {
            var h = pack.Hexes[i];
            h.Roughness += roughChange[i];
        }
    }
    private static void DoHexErosion(int i, MapGenPackage pack, float[] roughChange)
    {
        var h = pack.Hexes[i];

        if(h.Cell.Land == false) return; 

            var ns = h.Neighbors;
            foreach (var n in ns)
            {
                if(n.Cell.Land == false) 
                {
                    float roughDiff = h.Roughness;
                    roughChange[i] -= h.Roughness / 3f;
                }

                // else if(h.Roughness > n.Roughness)
                // {
                //     float roughDiff = h.Roughness - n.Roughness;
                //     roughChange[i] -= roughDiff / 12f;
                //     int j = pack.Hexes.IndexOf(n);
                //     roughChange[j] += roughDiff / 24f;
                // }
            }
    }

    
    private static void AssignHexTerrains(MapGenPackage pack)
    {
        var prms = pack.Parameters;
        var altNoise = SetupNoise(prms.AltNoiseParams);
        var wetNoise = SetupNoise(prms.WetNoiseParams);

        altNoise.Seed = prms.Seed;
        wetNoise.Seed = prms.Seed + 7;
        Game.I.Random.Seed = (ulong)prms.Seed; 
        int width = prms.Width;
        int height = prms.Height;


        var water = Game.I.Session.Data.Terrain["Water"];
        var items = Enumerable.Range(0,pack.Hexes.Count - 1).ToList();


        Parallel.ForEach(pack.Hexes, AssignHexTerrain);
    }

    private static void AssignHexTerrain(PreHex pre)
    {
        var cell = pre.Cell;
        var plate = cell.Plate; 
        var cont = plate.Continent;
        int fac = cell.FactionID;
        int terrainID;
        var water = Game.I.Session.Data.Terrain["Water"];

        if(cont.Land)
        {
            var terrain = Terrain.GetLandTerrain(pre);
            terrainID = terrain.ID;
        }
        else 
        {
            fac = 1;
            terrainID = water.ID;
        }

        pre.FactionID = fac;
        pre.TerrainID = terrainID;
    }
}
