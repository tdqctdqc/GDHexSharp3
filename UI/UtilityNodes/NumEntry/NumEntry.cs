using Godot;
using System;

public class NumEntry : HBoxContainer
{
    private Label _label;
    private SpinBox _entry;
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _entry = GetNode<SpinBox>("SpinBox");
    }
    public void Setup(string key)
    {
        _label.Text = key;
    }

    public float GetFloatValue()
    {
        return _entry.GetLineEdit().Text.ToFloat();
    }
    public int GetIntValue()
    {
        return Mathf.CeilToInt(_entry.GetLineEdit().Text.ToFloat());
    }
}
