using Godot;
using System;

public class Hint : Control
{
    private Label _currentCommand, _warning;
    public override void _Ready()
    {
        _currentCommand = GetNode<Label>("Hint");
        _warning = GetNode<Label>("Warning");
    }
    public void SetCurrentCommand(string comm)
    {
        _currentCommand.Text = comm;
    }
    public void SetWarning(string warn)
    {
        _warning.Text = warn;
    }
}
