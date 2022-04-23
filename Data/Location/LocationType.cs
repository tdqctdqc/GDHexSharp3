using Godot;
using System;

public class LocationType
{
    public int ID {get; private set;}
    public string Name { get; private set; }
    public Texture Texture { get; private set; }
    public float ProdPoints { get; private set; }
    public float Recruits { get; private set; }
    public float SupplyProd { get; private set; }
    public LocationType(LocationTypeModel model)
    {
        ID = model.ID;
        Name = model.Name;
        Texture = (Texture)GD.Load("res://"+model.TexturePath);
        ProdPoints = model.ProdPoints;
        Recruits = model.Recruits;
        SupplyProd = model.SupplyProd;
    }
}
