using Godot;
using System;
using System.Collections.Generic;

public class HexMesh : ArrayMesh
{
    public HexMesh()
    {
        Godot.Collections.Array vertices = GetVertices();
        var arrays = new Godot.Collections.Array();
        arrays.Resize(9);
        arrays[0] = vertices;
        AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
    }
    private Godot.Collections.Array GetVertices()
    {
        var radius = Constants.HexRadius;
        var vertices = new Godot.Collections.Array();
        var ne = radius * new Vector2(.5f, -.866f);
        var e = radius * new Vector2(1f, 0f);
        var se = radius * new Vector2(.5f, .866f);
        var sw = radius * new Vector2(-.5f, .866f);
        var w = radius * new Vector2(-1f, 0f);
        var nw = radius * new Vector2(-.5f, -.866f);

        vertices.Add(ne);
        vertices.Add(e);
        vertices.Add(se);

        vertices.Add(ne);
        vertices.Add(se);
        vertices.Add(sw);

        vertices.Add(ne);
        vertices.Add(sw);
        vertices.Add(w);

        vertices.Add(ne);
        vertices.Add(w);
        vertices.Add(nw);

        return vertices;
    }
}
