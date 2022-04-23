using Godot;
using System;
using System.Text;

public class Utility : IUtility
{
    public PathFinder PathFinder { get; private set; }
    public StringBuilder StringBuilder { get; private set; }
    public Utility()
    {
        PathFinder = new PathFinder();
        StringBuilder = new StringBuilder();
        GraphicsUtility.Setup();
    }
}
