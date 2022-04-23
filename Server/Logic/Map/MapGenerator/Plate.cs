using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Plate : IGeologyNode, IGraphNodeAggregation<Plate, Cell>
{
    public Continent Continent { get; set; }
    public Vector2 WorldPos { get; private set; }
    public int ID { get; private set; }
    public bool Land => Continent.Land; 
    public List<Cell> Children { get; private set; }
    public HashSet<Cell> BorderingCells { get; private set; }
    public List<HexModel> HexModels => Children.SelectMany(c => c.HexModels).ToList();
    public Color Color { get; private set; }
    public Color SelectColor { get; private set; }
    public Vector2 Drift { get; private set; }
    public List<Plate> Neighbors => GetNeighbors();
    public float Temperature { get; set; }
    public Plate(int id, Cell cell)
    {
        Temperature = 0f;
        Drift = new Vector2(Game.I.Random.RandfRange(-1f, 1f), Game.I.Random.RandfRange(-1f, 1f));
        Continent = null;
        WorldPos = cell.WorldPos;
        ID = id;
        Children = new List<Cell>();
        BorderingCells = new HashSet<Cell>();
        AddCell(cell);
    }
    public void AddCell(Cell cell)
    {
        if(Children.Contains(cell) == false)
        {
            cell.Plate = this; 
            Children.Add(cell);
            BorderingCells.Remove(cell);
            foreach (var n in cell.Neighbors)
            {
                if(Children.Contains(n) == false)
                {
                    BorderingCells.Add(n);
                }
            }
        }
    }
    private List<Plate> GetNeighbors()
    {
        var result = Children.SelectMany(c => c.Neighbors).Select(c => c.Plate).ToHashSet();
        result.Remove(this);
        return result.ToList();
    }

    public void GenerateColor()
    {
        float small1 = Game.I.Random.RandfRange(.05f, .2f);
        float small2 = Game.I.Random.RandfRange(.05f, .2f);
        float big = Game.I.Random.RandfRange(.5f, 1f);
        if(Continent.Land) Color = new Color(small1, big, small1);
        else Color = new Color(small1, small2, big);

        SelectColor = new Color(big, small1, small2);

    }
}
