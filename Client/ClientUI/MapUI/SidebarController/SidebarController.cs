using Godot;
using System;

public class SidebarController : Node
{
    private UIBarController _controller; 

    private FuncButton _editor, _units; 
    public override void _Ready()
    {
        _editor = GetNode<FuncButton>("HBox/Editor");
        _units = GetNode<FuncButton>("HBox/Units");

        _controller = new UIBarController();
    }

    public void Setup()
    {
        _controller.AddPanel(_editor, Game.I.Session.Client.UI.MapUI.EditorBar);
        _controller.AddPanel(_units, Game.I.Session.Client.UI.MapUI.UnitBar);
    }
}
