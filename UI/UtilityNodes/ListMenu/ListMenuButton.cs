using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class ListMenuButton : MenuButton
{
    private IListContainer _list;
    private Action<int> _select; 
    private Func<int, string> _getEntryName; 
    private List<string> _extraNames;
    private List<Action> _extraFuncs; 
    public void Set(IListContainer list, List<string> extraNames = null, List<Action> extraFuncs = null)
    {
        var popup = GetPopup();
        _list = list; 
        if(popup.GetSignalConnectionList("index_pressed").Count > 0) popup.Disconnect("index_pressed", this, nameof(Select));
        GetPopup().Clear();
        _extraNames = extraNames;
        _extraFuncs = extraFuncs;
        _select = list.Select; 
        _getEntryName = list.GetString;
        SetupMenu(list.List.Count, _getEntryName);
        Select(0);
        GetPopup().Connect("index_pressed", this, nameof(Select));
    }
    private void SetupMenu(int size, Func<int, string> getString)
    {
        var popup = GetPopup();
        for (int i = 0; i < size; i++)
        {
            string entry = getString(i);
            popup.AddItem(entry);
        }
        if(_extraNames != null)
        {
            for (int i = 0; i < _extraNames.Count; i++)
            {
                string entry = _extraNames[i];
                popup.AddItem(entry);
            }
        }
    }
    private void Select(int i)
    {
        if(i < _list.List.Count)
        {
            Text = _getEntryName(i);
            _select(i);
        }
        else
        {
            _list.ClearSelection();
            int extraIndex = i - _list.List.Count;
            Text = _extraNames[extraIndex];
            _extraFuncs[extraIndex].Invoke();
        }
    }
}
