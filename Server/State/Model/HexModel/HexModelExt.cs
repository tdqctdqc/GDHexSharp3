
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HexWargame
{
public static class HexModelExt
{
    public static float GetHexEdgeCost(this HexModel from, HexModel to, MovementType moveType)
    {
        if(to.Units.Count >= Constants.MaxUnitsInHex) return Mathf.Inf; 
        var road = from.GetRoadToHex(to);
        var river = from.GetRiverToHex(to);
        float riverCost = 0f;
        if(river != null) riverCost = moveType.RiverCosts[river.RiverType];


        float terrainCost = (riverCost + moveType.TerrainCosts[to.Terrain]);
        if(road != null)
        {
            float roadCost = moveType.RoadCosts[road.RoadType];
            if(roadCost < terrainCost) return roadCost; 
        }
        return terrainCost;
    }
    public static HexModel GetNorth(this HexModel model) 
    {
        var n = CubeToOffset(model.CubeCoords + Constants.North);
        return Cache<HexModel>.GetModel(n.GetHexIDFromCoords());
    }
    public static HexModel GetNorthEast(this HexModel model) 
    {
        var ne = CubeToOffset(model.CubeCoords + Constants.NorthEast);
        return Cache<HexModel>.GetModel(ne.GetHexIDFromCoords());
    }
    public static HexModel GetSouthEast(this HexModel model) 
    {
        var se = CubeToOffset(model.CubeCoords + Constants.SouthEast);
        return Cache<HexModel>.GetModel(se.GetHexIDFromCoords());
    }
    public static HexModel GetSouth(this HexModel model) 
    {
        var s = CubeToOffset(model.CubeCoords + Constants.South);
        return Cache<HexModel>.GetModel(s.GetHexIDFromCoords());
    }
    public static HexModel GetSouthWest(this HexModel model) 
    {
        var sw = CubeToOffset(model.CubeCoords + Constants.SouthWest);
        return Cache<HexModel>.GetModel(sw.GetHexIDFromCoords());
    }

    public static HexModel GetNorthWest(this HexModel model) 
    {
        var nw = CubeToOffset(model.CubeCoords + Constants.NorthWest);
        return Cache<HexModel>.GetModel(nw.GetHexIDFromCoords());
    }


    public static List<HexModel> GetNeighbors(this HexModel model)
    {
        var ids = Game.I.Session.MapGenPackage.HexNeighbors[model.ID];
        
        return Cache<HexModel>.GetModels(ids);
    }
    public static bool CoordsAreInBounds(this Vector2 coords)
    {
        int mapWidth = Game.I.Session.Params.Width;
        int mapHeight = Game.I.Session.Params.Height;
        if (coords.x < 0 || coords.x > mapWidth - 1 || coords.y < 0 || coords.y > mapHeight - 1) return false;
        return true; 
    }
    public static List<int> GetNeighborIDs(this int hexID)
    {
        return Game.I.Session.MapGenPackage.HexNeighbors[hexID];
    }

    public static List<int> GenerateNeighborIDs(this int model)
    {
        var possNeighborIDs = new List<int>(6);
        var cubeCoords = model.GetCoordsFromHexID().OffsetToCube();

        var n = CubeToOffset(cubeCoords + Constants.North);
        if(CoordsAreInBounds(n))  possNeighborIDs.Add(GetHexIDFromCoords(n));

        var ne = CubeToOffset(cubeCoords + Constants.NorthEast);
        if(CoordsAreInBounds(ne)) possNeighborIDs.Add(GetHexIDFromCoords(ne));

        var se = CubeToOffset(cubeCoords + Constants.SouthEast);
        if(CoordsAreInBounds(se)) possNeighborIDs.Add(GetHexIDFromCoords(se));

        var s = CubeToOffset(cubeCoords + Constants.South);
        if(CoordsAreInBounds(s)) possNeighborIDs.Add(GetHexIDFromCoords(s));

        var sw = CubeToOffset(cubeCoords + Constants.SouthWest);
        if(CoordsAreInBounds(sw)) possNeighborIDs.Add(GetHexIDFromCoords(sw));

        var nw = CubeToOffset(cubeCoords + Constants.NorthWest);
        if(CoordsAreInBounds(nw)) possNeighborIDs.Add(GetHexIDFromCoords(nw));  

        return possNeighborIDs; 
    }
    public static Vector2 CubeToOffset(this Vector3 cube)
    {
        int offsetX = (int)cube.x;
        int offsetY = (int)cube.y + (int)(  ( cube.x + ((uint)cube.x & 1) )/2  );
        Vector2 offset = new Vector2(offsetX, offsetY);
        return offset; 
    }
    public static Vector3 OffsetToCube(this Vector2 offset) 
    {
        int cubeX = (int)offset.x; 
        int cubeY = (int)offset.y - (int)(  ( offset.x + ((uint)offset.x & 1) )/2  );
        int cubeZ = -cubeX - cubeY; 
        Vector3 cube = new Vector3(cubeX, cubeY, cubeZ);
        return cube; 
    }
    public static int GetHexIDFromCoords(this Vector2 coords)
    {
        int mapWidth = Game.I.Session.Params.Width;

        return (int)(coords.y * mapWidth + coords.x + 1);
    }
    public static Vector2 GetCoordsFromHexID(this int id)
    {
        int mapWidth = Game.I.Session.Params.Width;
        int x = (id - 1) % mapWidth;
        int y = (id - 1) / mapWidth;
        return new Vector2(x,y);
    }
    public static List<HexModel> GetHexesInRadius(this HexModel hex, int radius)
    {
        int numPossHexesInRadius = (radius) * (radius + 1) / 2;
        var ids = new List<int>(numPossHexesInRadius);

        for (int i = -radius; i < radius + 1; i++)
        {
            for (   int j = Mathf.Max(-radius, -i - radius); 
                    j < Mathf.Min(radius, -i + radius) + 1; 
                    j++)
            {
                int x = i + (int)hex.CubeCoords.x;
                int y = j + (int)hex.CubeCoords.y;
                int z = -x-y;
                var cube = new Vector3(x,y,z);
                var offset = cube.CubeToOffset();
                if(offset.CoordsAreInBounds())
                {
                    ids.Add(offset.GetHexIDFromCoords());
                }
            }
        }
        return Cache<HexModel>.GetModels(ids); 
    }
    public static List<List<HexModel>> GetHexesInRadiusSorted(this HexModel hex, int radius)
    {
        var list = new List<List<HexModel>>(radius);

        for (int i = 0; i < radius; i++)
        {
            list.Add(GetHexesInRing(hex, i));
        }

        return list.ToList();
    }
    public static List<HexModel> GetHexesInRing(this HexModel hex, int radius)
    {
        if(radius == 0) return new List<HexModel>(){hex};
        var ids = new List<int>(6 * radius);
        var dir = Constants.SouthWest * radius;
        var cube = hex.CubeCoords + dir; 
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                var offset = cube.CubeToOffset();
                if(offset.CoordsAreInBounds())
                {
                    ids.Add(offset.GetHexIDFromCoords());
                    cube = Constants.HexDirs[i] + cube; 
                }
            }
        }
        return Cache<HexModel>.GetModels(ids); 
    }
    public static float GetHexAngle(this HexModel h1, HexModel h2)
    {
        return .5f * Mathf.Pi - (h2.WorldPos - h1.WorldPos).AngleTo(Vector2.Up);
    }
    public static int GetHexDistance(this HexModel h1, HexModel h2)
    {
        var h1cube = h1.Coords.OffsetToCube();
        var h2cube = h2.Coords.OffsetToCube();
        return (int)((Math.Abs(h1cube.x - h2cube.x) + Math.Abs(h1cube.y - h2cube.y) + Math.Abs(h1cube.z - h2cube.z)) / 2);
    }
    public static Vector2 GetWorldPosFromOffset(this Vector2 offset)
    {   
        float hexRadius = Constants.HexRadius;
        var pos = new Vector2(offset.x * hexRadius * 1.5f, offset.y * hexRadius * .866f * 2f);
        if(offset.x % 2 != 1)
        {
            pos.y += hexRadius * .866f;
        }
        return pos;
    }

    public static int GetHexPairIndex(this HexModel h1, HexModel h2)
    {
        if(h1.GetHexDistance(h2) != 1) return -1;
        var low = h1.ID < h2.ID ? h1 : h2;
        var high = h1.ID < h2.ID ? h2 : h1;

        int dirIndex = low.GetDirIndex(high);
        return low.ID * 6 + dirIndex;
    }
    public static int GetHexPairIndex(this PreHex h1, PreHex h2)
    {
        if(h1.Neighbors.Contains(h2) == false) return -1;
        var low = h1.ID < h2.ID ? h1 : h2;
        var high = h1.ID < h2.ID ? h2 : h1;

        int dirIndex = low.GetDirIndex(high);
        return low.ID * 6 + dirIndex;
    }
    public static int GetHexPairIndex(this int h1, int h2)
    {
        var neighbors = Game.I.Session.MapGenPackage.HexNeighbors[h1];
        if(neighbors.Contains(h2) == false) return -1;
        var low = h1 < h2 ? h1 : h2;
        var high = h1 < h2 ? h2 : h1;

        int dirIndex = low.GetDirIndex(high);
        return low * 6 + dirIndex;
    }
    public static Tuple<HexModel, HexModel> GetHexesFromPairIndex(this int pairIndex)
    {
        var low = Cache<HexModel>.GetModel(pairIndex / 6);
        int dir = pairIndex % 6;
        var high = GetNeighborFromDir(low, dir);
        return new Tuple<HexModel, HexModel>(low, high);
    }

    public static List<HexModel> GetOrthogonalHexesFromPairIndex(this int pairID)
    {
        var pairHexes = GetHexesFromPairIndex(pairID);
        return pairHexes.Item1.GetNeighbors().Intersect(pairHexes.Item2.GetNeighbors()).ToList();
    }
    public static Tuple<int, int> GetHexIDsFromPairIndex(this int pairIndex)
    {
        var low = pairIndex / 6;
        int dir = pairIndex % 6;
        var high = GetNeighborIDFromDir(low, dir);
        return new Tuple<int, int>(low, high);
    }
    public static List<int> GetOrthogonalHexIDsFromPairIndex(this int pairID)
    {
        var pairHexes = GetHexIDsFromPairIndex(pairID);
        var n1 = Game.I.Session.MapGenPackage.HexNeighbors[pairHexes.Item1];
        var n2 = Game.I.Session.MapGenPackage.HexNeighbors[pairHexes.Item2];
        return n1.Intersect(n2).ToList();
    }
    public static HexModel GetNeighborFromDir(this HexModel from, int dir)
    {
        var dirOffset = Constants.HexDirs[dir];
        var coords = (from.CubeCoords + dirOffset).CubeToOffset();
        var toID = coords.GetHexIDFromCoords();
        return Cache<HexModel>.GetModel(toID);
    }
    public static int GetNeighborIDFromDir(this int from, int dir)
    {
        var dirOffset = Constants.HexDirs[dir];
        var coords = (from.GetCoordsFromHexID().OffsetToCube() + dirOffset).CubeToOffset();
        var toID = coords.GetHexIDFromCoords();
        return toID;
    }
    public static int GetDirIndex(this HexModel from, HexModel to)
    {
        var dir = to.CubeCoords - from.CubeCoords;
        if(Constants.HexDirs.Contains(dir) == false)
        {
            throw new ArgumentException("Tried to get direction index for non adjacent hexes");
        }
        int dirIndex = Constants.HexDirs.IndexOf(dir);
        return dirIndex;
    }
    public static int GetDirIndex(this PreHex from, PreHex to)
    {
        var dir = to.CubeCoords - from.CubeCoords;
        if(Constants.HexDirs.Contains(dir) == false)
        {
            throw new ArgumentException("Tried to get direction index for non adjacent hexes");
        }
        int dirIndex = Constants.HexDirs.IndexOf(dir);
        return dirIndex;
    }
    public static int GetDirIndex(this int from, int to)
    {
        var toCube = to.GetCoordsFromHexID().OffsetToCube();
        var fromCube = from.GetCoordsFromHexID().OffsetToCube();
        var dir = toCube - fromCube;
        if(Constants.HexDirs.Contains(dir) == false)
        {
            throw new ArgumentException("Tried to get direction index for non adjacent hexes");
        }
        int dirIndex = Constants.HexDirs.IndexOf(dir);
        return dirIndex;
    }

    public static bool HexPairsTouch(this int pairID1, int pairID2)
    {
        var hexes1 = GetHexesFromPairIndex(pairID1);
        var hexes2 = GetHexesFromPairIndex(pairID2);
        if(hexes1.Item1.ID == hexes2.Item2.ID)
        { 
            if(hexes1.Item2.GetHexDistance(hexes2.Item1) == 1) return true;
        }
        if(hexes1.Item2.ID == hexes2.Item2.ID)
        { 
            if(hexes1.Item1.GetHexDistance(hexes2.Item1) == 1) return true;
        }
        if(hexes1.Item1.ID == hexes2.Item1.ID)
        { 
            if(hexes1.Item2.GetHexDistance(hexes2.Item2) == 1) return true;
        }
        if(hexes1.Item2.ID == hexes2.Item1.ID)
        { 
            if(hexes1.Item1.GetHexDistance(hexes2.Item2) == 1) return true;
        }
        return false;
    }
    public static Vector2 GetPosForPairID(this int pairID)
    {
        var hexes = GetHexIDsFromPairIndex(pairID);
        var pos1 = hexes.Item1.GetCoordsFromHexID().GetWorldPosFromOffset();;
        var pos2 = hexes.Item2.GetCoordsFromHexID().GetWorldPosFromOffset();;
        return (pos1 + pos2) / 2f;
    }
    public static List<int> GetEdgeNeighbors(this int hexPairID)
    {
        return Game.I.Session.MapGenPackage.EdgeNeighbors[hexPairID];
    }
    public static List<int> GenerateEdgeNeighbors(this int hexPairID)
    {
        //return Game.I.Session.MapGenPackage.EdgeNeighbors[hexPairID];
        var edges = new List<int>();
        var hexes = hexPairID.GetHexIDsFromPairIndex();
        var n1 = GenerateNeighborIDs(hexes.Item1);//Game.I.Session.MapGenPackage.HexNeighbors[hexes.Item1];
        var n2 = GenerateNeighborIDs(hexes.Item2);//Game.I.Session.MapGenPackage.HexNeighbors[hexes.Item2];
        var mutualNeighbors = n1.Intersect( n2 ).ToList();
        
        for (int i = 0; i < mutualNeighbors.Count; i++)
        {
            var edge1 = mutualNeighbors[i].GetHexPairIndex(hexes.Item1);
            var edge2 = mutualNeighbors[i].GetHexPairIndex(hexes.Item2);
            edges.Add(edge1);
            edges.Add(edge2);
        }
        return edges; 
    }
    public static HexModel FindHexFromWorldPos(this Vector2 worldPos)
    {
        var radius = Constants.HexRadius;
        int x = (int)(worldPos.x / (radius * 1.5f));
        int y = (int)(worldPos.y / (radius * .866f * 2f));
        int mapWidth = Game.I.Session.Params.Width;
        int mapHeight = Game.I.Session.Params.Height;
        
        float dist = Mathf.Inf; 
        int closeID = -1;
        for (int i = x - 2; i < x + 2; i++)
        {
            for (int j = y - 2; j < y + 2; j++)
            {
                if(i >= 0 && i < mapWidth && j >= 0 && j < mapHeight)
                {
                    var coords = new Vector2(i,j);
                    int tempID = coords.GetHexIDFromCoords();
                    Vector2 tempPos = coords.GetWorldPosFromOffset();
                    float tempDist = tempPos.DistanceTo(worldPos);
                    if(tempDist < dist)
                    {
                        dist = tempDist;
                        closeID = tempID;
                    }
                } 
            }
        }
        return Cache<HexModel>.GetModel(closeID);
    }
    public static Tuple<HexModel, HexModel> FindTwoClosestHexes(this Vector2 worldPos)
    {
        var radius = Constants.HexRadius;
        int x = (int)(worldPos.x / (radius * 1.5f));
        int y = (int)(worldPos.y / (radius * .866f * 2f));
        int mapWidth = Game.I.Session.Params.Width;
        int mapHeight = Game.I.Session.Params.Height;
        
        float closestDist = Mathf.Inf; 
        int closestID = -1;

        float closeDist = Mathf.Inf; 
        int closeID = -1;
        for (int i = x - 2; i < x + 2; i++)
        {
            for (int j = y - 2; j < y + 2; j++)
            {
                if(i >= 0 && i < mapWidth && j >= 0 && j < mapHeight)
                {
                    var coords = new Vector2(i,j);
                    int tempID = coords.GetHexIDFromCoords();
                    Vector2 tempPos = coords.GetWorldPosFromOffset();
                    float tempDist = tempPos.DistanceTo(worldPos);
                    if(tempDist < closestDist)
                    {
                        closeDist = closestDist;
                        closeID = closestID;
                        closestDist = tempDist;
                        closestID = tempID;
                    }
                    else if(tempDist < closeDist)
                    {
                        closeDist = tempDist;
                        closeID = tempID;
                    }
                } 
            }
        }
        var hex1 = Cache<HexModel>.GetModel(closestID);
        var hex2 = Cache<HexModel>.GetModel(closeID);
        return new Tuple<HexModel, HexModel>(hex1, hex2);
    }

    public static Cell GetCell(this HexModel h)
    {
        return Game.I.Session.MapGenPackage.HexesByCube[h.CubeCoords].Cell; 
    }

    public static LocationModel GetLocation(this HexModel h)
    {
        return CacheManager.Locations.GetLocByHexID(h.ID);
    }
} 
}