using Godot;
using System;

public class AppStateController
{
    public IAppState Current {get; private set;}
    public void SetState(IAppState state)
    {
        var old = Current; 
        Current = state;
        Current.Enter();
        if(old != null) old.Exit();
    }
}
