using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;
using KdTree;
using KdTree.Math;

public class CurrentGenerator 
{

    public static void DoCurrents(MapGenPackage pack)
    {
        var water = pack.Cells.Where(c => c.Land == false).ToList();
        var land = pack.Cells.Except(water).ToList();
        float sizeMod = 100f * (float)pack.Parameters.Width; 
        //var waterQuad = new QuadTree<Cell>(20, water, c => c.WorldPos, pack.Bounds);
        
        var waterKD = new KdTree<float, Cell>(2, new FloatMath());
        foreach (var c in water)
        {
            waterKD.Add(c.WorldPos.ToArray(), c);
        }
        
        var equator = new List<Cell>();
        var cold = new List<Cell>();

        Func<float, float> moistureFromTemp = (f) => 70f;//((f + 100f) / 200f ) * 90f;
        float equatorY = pack.Parameters.RealHeight / 2f;
        foreach (var c in water)
        {
            float dist = Mathf.Abs(equatorY - c.WorldPos.y);
            float distNormalized = dist / equatorY;
            //c.Temperature = 100f * (1f - (dist / equatorY));
            if(distNormalized < .15f) 
            {
                c.Temperature = 100f * (1f - (dist / equatorY)); 
                c.Moisture = moistureFromTemp(c.Temperature); 
                equator.Add(c);
            }
            else cold.Add(c);

        }
        //var equatorQuad = new QuadTree<Cell>(20, equator, c => c.WorldPos, pack.Bounds);
        var equatorKD = new KdTree<float, Cell>(2, new FloatMath());
        foreach (var c in equator)
        {
            equatorKD.Add(c.WorldPos.ToArray(), c);
        }
        Func<Cell, Cell, float> waterEdgeCost = (c,d) => 
        { 
            if(c.Land || d.Land) return Mathf.Inf;
            if(c.Neighbors.Contains(d)) return c.WorldPos.DistanceTo(d.WorldPos); 
            return Mathf.Inf;
        };
        Func<Cell,List<Cell>> waterNeighborFunc = c => c.Neighbors.Where(n => n.Plate.Land == false).ToList();
        Func<Cell,Cell,float> heuristicFunc = (c,d) => c.WorldPos.DistanceTo(d.WorldPos);
            
        foreach (var c in water)
        {
            if(equator.Contains(c)) continue; 
            //var closeEquator = equatorQuad.GetClosestElement(c.WorldPos);
            var closeEquator = equatorKD.GetNearestNeighbours(c.WorldPos.ToArray(), 1)[0].Value;
            var path = GenericPathFinder.FindPath(waterEdgeCost, waterNeighborFunc, heuristicFunc, c, closeEquator);
            float dist = Mathf.Abs(equatorY - c.WorldPos.y);
            float distNormalized = dist / equatorY;
            float latTemp = 100f * (1f - (dist / equatorY));
            if(path == null)
            {
                c.Temperature = latTemp;
                c.Temperature = Mathf.Clamp(c.Temperature, 0, 100f);
                c.Moisture = moistureFromTemp(c.Temperature); 
            } 
            else 
            {
                var diffTemp = 100f - (float)path.Count * 4f;
                c.Temperature = Mathf.Max(diffTemp, latTemp);
                c.Temperature = Mathf.Clamp(c.Temperature, 0, 100f);
                c.Moisture = moistureFromTemp(c.Temperature); 
            }
        }
        foreach (var c in land)
        {
            float dist = Mathf.Abs(equatorY - c.WorldPos.y);
            float distNormalized = dist / equatorY;
            float latTemp = 100f * (1f - (dist / equatorY));
            c.Temperature = latTemp;
            Func<Cell,Cell,float> landEdgeCost = (c,d) => 
            {
                if(c.Neighbors.Contains(d))
                {
                    float dist = c.WorldPos.DistanceTo(d.WorldPos);
                    Vector2 travel = d.WorldPos - c.WorldPos;
                    if(pack.MountainRanges.Contains(c,d))
                    { 
                        float mtnPenalty = pack.MountainRanges[c,d] * 2000f;
                        dist += mtnPenalty;
                    }
                    float mult = GetWindVectorCostMultiplier(travel, distNormalized);
                    //GD.Print("multiplier: "  + mult);
                    return mult*dist*sizeMod; 
                }
                return Mathf.Inf;
            };
            //var closeWater = waterQuad.GetClosestElement(c.WorldPos);
            
            var closeWater = waterKD.GetNearestNeighbours(c.WorldPos.ToArray(), 1)[0].Value;
            
            var path = GenericPathFinder.FindPath(landEdgeCost, c => c.Neighbors, heuristicFunc, c, closeWater);
            if(path == null)
            {
                return;
            }
            else
            {
                c.PathToWater = path; 
                var diffTemp = closeWater.Temperature - c.Temperature;
                c.Temperature += diffTemp / path.Count;
                c.Temperature = Mathf.Clamp(c.Temperature, 0, 100f);
                c.Moisture = 2f * moistureFromTemp(c.Temperature) / path.Count; 
            }
        }

        foreach (var pre in pack.Hexes)
        {
            pre.Moisture = pre.Cell.Moisture + Game.I.Random.RandfRange(-5f, 5f);
        }
    }

    private static float GetWindVectorCostMultiplier(Vector2 travelVector, float normalizedDistFromEquator)
    {
        float nrm = normalizedDistFromEquator * 2f * Mathf.Pi;
        float windVectorStr = 1f - Mathf.Cos(nrm);
        Vector2 windVector = Vector2.Left.Rotated(nrm) * windVectorStr;
        float angle = windVector.AngleTo(travelVector);
        float mult = ( -(windVectorStr * Mathf.Cos(angle)) + 1f) / 2f;
        return mult; 
    }
}
