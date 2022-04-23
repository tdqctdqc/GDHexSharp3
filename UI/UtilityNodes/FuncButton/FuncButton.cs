using Godot;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
[Tool]
public class FuncButton : Button
{
    public Action Action { get; private set; }
    private List<Action> _actions; 
    public override void _Ready()
    {
        Connect("button_up", this, nameof(Do));
        FocusMode = FocusModeEnum.None; 
        _actions = new List<Action>();
    }
    public void Set(Action action)
    {
        _actions = new List<Action>();
        _actions.Add(action); 
    }

    public void Add(Action action)
    {
        if(_actions == null) _actions = new List<Action>();
        _actions.Add(action); 
    }
    public void Do()
    {
        foreach (var action in _actions)
        {
            action?.Invoke();
        }
    }
}
