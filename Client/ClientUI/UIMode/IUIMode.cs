using Godot;
using System;

public interface IUIMode 
{
    IMouseInputModule Mouse {get;}
    IKeyboardInputModule Key {get;}
    void Activate();
    void Deactivate();
    void HandleInput(InputEvent input);
    void HandleDeltaInput(float delta);
}
