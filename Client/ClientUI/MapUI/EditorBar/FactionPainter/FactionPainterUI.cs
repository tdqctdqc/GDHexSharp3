using Godot;
using System;

public class FactionPainterUI : Control, IPainterUI
{
    private Label _selected; 
    private CheckBox _active; 
    private ListMenuButton _menu;
    private ListContainer<FactionModel> _list;
    private Action<IPainterUI> _activate; 
    public override void _Ready()
    {
        _selected = GetNode<Label>("VBox/SelectedFaction");
        _active = GetNode<CheckBox>("VBox/Active");
        _active.Connect("toggled", this, nameof(ToggledActive));
        _menu = GetNode<ListMenuButton>("VBox/Menu");
    }
    public void Setup(Action<IPainterUI> activate)
    {
        _activate = activate; 
        Cache<FactionModel>.ModelsChanged += (f) => SetupList();
        Cache<FactionModel>.ModelsAdded += (f) => SetupList();
        Cache<FactionModel>.ModelsDeleted += (f) => SetupList();
        SetupList();
        CacheManager.LoadedState += SetupList;
    }
    public void SetupList()
    {
        var facs = Cache<FactionModel>.GetModels();
        Action<FactionModel> selectFaction = (f) => Game.I.Session.Editor.FactionBrush.Current = f;
        _list = new ListContainer<FactionModel>(facs, selectFaction);
        if(_list.List.Count > 0) _menu.Set(_list);
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

            editor.SetBrush(editor.FactionBrush);
        }
        else
        {
            if(editor.CurrentBrush == editor.FactionBrush) editor.SetBrush(null);
        }
    }
}
