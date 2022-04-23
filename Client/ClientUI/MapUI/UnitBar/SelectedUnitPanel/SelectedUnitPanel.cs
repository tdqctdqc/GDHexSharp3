using Godot;
using System;

public class SelectedUnitPanel : Control
{
    private Label _order, _force, _readiness, _supply;
    private UIUnit _unit; 
    public override void _Ready()
    {
        _unit = GetNode<UIUnit>("UIUnit");
        _order = GetNode<Label>("VBox/Order");
        _force = GetNode<Label>("VBox/Force");
        _readiness = GetNode<Label>("VBox/Readiness");
        _supply = GetNode<Label>("VBox/Supply");
    }
    public void Setup(UnitModel unit)
    {
        _unit.Setup(unit);
        var order = unit.Order;
        _order.Text = order.Name; 
        _force.Text = $"Strength: {(int)(100f * unit.Strength)}%";
        _readiness.Text = $"Readiness: {(int)(100f * unit.Readiness)}%";
        _supply.Text = $"Supply: {(int)(100f * unit.Supply)}%";
    }
}
