using Godot;
using System;

public class FuncControl : Control
{
    private bool _mouseOver; 
    private Control _child; 
    public Action Select; 
    public FuncControl(Action select, Control child)
    {
        Select = select; 
        _child = child; 
        AddChild(_child);

        MouseFilter = MouseFilterEnum.Stop;
        RectMinSize = _child.RectMinSize;
        FocusMode = FocusModeEnum.None; 
    }
    public override void _Ready()
    {
        _child.Connect("mouse_entered", this, nameof(MouseEnter));
        _child.Connect("mouse_exited", this, nameof(MouseExit));
    }
    public override void _Process(float delta)
    {
        if(_mouseOver)
        {
            if(Input.IsActionJustReleased("leftClick"))
            {
                Select?.Invoke();
            }
        }
    }

    public void MouseEnter()
    {
        _mouseOver = true; 
    }
    public void MouseExit()
    {
        _mouseOver = false; 
    }
}
