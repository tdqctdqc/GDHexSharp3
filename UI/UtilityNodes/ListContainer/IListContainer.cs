using Godot;
using System;
using System.Collections;

public interface IListContainer
{
    IList List {get;}
    string GetString(int i);
    void Select(int i);
    void ClearSelection();
    object SelectedObject {get;}
}
