using Godot;
using System;

public class BrushControls : Control
{
    private HSlider _brushSize; 
    private Label _brushSizeLabel; 
    private CheckBox _fillMode; 
    public override void _Ready()
    {
        _brushSize = GetNode<HSlider>("VBoxContainer/BrushSize");
        _brushSizeLabel = GetNode<Label>("VBoxContainer/BrushSizeLabel");
        _fillMode = GetNode<CheckBox>("VBoxContainer/Fill/CheckBox");
    }

    public void Setup()
    {
        _brushSize.Connect("value_changed", this, nameof(SetBrushSize));
        _fillMode.Connect("toggled", this, nameof(SetFillMode));
    }
    public void SetFillMode(bool fill)
    {
        Game.I.Session.Editor.FillMode = fill; 
    }
    public void SetBrushSize(float size)
    {
        int brushSize = Mathf.CeilToInt(size);
        _brushSizeLabel.Text = $"Brush Size: {brushSize}";
        Game.I.Session.Editor.BrushRadius = brushSize - 1;
    }
}
