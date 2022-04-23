using Godot;
using System;

public class FuncTimer : Timer
{
    public Action Func { get; private set; }
    public override void _Ready()
    {
        Connect("timeout", this, nameof(Do));
    }
    public void Set(float time, Action func)
    {
        Func = func; 
        Start(time);
    }
    public void Do()
    {
        Func?.Invoke();
        Stop();
    }
}
