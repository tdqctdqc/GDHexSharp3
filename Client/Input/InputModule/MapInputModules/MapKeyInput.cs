using Godot;
using System;

public class MapKeyInput : IKeyboardInputModule
{
    public void HandleDeltaInput(float delta)
    {
        var camera = Game.I.Session.Client.Graphics.Camera;
        float mult = 1f;
        if(Input.IsKeyPressed((int)KeyList.Shift)) mult = 3f;
        if(Input.IsActionPressed("mapLeft")) camera.CameraMove(delta * mult, Vector2.Left);
        if(Input.IsActionPressed("mapRight")) camera.CameraMove(delta * mult, Vector2.Right);
        if(Input.IsActionPressed("mapUp")) camera.CameraMove(delta * mult, Vector2.Up);
        if(Input.IsActionPressed("mapDown")) camera.CameraMove(delta * mult, Vector2.Down);

    }

    public void HandleInput(InputEventKey input)
    {
        if(Input.IsActionPressed("doClientCommand"))
        {
            Game.I.Session.Client.Command.DoCommand();
        }
        if(Input.IsActionJustPressed("escapeClientCommand"))
        {
            Game.I.Session.Client.Command.CancelCommand();
        }
    }
}
