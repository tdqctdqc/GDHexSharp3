using Godot;
using System;

public class UIUnitHolderControl : Node
{
    private UIUnitHolder _holder; 
    private FuncButton _add, _remove, _edit;
    private CreateUnitWindow _create;
    public override void _Ready()
    {
        _add = GetNode<FuncButton>("VBox/Add");
        _remove = GetNode<FuncButton>("VBox/Remove");
        _edit = GetNode<FuncButton>("VBox/Edit");
    }
    public void Setup(Action add, Action edit, Action remove)
    {
        _add.Set(add);
        _edit.Set(edit);
        _remove.Set(remove);
    }
}
