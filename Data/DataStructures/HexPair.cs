using Godot;
using System;

public struct HexPair
{
    public int H1 { get; private set; }
    public int H2 { get; private set; }

    public HexPair(HexModel h1, HexModel h2)
    {
        if(h1.Coords.x > h2.Coords.x)
        {
            H1 = h1.ID; 
            H2 = h2.ID;
        }
        else if(h2.Coords.x > h1.Coords.x)
        {
            H2 = h1.ID; 
            H1 = h2.ID;
        }
        else if(h1.Coords.y > h2.Coords.y)
        {
            H1 = h1.ID; 
            H2 = h2.ID;
        }
        else
        {
            H2 = h1.ID; 
            H1 = h2.ID;
        }
    }
}
