using Godot;
using System;

public class HexPathFinderNode 
{
    public HexModel Hex { get; set; }
    public HexPathFinderNode Parent { get; set; }
    public HexPathFinderNode(HexModel hex)
    {
        Hex = hex; 
    }
}
