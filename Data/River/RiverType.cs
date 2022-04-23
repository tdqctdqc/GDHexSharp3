using Godot;
using System;

public class RiverType : INamed
{
    public int ID {get; private set;}
    public string Name { get; private set; }
    public int Width { get; private set; }
    public Texture Texture { get; private set; }
    public float BuildCost { get; private set; }
    public float AttackPenalty { get; private set; }
    public float MinFlow { get; private set; }
    public RiverType(RiverTypeModel model)
    {
        ID = model.ID;
        Name = model.Name;
        Width = model.Width; 
        Texture = (Texture)GD.Load("res://"+model.TexturePath);
        BuildCost = model.BuildCost;
        MinFlow = model.MinFlow;
        AttackPenalty = model.AttackPenalty;
    }
}
