using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using HexWargame;

public class PathMultiMesh : MultiMeshInstance2D
{
    private Task _task;
    private CancellationTokenSource _token;
    public override void _Ready()
    {
        var mesh = new QuadMesh();
        mesh.Size = new Vector2(Constants.HexRadius * 2f * .866f, 40f);
        Multimesh = new MultiMesh();
        Multimesh.Mesh = mesh; 
    }
    public void Setup(Color color)
    {
        Modulate = color; 
    }
    public void DrawPaths(List<List<HexModel>> paths)
    {
        if(_task != null)
        {
            if(_task.IsCompleted == false)
            {
                _token.Cancel();
                Clear();
            }
        }
        Action drawAction = () => DoDrawPaths(paths);
        _token = new CancellationTokenSource();
        _task = new Task(drawAction, _token.Token); 
        _task.Start();
    }
    private void DoDrawPaths(List<List<HexModel>> paths)
    {
        Multimesh.InstanceCount = paths.Sum(p => p.Count - 1);
        int iter = 0;
        foreach (var path in paths)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                HexModel from = path[i];
                HexModel to = path[i + 1];
                float angle = from.GetHexAngle(to);
                Vector2 pos = (from.WorldPos + to.WorldPos) / 2f;
                Multimesh.SetInstanceTransform2d(iter, new Transform2D(angle, pos));
                iter++;
            }
        }
    }
    public void Clear()
    {
        Multimesh.InstanceCount = 0;
        _token?.Cancel();
    }
}
