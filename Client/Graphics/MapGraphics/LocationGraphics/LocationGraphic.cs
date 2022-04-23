using Godot;
using System;

public class LocationGraphic : MeshInstance2D
{
    public void Setup(LocationModel model)
    {
        var mesh = new QuadMesh();
        mesh.Size = new Vector2(Constants.HexRadius, Constants.HexRadius);
        Mesh = mesh;
        Texture = model.LocationType.Texture;
    }
    public void Setup(LocationType type)
    {
        var mesh = new QuadMesh();
        mesh.Size = new Vector2(Constants.HexRadius, Constants.HexRadius);
        Mesh = mesh;
        Texture = type.Texture;
    }
    public void Clear()
    {
        Texture = null;
        Mesh = null;
    }
}
