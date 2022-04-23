using Godot;
using System;

public class ColorEntry : HBoxContainer
{
    private Label _label;
    private ColorPickerButton _entry;
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _entry = GetNode<ColorPickerButton>("ColorPickerButton");
    }
    public void Setup(string key)
    {
        _label.Text = key;
    }

    public Color GetValue()
    {
        return _entry.Color;
    }
}
