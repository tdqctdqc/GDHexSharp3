using Godot;
using System;
using System.Collections.Generic;
using HexWargame;
public class PreHex : IGraphNode<PreHex>
{
    public int ID { get; set; }
    public Vector2 WorldPos => Coords.GetWorldPosFromOffset();
    public Vector2 Coords { get; set; }
    public Vector3 CubeCoords { get; set; }
    public List<PreHex> Neighbors { get; set; }
    public Cell Cell { get; set; }
    public int FactionID { get; set; }
    public int TerrainID { get; set; }
    public float Roughness { get; set; }
    public float Moisture { get; set; }
    public PreHex()
    {
        Roughness = 0f;
        Moisture = 0f;
    }
}
