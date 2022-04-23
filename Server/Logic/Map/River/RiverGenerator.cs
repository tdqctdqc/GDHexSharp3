using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class RiverGenerator 
{
    public static RiverModel BuildRiver(int typeID, int h1, int h2)
    {
        if(typeID == 0) return null;
        int lowID = h1 <= h2 ? h1 : h2;
        int highID = h1 <= h2 ? h2 : h1;
        var from = lowID;
        var to = highID;
        int id = from.GetHexPairIndex(to);
        var model = new RiverModel(Cache<RiverModel>.TakeID(), id, typeID, from, to);
                        
        return model; 
    }


    public static void BuildRiversForPack(MapGenPackage pack)
    {
        bool benchmark = true; 
        var sw = new System.Diagnostics.Stopwatch();
        var pathfind = new System.Diagnostics.Stopwatch();

        var rSegments = new Dictionary<int, float>(); //by pair id
        
        var rPathDic = new PairDictionary<Cell, List<int>>();
       
        var land = pack.Cells.Where(c => c.Plate.Continent.Land);
        for(int k = 0; k < land.Count(); k++)
        {
            var c = land.ElementAt(k);
            var p = c.PathToWater;
            for (int i = 0; i < p.Count - 1; i++)
            {
                var c1 = p[i];
                var c2 = p[i+1];
                List<int> newP;
                if(rPathDic.Contains(c1,c2))
                {
                    newP = rPathDic[c1,c2];
                }
                else
                {
                    pathfind.Start();
                    newP = FindRiverPath(c1,c2);
                    pathfind.Stop();
                    rPathDic.Add(c1,c2,newP);
                }
                for (int j = 1; j < newP.Count; j++)
                {
                    var pairID = newP[j];
                    if(rSegments.ContainsKey(pairID) == false) rSegments.Add(pairID, 0f);
                    rSegments[pairID] += (c1.Moisture + c2.Moisture) / 2f;
                }
            }
        }

        var riversByFlow = Game.I.Session.Data.RiverTypes.RiverTypesByMinFlow;
        float lowestFlow = riversByFlow[0].MinFlow;
        
        var rivers = new List<RiverModel>(rSegments.Count);

        var rUnion = new EdgeUnion();
        var seaPairIds = new HashSet<int>();
        sw.Start();
        foreach (var entry in rSegments)
        {
            var hexes = entry.Key.GetHexIDsFromPairIndex();
            var orthoHexes = entry.Key.GetOrthogonalHexIDsFromPairIndex();
            var water = Game.I.Session.Data.Terrain["Water"];

            if(orthoHexes.Where(h => pack.HexesByID[h].TerrainID == water.ID).Count() > 0) 
            {
                seaPairIds.Add(entry.Key);
            }
            var h1 = pack.HexesByID[hexes.Item1];
            var h2 = pack.HexesByID[hexes.Item2];

            if(h1.TerrainID == water.ID || h2.TerrainID == water.ID) continue; 
            if(entry.Value < lowestFlow) continue;
            rUnion.AddEdge(entry.Key);
        }

        var unions = rUnion.GetUnions();
        foreach (var entry in unions)
        {
            var union = entry.Value; 
            if(union.Intersect(seaPairIds).Count() == 0) continue;

            foreach (var pairID in union)
            {
                int riverTypeID = 0;

                float flow = rSegments[pairID];
                var hexes = pairID.GetHexIDsFromPairIndex();

                for (int i = 0; i < riversByFlow.Count; i++)
                {
                    var r = riversByFlow[i];

                    if(i == riversByFlow.Count - 1 )
                    {
                        riverTypeID = r.ID;
                        break;
                    }
                    var nextR = riversByFlow[i + 1];
                    if(flow < nextR.MinFlow)
                    {
                        riverTypeID = r.ID;
                        break;
                    }
                }
                rivers.Add(BuildRiver(riverTypeID, hexes.Item1, hexes.Item2));
            }
        }


        sw.Stop();
        if(benchmark) GD.Print($"checking river types time: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        Game.I.Session.Server.LogicInterface.River.BuildRivers(rivers);
        sw.Stop();
        if(benchmark) GD.Print($"pushing rivrs to server time: {sw.Elapsed}");
        sw.Reset();

        if(benchmark) GD.Print($"river pathfinding time: {pathfind.Elapsed}");
    }

    private static float GetRiverEdgeCost(int fromPairID, int toPairID)
    {
        Tuple<int, int> hs = toPairID.GetHexIDsFromPairIndex();
        var hexes = Game.I.Session.MapGenPackage.Hexes;
        var h1 = hexes[hs.Item1 - 1];
        var h2 = hexes[hs.Item2 - 1];
        return h1.Roughness + h2.Roughness;
    }
    private static List<int> FindRiverPath(Cell c1, Cell c2)
    {
        var start = c1.HomeHexPairID;
        var end = c2.HomeHexPairID;
        return Game.I.Session.Utility.PathFinder.FindEdgePath(start, end, GetRiverEdgeCost);
    }
}
