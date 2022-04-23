using Godot;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;


public class ListContainer<T>: IListContainer where T: INamed
{
    public T Selected { get; private set; }
    public object SelectedObject => Selected; 
    public Action<T> SelectAction { get; set; }
    public List<T> _list { get; private set; }
    public IList List => _list;
    public T this[int i] => _list[i];
    public ListContainer(List<T> list, Action<T> select = null)
    {
        SelectAction = select;
        _list = new List<T>(list);
        Select(0);
    }
    public void SetSelectAction(Action<T> select)
    {
        SelectAction = select;
        Select(0);
    }
    public void Select(int i)
    {
        if(i > _list.Count - 1) return;
        Selected = _list[i];
        SelectAction?.Invoke(Selected);
    }

    public void Select(T t)
    {
        if(_list.Contains(t) == false) return; 
        Selected = t; 
        SelectAction?.Invoke(Selected);
    }
    public void ClearSelection()
    {
        Selected = default(T);
        SelectAction?.Invoke(Selected);
    }
    public string GetString(int i) => _list[i].Name;
}
