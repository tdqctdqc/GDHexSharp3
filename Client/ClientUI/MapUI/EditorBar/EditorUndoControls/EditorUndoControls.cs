using Godot;
using System;

public class EditorUndoControls : Node
{
    private FuncButton _undo, _redo;
    public override void _Ready()
    {
        _undo = GetNode<FuncButton>("HBoxContainer/Undo");
        _undo.Set(Undo);
        _redo = GetNode<FuncButton>("HBoxContainer/Redo");
        _redo.Set(Redo);
    }

    public void Undo()
    {
        Game.I.Session.Editor.Undo();
    }

    public void Redo()
    {
        Game.I.Session.Editor.Redo();
    }
}
