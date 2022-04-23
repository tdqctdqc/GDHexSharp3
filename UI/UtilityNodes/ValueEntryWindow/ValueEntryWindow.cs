using Godot;
using System;
using System.Collections.Generic;

public class ValueEntryWindow : WindowDialog
{
    public Dictionary<string, StringEntry> Strings; 
    public Dictionary<string, NumEntry> Nums; 
    public Dictionary<string, IListContainer> Lists; 
    public Dictionary<string, ColorEntry> Colors; 
    private VBoxContainer _vBox;
    private Action _confirm; 
    private FuncButton _button;
    public override void _Ready()
    {
        _vBox = GetNode<VBoxContainer>("VBox");
        _button = GetNode<FuncButton>("VBox/Confirm");
        _button.Set(Confirm);
    }

    public void Setup(Action confirm, List<string> stringKeys = null,  List<string> numKeys = null, List<string> listKeys = null, List<IListContainer> lists = null, List<string> colorKeys = null)
    {
        Clear();
        _confirm = confirm;
        int itemCount = 0;
        Strings = new Dictionary<string, StringEntry>();
        Nums = new Dictionary<string, NumEntry>();
        Lists = new Dictionary<string, IListContainer>();
        Colors = new Dictionary<string, ColorEntry>();
        if(stringKeys != null)
        {
            foreach (var stringKey in stringKeys)
            {
                var entry = Scenes.StringEntry;
                entry.RectMinSize = entry.RectSize; 
                _vBox.AddChild(entry);
                entry.Setup(stringKey);
                Strings.Add(stringKey, entry);
                itemCount++;
            }
        }
        if(numKeys != null)
        {
            foreach (var numKey in numKeys)
            {
                var entry = Scenes.NumEntry;
                entry.RectMinSize = entry.RectSize; 

                _vBox.AddChild(entry);
                entry.Setup(numKey);
                Nums.Add(numKey, entry);
                itemCount++;
            }
        }
        if(lists != null)
        {
            for(int i = 0; i < lists.Count; i++)
            {
                var list = lists[i];
                string listKey = listKeys[i];
                var menu = new ListMenuButton();
                menu.RectMinSize = menu.RectSize; 

                _vBox.AddChild(menu);
                menu.Set(list);
                Lists.Add(listKey, list);
                itemCount++;
            }
        }
        if(colorKeys != null)
        {
            for (int i = 0; i < colorKeys.Count; i++)
            {
                var entry = Scenes.ColorEntry;
                _vBox.AddChild(entry);
                entry.Setup(colorKeys[i]);
                Colors.Add(colorKeys[i], entry);
                itemCount++;
            }
        }

        _vBox.RemoveChild(_button);
        _vBox.AddChild(_button);

        RectSize = new Vector2(RectSize.x, itemCount * 50f + 25f);
    }

    private void Clear()
    {
        var children = _vBox.GetChildren();
        foreach (var child in children)
        {
            if(child != _button) ((Node)child).Free();
        }
    }

    public void Confirm()
    {
        _confirm();
        Visible = false; 
    }

    public string GetStringResult(string key)
    {
        if(Strings.ContainsKey(key) == false) return "";
        else return Strings[key].GetValue();
    }

    public float GetNumResult(string key)
    {
        if(Nums.ContainsKey(key) == false) return float.NaN;
        else return Nums[key].GetFloatValue();
    }

    public object GetListResult(string key)
    {
        if(Lists.ContainsKey(key) == false) return null;
        else return Lists[key].SelectedObject;
    }

    public object GetColorResult(string key)
    {
        if(Colors.ContainsKey(key) == false) return null;
        else return Colors[key].GetValue();
    }
}
