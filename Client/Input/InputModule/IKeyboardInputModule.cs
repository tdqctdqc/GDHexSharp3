using Godot;
using System;

public interface IKeyboardInputModule 
{
    void HandleDeltaInput(float delta);
    void HandleInput(InputEventKey input);
}
