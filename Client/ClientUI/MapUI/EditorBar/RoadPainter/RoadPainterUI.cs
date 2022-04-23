using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class RoadPainterUI : Node, IPainterUI
{
    private Label _selected; 
    private CheckBox _active; 
    private ListMenuButton _menu;
    private ListContainer<RoadType> _list;
    private Action<IPainterUI> _activate; 

    public override void _Ready()
    {
        _selected = GetNode<Label>("VBox/SelectedRoad");
        _active = GetNode<CheckBox>("VBox/Active");
        _active.Connect("toggled", this, nameof(ToggledActive));
        _menu = GetNode<ListMenuButton>("VBox/Menu");
    }
    public void Setup(Action<IPainterUI> activate)
    {
        _activate = activate; 

        SetupList();
        CacheManager.LoadedState += SetupList;
    }
    public void SetupList()
    {
        var roads = Game.I.Session.Data.RoadTypes.RoadTypes.Values.ToList();
        Action<RoadType> selectRoad = (r) => Game.I.Session.Editor.RoadBrush.Current = r;
        _list = new ListContainer<RoadType>(roads, selectRoad);
        Action selectNull = () => Game.I.Session.Editor.RoadBrush.Current = null;
        string nameNull = "Clear Road";
        if(_list.List.Count > 0) _menu.Set(_list, new List<string>(){nameNull}, new List<Action>(){selectNull} );
    }
    public void Deactivate()
    {
        _active.Pressed = false; 
        ToggledActive(false);
    }
    public void ToggledActive(bool active)
    {
        var editor = Game.I.Session.Editor;

        if(active)
        {
            _activate(this);
            editor.SetBrush(editor.RoadBrush);
        }
        else
        {
            if(editor.CurrentBrush == editor.RoadBrush) editor.SetBrush(null);
        }
    }
}
