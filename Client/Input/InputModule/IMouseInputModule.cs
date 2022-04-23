using Godot;
using System;

public interface IMouseInputModule 
{
    void HandleDeltaInput(float delta);
    void HandleInput(InputEventMouse input);
    void Activate();
    void Deactivate();
}
