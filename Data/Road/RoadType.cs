using Godot;
using System;

public class RoadType : INamed
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public Texture Texture { get; private set; }
    public float BaseBuildCost { get; private set; }
    public int Quality { get; private set; }
    public RoadType(RoadTypeModel model)
    {
        ID = model.ID;
        Name = model.Name;
        Texture = (Texture)GD.Load("res://"+model.TexturePath);
        BaseBuildCost = model.BaseBuildCost;
        Quality = model.Quality;
    }
}
