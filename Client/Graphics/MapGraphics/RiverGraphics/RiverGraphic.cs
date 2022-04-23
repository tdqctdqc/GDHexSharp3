using Godot;
using System;

public class RiverGraphic : MeshInstance2D
{
    private static float _riverPixelsPerWidth = 10f;
    private static float _riverMinPixels = 5f;
    public void Setup(RiverModel model)
    {
        if(model == null) { Clear(); return; }
        var mesh = new QuadMesh();
        float width = _riverMinPixels + (float)model.RiverType.Width * _riverPixelsPerWidth;
        mesh.Size = new Vector2(Constants.HexRadius, width);
        Mesh = mesh;
        Texture = model.RiverType.Texture;
        Rotation = model.Angle;
    }
    public void Setup(RiverType type, float angle)
    {
        var mesh = new QuadMesh();
        float width = _riverMinPixels + (float)type.Width * _riverPixelsPerWidth;
        mesh.Size = new Vector2(Constants.HexRadius, width);
        Mesh = mesh;
        Texture = type.Texture;
        Rotation = angle;
    }
    public void Clear()
    {
        Texture = null;
        Mesh = null;
    }
}
