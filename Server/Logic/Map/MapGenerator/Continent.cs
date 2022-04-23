using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Continent : IGeologyNode, IGraphNodeAggregation<Continent, Plate>
{
    public Vector2 WorldPos { get; private set; }
    public int ID { get; private set; }
    public bool Land { get; private set; }
    public List<Plate> Children { get; private set; }
    public HashSet<Plate> BorderingPlates { get; private set; }
    public List<HexModel> HexModels => Children.SelectMany(p => p.Children.SelectMany(c => c.HexModels)).ToList();
    public List<Continent> Neighbors => GetNeighbors();
    public Color Color { get; private set; }
    public Color SelectColor { get; private set; }
    public Vector2 Drift { get; private set; }


    public Continent(int id, Plate plate, bool land)
    {
        Drift = new Vector2(Game.I.Random.RandfRange(-1f, 1f), Game.I.Random.RandfRange(-1f, 1f));

        WorldPos = plate.WorldPos;
        Land = land; 
        ID = id;
        Children = new List<Plate>();
        BorderingPlates = new HashSet<Plate>();
        AddPlate(plate);
    }
    public void AddPlate(Plate plate)
    {
        if(plate.Continent != null)
        {
            plate.Continent.RemovePlate(plate);
        }
        if(Children.Contains(plate) == false)
        {
            plate.Continent = this; 
            Children.Add(plate);
            BorderingPlates.Remove(plate);
            foreach (var n in plate.Neighbors)
            {
                if(Children.Contains(n) == false)
                {
                    BorderingPlates.Add(n);
                }
            }
        }
    }
    public void RemovePlate(Plate plate)
    {
        if(Children.Contains(plate))
        {
            plate.Continent = null; 
            Children.Remove(plate);
            BorderingPlates.Add(plate);
            foreach (var n in plate.Neighbors)
            {
                if(Children.Contains(n))
                {
                    BorderingPlates.Add(n);
                }
            }
        }
    }
    private List<Continent> GetNeighbors()
    {
        var result = Children.SelectMany(c => c.Neighbors).Select(c => c.Continent).ToHashSet();
        result.Remove(this);
        return result.ToList();
    }
    public void GenerateColor()
    {
        float small1 = Game.I.Random.RandfRange(.05f, .2f);
        float small2 = Game.I.Random.RandfRange(.05f, .2f);
        float big = Game.I.Random.RandfRange(.5f, 1f);
        if(Land) Color = new Color(small1, big, small1);
        else Color = new Color(small1, small2, big);

        SelectColor = new Color(big, small1, small2);

    }
}
