using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public static class BowerWatsonTriangulator 
{
    public static List<List<T>> GetNetworkOfObjects<T>(List<T> objects, Func<T, Vector2> pointFunc)
    {
        //each objects point must be unique
        var dic = objects.ToDictionary(o => pointFunc(o));//new Dictionary<Vector2, T>();

        var points = objects.Select(o => pointFunc(o)).ToList();
        var triangles = Triangulate(points);
        
        var result = new List<List<T>>();
        foreach (var tri in triangles)
        {
            var o1 = dic[tri.A];
            var o2 = dic[tri.B];
            var o3 = dic[tri.C];

            if(result.Where(r => r.Contains(o1) && r.Contains(o2)).Count() != 0)
            {
                result.Add(new List<T>(){o1, o2});
            }
            if(result.Where(r => r.Contains(o1) && r.Contains(o3)).Count() != 0)
            {
                result.Add(new List<T>(){o1, o3});
            }
            if(result.Where(r => r.Contains(o3) && r.Contains(o2)).Count() != 0)
            {
                result.Add(new List<T>(){o3, o2});
            }
        }
        return result; 
    }
    public static List<Line> GetTriangulationLines(List<Vector2> points)
    {
        var tris = Triangulate(points);
        var list = new HashSet<Line>();
        foreach (var tri in tris)
        {
            foreach (var line in tri.Edges)
            {
                list.Add(line);
            }
        }
        return list.ToList(); 
    }
    public static HashSet<Triangle> Triangulate(List<Vector2> points)
    {
        HashSet<Triangle> triangles = new HashSet<Triangle>();
        Triangle superTriangle = GetSuperTriangle(points);
        triangles.Add(superTriangle);

        foreach (var p in points)
        {
            HashSet<Triangle> badTris = new HashSet<Triangle>();
            foreach (var tri in triangles)
            {
                if(GeometryUtility.IsPointInCircumcircleOfTriangle(p, tri))
                {
                    badTris.Add(tri);
                }
            }

            HashSet<Line> polygon = new HashSet<Line>();

            foreach (var badTri in badTris)
            {
                foreach (var edge in badTri.Edges)
                {
                    var badTrisThatShareEdge = badTris.Where(t => t.Edges.Contains(edge));
                    if(badTrisThatShareEdge.Count() == 1) //1 because badTri itself always includes it
                    {
                        polygon.Add(edge);
                    }
                }
            }

            foreach(var badTri in badTris)
            {
                triangles.Remove(badTri);
            }

            foreach (var line in polygon)
            {
                Triangle newTri = new Triangle(line.Left, line.Right, p);
                triangles.Add(newTri);
            }
        }

        List<Triangle> trianglesToRemove = new List<Triangle>();

        foreach (var tri in triangles)
        {
            if( tri.Points.Contains(superTriangle.A) 
                || tri.Points.Contains(superTriangle.B) 
                || tri.Points.Contains(superTriangle.C))
            {
                trianglesToRemove.Add(tri);
            }
        }

        foreach (var tri in trianglesToRemove)
        {
            triangles.Remove(tri);
        }

        return triangles; 
    }
    private static Vector2 GetPointsCenter(List<Vector2> points)
    {
        Vector2 center = new Vector2(0f,0f);
        foreach (var p in points)
        {
            center += p;
        }
        center.x /= points.Count;
        center.y /= points.Count;
        return center; 
    }
    private static Triangle GetSuperTriangle(List<Vector2> points)
    {
        float maxX = 0f;
        float minX = Mathf.Inf;
        float maxY = 0f;
        float minY = Mathf.Inf;
        foreach (var p in points)
        {
            if(p.x > maxX) maxX = p.x;
            if(p.y > maxY) maxY = p.y;
            if(p.x < minX) minX = p.x;
            if(p.y < minY) minY = p.y;
        }
        Triangle super = new Triangle(new Vector2(-10f, -10f), new Vector2(maxX * 3f, -10f), new Vector2(-10f, maxY * 3f));
        return super;
    }
}


