using Godot;
using System;

public class Graphics : Node2D
{
    public MapGraphics MapGraphics { get; private set; }
    public CameraController Camera { get; private set; }
    public Graphics()
    {
        Camera = new CameraController();
        AddChild(Camera);
        MapGraphics = new MapGraphics();
        AddChild(MapGraphics);
    }
    public void Setup()
    {
        Camera.Current = true; 

        MapGraphics.Setup();
    }
}
