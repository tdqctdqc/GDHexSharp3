using Godot;
using System;

public class CameraController : Camera2D
{
    const float Speed = 300f;
    const float ZoomSpeed = 0.05f;

    public override void _Ready()
    {
        Position = Vector2.Zero;
    }

    public void CameraMove(float delta, Vector2 dir)
    {
        Position = Position + dir * delta * Speed * Zoom;
        /*
        if(Position.x > Game.I.Session.State.Map.RealWidth) Position = new Vector2(Game.I.Session.State.Map.RealWidth, Position.y);
        if(Position.x < 0f) Position = new Vector2(0f, Position.y);
        if(Position.y > Game.I.Session.State.Map.RealHeight) Position = new Vector2(Position.x, Game.I.Session.State.Map.RealHeight);
        if(Position.y < 0f) Position = new Vector2(Position.x, 0f);
        */
    }


    public void ZoomIn()
    {
        Zoom *= (1f - ZoomSpeed);
    }

    public void ZoomOut()
    {
        Zoom *= (1f + ZoomSpeed);
    }
}
