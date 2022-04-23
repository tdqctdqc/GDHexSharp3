using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HexWargame
{
public static class VectorExt 
{
    public static float[] ToArray(this Vector2 v)
    {
        return new float[]{v.x, v.y};
    }
    public static Vector2 GetLineFacingVector(this List<HexModel> line)
    {
        if(line == null || line.Count == 0) return Vector2.Zero;
        Vector2 threatDir = (line[0].WorldPos - line.Last().WorldPos).Rotated(Mathf.Pi / 2f);
        return threatDir; 
    }
}
}
