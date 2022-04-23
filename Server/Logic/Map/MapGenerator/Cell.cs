using Godot;
using System;
using HexWargame;
using System.Collections.Generic;

public class Cell : IGeologyNode, IGraphNodeAggregation<Cell, PreHex>
{
    public int ID { get; private set; }
    public Vector2 WorldPos { get; private set; }
    public PreHex HomeHex { get; private set; }
    public int HomeHexPairID { get; private set; }
    public List<PreHex> Children { get; private set; }
    public List<int> HexIDs { get; private set; }
    public List<HexModel> HexModels => Cache<HexModel>.GetModels(HexIDs);
    public HashSet<PreHex> BorderingHexes { get; private set; }
    public List<Cell> Neighbors { get; private set; }
    public int FactionID { get; set; }
    public Plate Plate { get; set; }
    public Color Color { get; private set; }
    public Color SelectColor { get; private set; }
    public bool Land => Plate.Land;
    public float Temperature { get; set; }
    public float Moisture { get; set; }
    public List<Cell> PathToWater { get; set; }
    public Cell(int id, PreHex hex)
    {
        HomeHex = hex;
        var n = hex.Neighbors[0];
        HomeHexPairID = hex.GetHexPairIndex(n);
        Temperature = 0f;
        Moisture = 0f; 
        WorldPos = hex.WorldPos;
        ID = id;
        Children = new List<PreHex>();
        HexIDs = new List<int>();
        BorderingHexes = new HashSet<PreHex>();
        Neighbors = new List<Cell>();
        AddHex(hex);
    }
    public void AddHex(PreHex hex)
    {
        if(Children.Contains(hex) == false)
        {
            HexIDs.Add(hex.ID);
            hex.Cell = this; 
            Children.Add(hex);
            BorderingHexes.Remove(hex);
            foreach (var n in hex.Neighbors)
            {
                if(Children.Contains(n) == false)
                {
                    BorderingHexes.Add(n);
                }
            }
        }
    }
    public void AddNeighbor(Cell cell)
    {
        if(Neighbors.Contains(cell) == false && cell != this)
        {
            Neighbors.Add(cell);
        }
    }

    public void GenerateColor()
    {
        float small1 = Game.I.Random.RandfRange(.1f, .4f);
        float small2 = Game.I.Random.RandfRange(.1f, .4f);
        float big = Game.I.Random.RandfRange(.5f, .8f);
        if(Plate.Continent.Land) Color = new Color(small1, big, small1);
        else Color = new Color(small1, small2, big);

        SelectColor = new Color(big, small1, small2);
    }
}
