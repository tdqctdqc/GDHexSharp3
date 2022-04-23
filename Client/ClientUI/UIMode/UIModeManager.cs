using Godot;
using System;

public class UIModeManager 
{
    private IUIMode _mode; 
    public void SetUIMode(IUIMode mode)
    {
        if(_mode != null) _mode.Deactivate();
        _mode = mode; 
        _mode.Activate(); 
    }
    public void HandleInput(InputEvent input)
    {
        _mode?.HandleInput(input);
    }

    public void DeltaInput(float delta)
    {
        _mode?.HandleDeltaInput(delta);
    }
}
