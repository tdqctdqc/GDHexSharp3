using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;
using System.Threading.Tasks;
using KdTree;
using KdTree.Math;

public class MapGenPackage 
{
    public List<Continent> Continents { get; private set; }
    public List<Plate> Plates { get; private set; }
    public List<Cell> Cells { get; private set; }
    public List<PreHex> Hexes { get; private set; }
    public Dictionary<Vector3, PreHex> HexesByCube { get; private set; }
    public Dictionary<int, PreHex> HexesByID { get; private set; }
    public GameGenerationParameters Parameters { get; private set; }
    public Dictionary<int, List<int>> HexNeighbors { get; private set; } 
    public Dictionary<int, List<int>> EdgeNeighbors { get; private set; } 
    public PairDictionary<Cell, float> MountainRanges { get; private set; }
    public Rectangle Bounds { get; private set; }
    public void Generate(GameGenerationParameters prms)
    {
        bool benchmark = true; 
        var sw = new System.Diagnostics.Stopwatch();
        Parameters = prms; 
        Bounds = new Rectangle(Vector2.Zero, new Vector2(prms.RealWidth, prms.RealHeight));
        Hexes = new List<PreHex>();
        HexesByCube = new Dictionary<Vector3, PreHex>();
        HexesByID = new Dictionary<int, PreHex>();
        Cells = new List<Cell>();
        Plates = new List<Plate>();
        Continents = new List<Continent>();

        HexNeighbors = new Dictionary<int, List<int>>();
        EdgeNeighbors = new Dictionary<int, List<int>>();
        MountainRanges = new PairDictionary<Cell, float>();
        sw.Start();
        GenerateHexNeighbors();
        GenerateEdgeNeighbors();
        sw.Stop();
        if(benchmark) GD.Print("Generating neighbors time: " + sw.Elapsed);
        sw.Reset();


        sw.Start();
        GenerateHexes();
        sw.Stop();
        if(benchmark) GD.Print("Generating hexes time: " + sw.Elapsed);
        sw.Reset();

        sw.Start();
        GenerateCells(prms.Width * prms.Height / 10);
        sw.Stop();
        if(benchmark) GD.Print("Generating cells time: " + sw.Elapsed);
        sw.Reset();

        sw.Start();
        CellsPickHexes();
        sw.Stop();
        if(benchmark) GD.Print("cells pick hexes time: " + sw.Elapsed);
        sw.Reset();


        sw.Start();
        FindCellNeighbors();
        sw.Stop();
        if(benchmark) GD.Print("cells find neighbors time: " + sw.Elapsed);
        sw.Reset();

        sw.Start();
        GeneratePlates(Cells.Count / 10);
        sw.Stop();
        if(benchmark) GD.Print("generate plates time: " + sw.Elapsed);
        sw.Reset();

        sw.Start();
        PlatesPickCellsFloodfill();
        sw.Stop();
        if(benchmark) GD.Print("plates pick cells time: " + sw.Elapsed);
        sw.Reset();

        sw.Start();
        GenerateContinents(Parameters.NumContinents);
        sw.Stop();
        if(benchmark) GD.Print("generate continents time: " + sw.Elapsed);
        sw.Reset();

        sw.Start();
        ContinentsPickPlatesFloodfillRatio();
        sw.Stop();
        if(benchmark) GD.Print("continents pick plates time: " + sw.Elapsed);
        sw.Reset();

        sw.Start();
        GenerateColors();
        sw.Stop();
        if(benchmark) GD.Print("generating colors time: " + sw.Elapsed);
        sw.Reset();
    }
    private void GenerateHexNeighbors()
    {
        for (int j = 0; j < Parameters.Height; j++)
        {
            for (int i = 0; i < Parameters.Width; i++)
            {
                int id = i + Parameters.Width * j + 1;
                var ns = id.GenerateNeighborIDs();
                HexNeighbors.Add(id, ns);
            }
        }
    }
    private void GenerateEdgeNeighbors()
    {
        foreach (var entry in HexNeighbors)
        {
            int id = entry.Key;
            var ns = entry.Value; 
            foreach (var n in ns)
            {
                int pairID = id.GetHexPairIndex(n);
                if(EdgeNeighbors.ContainsKey(pairID)) continue; 
                var edgeNeighbors = pairID.GenerateEdgeNeighbors();
                EdgeNeighbors.Add(pairID, edgeNeighbors);
            }
        }
    }
    private void GenerateHexes()
    {
        for (int j = 0; j < Parameters.Height; j++)
        {
            for (int i = 0; i < Parameters.Width; i++)
            {
                int id = i + Parameters.Width * j + 1;
                var prehex = new PreHex()
                                    {
                                        ID = id,
                                        Coords = new Vector2(i, j),
                                        CubeCoords = new Vector2(i, j).OffsetToCube(),
                                        Cell = null,
                                        Neighbors = new List<PreHex>()
                                    };
                Hexes.Add(prehex);
                HexesByCube.Add(prehex.CubeCoords, prehex);
                HexesByID.Add(prehex.ID, prehex);
            }
        }

        Action<PreHex> action = (hex) => 
        {
            foreach (var dir in Constants.HexDirs)
            {
                var nCube = hex.CubeCoords + dir;
                if(HexesByCube.ContainsKey(nCube))
                {
                    hex.Neighbors.Add(HexesByCube[nCube]);
                }
            }
        };
        Parallel.ForEach(Hexes, action);

        
    }
    private void GenerateCells(int numCells)
    {
        var seeds = Hexes.GetNRandomElements(numCells);
        for (int i = 0; i < numCells; i++)
        {
            int id = i;
            var cellHex = seeds[i];
            var cell = new Cell(id, cellHex);
            Cells.Add(cell);
        }
    }
    private void CellsPickHexes()
    {
        var kd = new KdTree<float, Cell>(2, new FloatMath());
        foreach (var c in Cells)
        {
            kd.Add(c.WorldPos.ToArray(), c);
        }

        foreach (var h in Hexes)
        {
            var n = kd.GetNearestNeighbours(h.WorldPos.ToArray(), 1)[0].Value;
            n.AddHex(h);
        }
    }
    
    private void FindCellNeighbors()
    {
        Action<Cell> action = (c) => 
        {
            var neighborCells = c.BorderingHexes.Select(h => h.Cell).ToHashSet();
            foreach (var n in neighborCells)
            {
                c.AddNeighbor(n);
            }
        };
        Parallel.ForEach(Cells, action);
    }
    
    private void GeneratePlates(int numPlates)
    {
        var seeds = Cells.GetNRandomElements(numPlates);
        for (int i = 0; i < seeds.Count; i++)
        {
            int id  = i;
            var plateCell = seeds[i];
            var plate = new Plate(id, plateCell);
            Plates.Add(plate);
        }        
    }
    private void PlatesPickCellsNearest()
    {
        var kd = new KdTree<float, Plate>(2, new FloatMath());
        foreach (var c in Plates)
        {
            kd.Add(c.WorldPos.ToArray(), c);
        }

        foreach (var c in Cells)
        {
            var n = kd.GetNearestNeighbours(c.WorldPos.ToArray(), 1)[0].Value;
            n.AddCell(c);
        }
    }
    private void PlatesPickCellsFloodfill()
    {
        var seeds = Plates.Select(c => c.Children[0]).ToList();
        var aggreg = new NodeAggregation<Cell>(Cells, p => p.Neighbors, (p,q) => 1f);
        
        aggreg.Floodfill(seeds, (p,q) => p.Plate.AddCell(q));
    }
    private void GenerateContinents(int numContinents)
    {
        //var landSeeds = Plates.GetNRandomElements(numContinents);
        var landPoses = new List<Vector2>();
        float minDist = Mathf.Max(Parameters.RealWidth, Parameters.RealHeight) / (float)numContinents;
        Func<Vector2> randPos = () => 
        {
            float x = Game.I.Random.RandfRange(0f, Parameters.RealWidth);
            float y = Game.I.Random.RandfRange(0f, Parameters.RealHeight);
            return new Vector2(x,y);
        };
        for (int i = 0; i < numContinents; i++)
        {
            var pos = randPos();
            while(landPoses.Where(l => l.DistanceTo(pos) < minDist).Count() > 0)
            {
                pos = randPos();
            }
            landPoses.Add(pos);
        }
        var plateKD = new KdTree<float, Plate>(2, new FloatMath());
        foreach (var plate in Plates)
        {
            plateKD.Add(plate.WorldPos.ToArray(), plate);
        }
        var landSeeds = new List<Plate>();
        foreach (var pos in landPoses)
        {
            var plate = plateKD.GetNearestNeighbours(pos.ToArray(), 1)[0].Value;
            landSeeds.Add(plate);
        }
        //var landSeeds = Plates.GetNRandomElements(numContinents);

        var landConts = new List<Continent>();
        
        for (int i = 0; i < landSeeds.Count; i++)
        {
            int id  = i;
            var continentPlate = landSeeds[i];
            var cont = new Continent(id, continentPlate, true);
            landConts.Add(cont);
        }      

        int[,] marker = new int[landConts.Count, landConts.Count];
        var oceanPoints = new List<Vector2>();
        for (int i = 0; i < landConts.Count; i++)
        {
            for (int j = 0; j < landConts.Count; j++)
            {
                if(i == j) continue; 
                if(marker[i,j] == 1 || marker[j,i] == 1) continue; 
                var cont1 = landConts[i];
                var cont2 = landConts[j];
                var midPoint = (cont1.WorldPos + cont2.WorldPos) / 2;
                var landContsRanked = landConts.OrderBy(c => c.WorldPos.DistanceTo(midPoint));
                bool buildOcean = false;
                if(landContsRanked.ElementAt(0) == cont1 && landContsRanked.ElementAt(1) == cont2)
                {
                    buildOcean = true;
                }
                if(landContsRanked.ElementAt(0) == cont2 && landContsRanked.ElementAt(1) == cont1)
                {
                    buildOcean = true;
                }
                if(buildOcean)
                {
                    oceanPoints.Add(midPoint);
                }
                marker[i,j] = 1;
                marker[j,i] = 1;
            }
        }
        int randomSeasCount = numContinents * 2;
        for (int i = 0; i < randomSeasCount; i++)
        {
            float x = Game.I.Random.RandfRange(0f, Parameters.RealWidth);
            float y = Game.I.Random.RandfRange(0f, Parameters.RealHeight);
            oceanPoints.Add(new Vector2(x,y));
        }
        var oceanConts = new List<Continent>();

        var bounds = new Rectangle(Vector2.NegOne * Constants.HexRadius, new Vector2(Parameters.RealWidth + Constants.HexRadius, Parameters.RealHeight + Constants.HexRadius));
        Func<Plate, Vector2> posFunc = p => p.WorldPos;
        var freePlates = Plates.Where(p => p.Continent == null).ToList();
        var plateQuad = new QuadTree<Plate>(freePlates.Count / 50, freePlates, posFunc, bounds);

        float edgeOceanChance = .75f;

        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                if(i == 1 && j == 1) continue; 
                float rand = Game.I.Random.RandfRange(0f,1f);
                if(rand <= edgeOceanChance)
                {
                    var x = Parameters.RealWidth / 2f * (float)i;
                    var y = Parameters.RealHeight / 2f * (float)j;
                    var pos = new Vector2(x,y);
                    oceanPoints.Add(pos);
                }
            }
        }
        
        for (int i = 0; i < oceanPoints.Count; i++)
        {
            int id = i + landConts.Count;
            var continentPlate = plateQuad.GetClosestElement(oceanPoints[i]);
            if(continentPlate.Continent != null) continue;  
            var cont = new Continent(id, continentPlate, false);
            oceanConts.Add(cont);
        }

        Continents.AddRange(landConts);
        Continents.AddRange(oceanConts);
    }
    private void ContinentsPickPlatesFloodfillRatio()
    {
        var pcLand = Parameters.PercentLand;
        var waterConts = Continents.Where(c => c.Land == false).ToList();
        var landConts = Continents.Where(c => c.Land).ToList();
        var waterKD = new KdTree<float, Continent>(2, new FloatMath());
        var landKD= new KdTree<float, Continent>(2, new FloatMath());
        foreach (var land in landConts)
        {
            landKD.Add(land.WorldPos.ToArray(), land);
        }
        foreach (var water in waterConts)
        {
            waterKD.Add(water.WorldPos.ToArray(), water);
        }
        int index = 0;
        while((float)landConts.Sum(c => c.Children.Count) / (float)Plates.Count < (pcLand))
        {
            var cont = landConts[index % landConts.Count];
            index++;
            var borderingOpenPlates = cont
                                        .BorderingPlates
                                        .Where(p => p.Continent == null)
                                        .OrderBy(p => p.WorldPos.DistanceTo(cont.WorldPos))
                                        .ToList();

            var betterOpenPlates = borderingOpenPlates
                                    .Where(p => p.Neighbors
                                                .Where(n => n.Continent != null && n.Continent != cont).Count() == 0)
                                    .ToList();
            if(borderingOpenPlates.Count() == 0) continue;
            List<Plate> eligiblePlates;
            if(betterOpenPlates.Count > 0) eligiblePlates = betterOpenPlates;
            else eligiblePlates = borderingOpenPlates;

            int range = Mathf.Min(eligiblePlates.Count(), 3);
            int rand = Game.I.Random.RandiRange(0, range - 1);    
            var plate = eligiblePlates.ElementAt(rand);
            cont.AddPlate(plate);  
        }
        foreach (var plate in Plates)
        {
            if(plate.Continent == null)
            {
                var cont = waterKD.GetNearestNeighbours(plate.WorldPos.ToArray(), 1)[0].Value;
                cont.AddPlate(plate);
            }
        }

        foreach (var cont in landConts)
        {
            if(Game.I.Random.Randf() < .75f) continue; 
            var borderLandPlates = cont.BorderingPlates.Where(p => p.Land).ToList();
            foreach (var b in borderLandPlates)
            {
                var bCells = b.Children.Where(c => c.Neighbors.Where(n => n.Plate.Continent == cont).Count() > 0).ToList();
                
                var water = waterKD.GetNearestNeighbours(b.WorldPos.ToArray(), 1)[0].Value;
                var waterPlate = water.Children.OrderBy(p => p.WorldPos.DistanceTo(b.WorldPos)).ElementAt(0);
                foreach (var bCell in bCells)
                {
                    waterPlate.AddCell(bCell);
                }
            }
        }
    }
    private void ContinentsPickPlatesFloodfillRatioOld()
    {
        //var landSeeds = seeds.Where(s => s.Land).ToList();
        var landSeeds = Continents.Where(s => s.Land).Select(c => c.Children[0]).ToList();
        //var waterSeeds = seeds.Where(s => s.Land == false).ToList();
        var waterSeeds = Continents.Where(s => s.Land == false).Select(c => c.Children[0]).ToList();
        var seedLists = new List<List<Plate>>(){landSeeds, waterSeeds};
        var weights = new List<float>(){Parameters.PercentLand, 1f - Parameters.PercentLand};
        var aggreg = new NodeAggregation<Plate>(Plates, p => p.Neighbors, (p,q) => 1f);
        aggreg.FloodfillRatio(seedLists, weights, (p,q) => p.Continent.AddPlate(q));
    }
    private void ContinentsPickPlatesNearest()
    {
        var kd = new KdTree<float, Continent>(2, new FloatMath());
        foreach (var c in Continents)
        {
            kd.Add(c.WorldPos.ToArray(), c);
        }

        foreach (var c in Plates)
        {
            var n = kd.GetNearestNeighbours(c.WorldPos.ToArray(), 1)[0].Value;
            n.AddPlate(c);
        }
    }
    private void GenerateColors()
    {
        Parallel.ForEach(Continents, c => c.GenerateColor());
        Parallel.ForEach(Plates, c => c.GenerateColor());
        Parallel.ForEach(Cells, c => c.GenerateColor());
    }
}
