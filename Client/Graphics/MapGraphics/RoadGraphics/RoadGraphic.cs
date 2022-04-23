using Godot;
using System;

public class RoadGraphic : MeshInstance2D
{
    public void Setup(RoadModel model)
    {
        if(model == null)
        {
            Clear();
            return;
        }
        var mesh = new QuadMesh();
        mesh.Size = new Vector2(Constants.HexRadius * 2f * .866f, 40f);
        Mesh = mesh;
        Texture = model.RoadType.Texture;
        Rotation = model.Angle;
    }
    public void Setup(RoadType type, float angle)
    {
        if(type == null)
        {
            Clear();
            return;
        }
        var mesh = new QuadMesh();
        mesh.Size = new Vector2(Constants.HexRadius * 2f * .866f, 40f);
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
