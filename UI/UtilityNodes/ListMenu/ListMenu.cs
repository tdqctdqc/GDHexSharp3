using Godot;
using System;
using System.Collections.Generic;

public class ListMenu : PopupMenu
{
	private IListContainer _list;
	private Action<int> _select; 
	private Func<int, string> _getEntryName; 
	private List<string> _extraNames;
    private List<Action> _extraFuncs; 
	public override void _Ready()
	{
		
	}
	public void Set(IListContainer list, List<string> extraNames = null, List<Action> extraFuncs = null)
	{
		_list = list;
		if(GetSignalConnectionList("index_pressed").Count > 0) Disconnect("index_pressed", this, nameof(Select));
		Clear();
		_extraNames = extraNames;
        _extraFuncs = extraFuncs;
		_select = list.Select; 
		_getEntryName = list.GetString;
		SetupMenu(list.List.Count, _getEntryName);
		Connect("index_pressed", this, nameof(Select));
	}
	private void SetupMenu(int size, Func<int, string> getString)
	{
		for (int i = 0; i < size; i++)
		{
			string entry = getString(i);
			AddItem(entry);
		}
		if(_extraNames != null)
        {
            for (int i = 0; i < _extraNames.Count; i++)
            {
                string entry = _extraNames[i];
                AddItem(entry);
            }
        }
	}
	private void Select(int i)
	{
		if(i < _list.List.Count)
        {
			_select(i);
		}
		else
        {
            int extraIndex = i - _list.List.Count;
            _extraFuncs[extraIndex].Invoke();
        }
	}
}
