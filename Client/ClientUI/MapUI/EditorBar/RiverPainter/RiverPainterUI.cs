using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public class RiverPainterUI : Node, IPainterUI
{
    private Label _selected; 
    private CheckBox _active; 
    private ListMenuButton _menu;
    private ListContainer<RiverType> _list;
    private Action<IPainterUI> _activate; 

    public override void _Ready()
    {
        _selected = GetNode<Label>("VBox/SelectedRiver");
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
        var rivers = Game.I.Session.Data.RiverTypes.RiverTypes.Values.ToList();
        Action<RiverType> selectRiver = (r) => Game.I.Session.Editor.RiverBrush.Current = r;
        _list = new ListContainer<RiverType>(rivers, selectRiver);
        Action selectNull = () => Game.I.Session.Editor.RiverBrush.Current = null;
        string nameNull = "Clear River";
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

            editor.SetBrush(editor.RiverBrush);
        }
        else
        {
            if(editor.CurrentBrush == editor.RiverBrush) editor.SetBrush(null);
        }
    }
}
