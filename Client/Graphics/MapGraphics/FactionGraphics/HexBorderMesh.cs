using Godot;
using System;

public class HexBorderMesh : ArrayMesh
{
    private float _width; 
    public void Setup(float width)
    {
        _width = width; 
        Godot.Collections.Array vertices = GetVertices();
        var arrays = new Godot.Collections.Array();
        arrays.Resize(9);
        arrays[0] = vertices;
        AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
        CustomAabb = new AABB();
    }
    private Godot.Collections.Array GetVertices()
    {
        var wLong = (Mathf.Sqrt(5f)/2) * (_width); 
        var radius = 10f;//HexUtility.HexRadius;
        var vertices = new Godot.Collections.Array();
        var a = radius * new Vector2(.5f * radius, -.866f * radius);
        var b = a + new Vector2((wLong / 2f) * 1.04f, _width);
        var d = radius * new Vector2(-.5f * radius, -.866f * radius);
        var c = d + new Vector2(-(wLong / 2f) * 1.04f, _width);

        vertices.Add(a);
        vertices.Add(c);
        vertices.Add(d);

        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        
        return vertices;
    }
}
