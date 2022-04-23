using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public static class GeometryUtility
{
    public static Vector3 GetCircumcircleOfTriangle(Vector2 A, Vector2 B, Vector2 C)
    {
        float a = (B-A).AngleTo(C-A);
        float b = (A-B).AngleTo(C-B);
        float c = (B-C).AngleTo(A-C);


        float d = 2f * (  A.x*(B.y-C.y) +     B.x*(C.y-A.y) +     C.x*(A.y-B.y)   );
        float centerX = ( 
                            (Mathf.Pow(A.x,2) + Mathf.Pow(A.y,2)) * (B.y - C.y)
                            + (Mathf.Pow(B.x,2) + Mathf.Pow(B.y,2)) * (C.y - A.y)
                            + (Mathf.Pow(C.x,2) + Mathf.Pow(C.y,2)) * (A.y - B.y)
                        ) / d;

        float centerY = ( 
                            (Mathf.Pow(A.x,2) + Mathf.Pow(A.y,2)) * (C.x - B.x)
                            + (Mathf.Pow(B.x,2) + Mathf.Pow(B.y,2)) * (A.x - C.x)
                            + (Mathf.Pow(C.x,2) + Mathf.Pow(C.y,2)) * (B.x - A.x)
                        ) / d;
        
        
        
        
        Vector2 centerPoint = new Vector2(centerX, centerY);
        float radius = centerPoint.DistanceTo(A);
        return new Vector3(centerX, centerY, radius);
    }
    public static Vector3 GetCircumcircleOfTriangle(Triangle tri)
    {
        return GetCircumcircleOfTriangle(tri.A,tri.B,tri.C);
    }


    public static bool ArePointsInCircumcircleOfTriangle(List<Vector2> points, Triangle triangle)
    {
        return ArePointsInCircumcircleOfTriangle(points, triangle.A, triangle.B, triangle.C);
    }
    
    public static bool ArePointsInCircumcircleOfTriangle(List<Vector2> points, Vector2 A, Vector2 B, Vector2 C)
    {
        Vector3 circumcircle = GetCircumcircleOfTriangle(A,B,C);
        Vector2 center = new Vector2(circumcircle.x,circumcircle.y);
        float radius = circumcircle.z;
        foreach (var point in points)
        {
            if(point.DistanceTo(center) < radius)
            {
                return true; 
            }
        }
        return false; 
    }

    public static bool IsPointInCircumcircleOfTriangle(Vector2 point, Triangle tri)
    {
        Vector3 circumcircle = GetCircumcircleOfTriangle(tri);
        Vector2 center = new Vector2(circumcircle.x,circumcircle.y);
        float radius = circumcircle.z;
        if(point.DistanceTo(center) < radius)
        {
            return true; 
        }
        
        return false; 
    }

    public static bool DoLinesIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        Vector2 leftA;
        Vector2 rightA;
        if(a1.x < a2.x)
        {
            leftA = a1;
            rightA = a2;
        }
        else
        {
            leftA = a2;
            rightA = a1;
        }

        Vector2 leftB;
        Vector2 rightB;
        if(b1.x < b2.x)
        {
            leftB = b1;
            rightB = b2;
        }
        else
        {
            leftB = b2;
            rightB = b1;
        }

        float minX = Mathf.Max(leftB.x, leftA.x);
        float maxX = Mathf.Min(rightB.x, rightA.x);

        float aSlope = (rightA.y - leftA.y) / (rightA.x - leftA.x);
        float bSlope = (rightB.y - leftB.y) / (rightB.x - leftB.x);
        float aIntercept = leftA.y - aSlope * leftA.x;
        float bIntercept = leftB.y - bSlope * leftB.x;


        float interceptX = (bIntercept - aIntercept) / (aSlope - bSlope);

        if(interceptX > minX && interceptX < maxX)
        {
            return true;
        }
        return false; 
        
    }
}

public struct Triangle 
{
    public Vector2 A, B, C;
    public Line AB, BC, CA;
    public List<Line> Edges;
    public List<Vector2> Points;
    public Triangle(Vector2 a, Vector2 b, Vector2 c) 
    {  
        Vector2 avg = (a + b + c) / 3f;

        List<Vector2> input = new List<Vector2>(); 
        input.Add(a); 
        input.Add(b); 
        input.Add(c); 
        input.OrderBy(v => (v-avg).Angle());
        A=input[0]; B=input[1]; C=input[2];  
        Points = new List<Vector2>();
        Points.Add(A);
        Points.Add(B);
        Points.Add(C);
        Edges = new List<Line>();
        AB = new Line(A,B);
        Edges.Add(AB);
        BC = new Line(B,C);
        Edges.Add(BC);
        CA = new Line(C,A);
        Edges.Add(CA);
    }

    public bool Equals(Triangle tri) 
    {
        return tri.Points.Contains(A) && tri.Points.Contains(B) && tri.Points.Contains(C);
    }
}

public struct Line
{
    public Vector2 Left, Right, Top, Bottom;

    public Line(Vector2 p1, Vector2 p2)
    {
        if(p1.x < p2.x)
        {
            Left = p1; 
            Right = p2;
        }
        else
        {
            Right = p1;
            Left = p2;
        }

        if(p1.y < p2.y)
        {
            Bottom = p1; 
            Top = p2;
        }
        else
        {
            Top = p1;
            Bottom = p2;
        }
    }

    public bool Equals(Line l)
    {
        if(l.Right == this.Left || l.Right == this.Right) return true;
        return false; 
    }
}