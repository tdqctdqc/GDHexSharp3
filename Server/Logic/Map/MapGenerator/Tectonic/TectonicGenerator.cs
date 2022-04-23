using Godot;
using HexWargame;
using System;
using System.Linq;

public class TectonicGenerator 
{
    public static void DoTectonicsPlate(MapGenPackage pack)
    {

        Func<Plate, Plate, bool> tecAction = (p,q) =>
        {
            if(p.Land == false || q.Land == false) return true; 
            float angle = p.Drift.AngleTo(q.Drift);
            if(angle < Mathf.Pi / 3f) return true; 

            float driftStrength = angle / Mathf.Pi; 

            var bord1 = p.GetOuterBorder<Cell, Plate>().Where(c => c.Plate == q).ToList();
            var bord2 = q.GetOuterBorder<Cell, Plate>().Where(c => c.Plate == p).ToList();

            var bord3 = bord1.SelectMany(c => c.GetOuterBorder<PreHex,Cell>())
                            .Where(h => bord2.Contains(h.Cell))
                            .ToHashSet().ToList();
            var bord4 = bord2.SelectMany(c => c.GetOuterBorder<PreHex,Cell>())
                            .Where(h => bord1.Contains(h.Cell))
                            .ToHashSet().ToList();
            var bord = bord3.Union(bord4).ToList();

            foreach (var c in bord1)
            {
                foreach (var n in c.Neighbors)
                {
                    if(bord2.Contains(n))
                    {
                        var mtn = pack.MountainRanges;
                        if(mtn.Contains(c,n))
                        {
                            mtn.Set(c,n, mtn[c,n] + driftStrength);
                        }
                        else
                        {
                            pack.MountainRanges.Add(c,n,driftStrength);
                        }
                    }
                }
            }

            int width = Mathf.FloorToInt(driftStrength * 3);
            float maxRough = driftStrength * 90f;
            float minRough = driftStrength * 65f; 
            var layers = bord.GetBorderLayers(width);

            Action<PreHex, int> act = (h, i) => 
            {
                h.Roughness = Game.I.Random.RandfRange(minRough - 9 * i, maxRough - 7 * i);
            };
            for (int i = 0; i < layers.Count; i++)
            {
                var layer = layers[i];
                
                foreach (var h in layer)
                {
                    act(h, i);
                }
            }
            return true; 
        };

        pack.Plates.DoNeighborActionConditionalUndirected(tecAction);
    }

    public static void DoTectonicsContinent(MapGenPackage pack)
    {
        Func<Continent, Continent, bool> tecAction = (p,q) =>
        {
            if(p.Land == false || q.Land == false) return true; 
            float angle = p.Drift.AngleTo(q.Drift);
            if(angle < Mathf.Pi / 4f) return true; 
            // GD.Print("doing continent tectonics");
            float driftStrength = Mathf.Sqrt(angle / Mathf.Pi); 
            var bord1 = p.GetOuterBorder<Plate, Continent>().Where(c => c.Continent == q).ToList();
            var bord2 = q.GetOuterBorder<Plate, Continent>().Where(c => c.Continent == p).ToList();

            var bord3 = bord1.SelectMany(c => c.GetOuterBorder<Cell,Plate>())
                            .Where(h => bord2.Contains(h.Plate))
                            .ToHashSet().ToList();
            var bord4 = bord2.SelectMany(c => c.GetOuterBorder<Cell,Plate>())
                            .Where(h => bord1.Contains(h.Plate))
                            .ToHashSet().ToList();
            foreach (var c in bord3)
            {
                foreach (var n in c.Neighbors)
                {
                    if(bord4.Contains(n))
                    {
                        var mtn = pack.MountainRanges;
                        if(mtn.Contains(c,n))
                        {
                            mtn.Set(c,n, mtn[c,n] + driftStrength);
                        }
                        else
                        {
                            pack.MountainRanges.Add(c,n,driftStrength);
                        }
                    }
                }
            }
            var bord5 = bord3.SelectMany(c => c.GetOuterBorder<PreHex,Cell>())
                            .Where(h => bord4.Contains(h.Cell))
                            .ToHashSet().ToList();
            var bord6 = bord4.SelectMany(c => c.GetOuterBorder<PreHex,Cell>())
                            .Where(h => bord3.Contains(h.Cell))
                            .ToHashSet().ToList();
            var bord = bord5.Union(bord6).ToList();


            int width = Mathf.FloorToInt(driftStrength * 5);
            float maxRough = driftStrength * 100f;
            float minRough = driftStrength * 75f; 
            var layers = bord.GetBorderLayers(width);

            Action<PreHex, int> act = (h, i) => 
            {
                h.Roughness = Game.I.Random.RandfRange(minRough - 7 * i, maxRough - 5 * i);
            };
            for (int i = 0; i < layers.Count; i++)
            {
                var layer = layers[i];
                
                foreach (var h in layer)
                {
                    act(h, i);
                }
            }
            return true; 
        };

        pack.Continents.DoNeighborActionConditionalUndirected(tecAction);
    }
}
