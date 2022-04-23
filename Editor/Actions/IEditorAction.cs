using Godot;
using System;

public interface IEditorAction 
{
    void DoAction();
    IEditorAction GetUndoAction();
}
