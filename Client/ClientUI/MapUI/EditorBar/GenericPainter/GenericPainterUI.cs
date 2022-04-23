using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class GenericPainterUI : Node, IPainterUI 
{
    private IBrush _brush; 
    private Label _selected; 
    private CheckBox _active; 
    private ListMenuButton _menu;
    private IListContainer _list;
    private Action<IPainterUI> _activate; 

    public override void _Ready()
    {
        _selected = GetNode<Label>("VBox/Selected");
        _active = GetNode<CheckBox>("VBox/Active");
        _active.Connect("toggled", this, nameof(ToggledActive));
        _menu = GetNode<ListMenuButton>("VBox/Menu");
    }
    public void Setup<T>(   string typeName, 
                            Action<IPainterUI> activate, 
                            List<T> types, 
                            Action<T> selectType, 
                            Action selectNull,
                            IBrush brush) where T : INamed
    {
        _brush = brush; 
        _selected.Text = $"Selected {typeName}";
        _menu.Text = typeName;
        _activate = activate; 
        _list = new ListContainer<T>(types, selectType);
        string nameNull = $"Clear {typeName}";
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
            editor.SetBrush(_brush);
        }
        else
        {
            if(editor.CurrentBrush == _brush) editor.SetBrush(null);
        }
    }
}
