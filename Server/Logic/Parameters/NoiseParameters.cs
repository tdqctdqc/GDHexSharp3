using Godot;
using System;

public class NoiseParameters 
{
    public NoiseParameters(int octaves, float period, float persistence)
    {
        Octaves = octaves;
        Period = period;
        Persistence = persistence;
    }
    public int Octaves;
    public float Period;
    public float Persistence; 
}
