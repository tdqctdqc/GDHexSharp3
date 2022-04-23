using Godot;
using System;

public class StringEntry : HBoxContainer
{
    private Label _label;
    private LineEdit _entry;
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _entry = GetNode<LineEdit>("LineEdit");
    }

    public void Setup(string key)
    {
        _label.Text = key;
    }

    public string GetValue()
    {
        return _entry.Text;
    }
}
