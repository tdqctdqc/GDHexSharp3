using Godot;
using System;
using HexWargame;
public class EdgePathFinderNode 
{
    public int HexPairID { get; private set; }
    public Vector2 Position {get; private set;}
    public EdgePathFinderNode Parent { get; set; }
    public EdgePathFinderNode(int hexPairID)
    {
        HexPairID = hexPairID; 
        Position = HexPairID.GetPosForPairID();
    }
}
