using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

public class PathMesh : MeshInstance2D
{
    private CancellableTask _taskCancel; 
    public override void _Ready()
    {
        var mesh = new QuadMesh();
        mesh.Size = new Vector2(Constants.HexRadius * 2f * .866f, 40f);
        _taskCancel = new CancellableTask();
    }
    public void Setup(Color color)
    {
        Modulate = color; 
    }
    public void DrawPathsArrow(List<List<HexModel>> paths)
    {
        Action drawAction = () => DoDrawPathsArrow(paths);
        _taskCancel.SetTask(drawAction);
    }
    public void DrawPathsDef(List<List<HexModel>> paths)
    {
        Action drawAction = () => DoDrawPathsDef(paths);
        _taskCancel.SetTask(drawAction);
    }
    private void DoDrawPathsArrow(List<List<HexModel>> paths)
    {
        var points = new Godot.Collections.Array();
        
        foreach (var path in paths)
        {
            var pathPoints = GraphicsUtility.GetPathArrowMeshPoints(path, 10f);
            foreach (var p in pathPoints)
            {
                points.Add(p);
            }
        }
        ArrayMesh arrayMesh;

        if(points.Count == 0)
        {
            arrayMesh = GetNullMesh();
            Mesh = arrayMesh; 
        }
        else 
        {
            arrayMesh = new ArrayMesh();
            var arrays = new Godot.Collections.Array();
            arrays.Resize(9);
            arrays[0] = points;
            arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
            Mesh = arrayMesh; 
        }
    }
    private void DoDrawPathsDef(List<List<HexModel>> paths)
    {
        var points = new Godot.Collections.Array();
        
        foreach (var path in paths)
        {
            var pathPoints = GraphicsUtility.GetDefPathMeshPoints(path, 10f);
            foreach (var p in pathPoints)
            {
                points.Add(p);
            }
        }
        ArrayMesh arrayMesh;

        if(points.Count == 0)
        {
            arrayMesh = GetNullMesh();
            Mesh = arrayMesh; 
        }
        else 
        {
            arrayMesh = new ArrayMesh();
            var arrays = new Godot.Collections.Array();
            arrays.Resize(9);
            arrays[0] = points;
            arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
            Mesh = arrayMesh; 
        }
    }
    public void Clear()
    {
        _taskCancel?.Cancel();
        Mesh = GetNullMesh();
    }

    private ArrayMesh GetNullMesh()
    {
        var arrayMesh = new ArrayMesh();
        var arrays = new Godot.Collections.Array();
        arrays.Resize(9);
        var points = new Godot.Collections.Array();
        points.Add(Vector2.Zero);
        points.Add(Vector2.Zero);
        points.Add(Vector2.Zero);
        arrays[0] = points; 
        arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
        return arrayMesh; 
    }
}
