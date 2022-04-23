using Godot;
using System;
using System.Collections.Generic;

public class Stopwatch
{
    public float TimeElapsed => GetTimeElapsed();
    private DateTime _start;
    public Stopwatch()
    {
        _start = DateTime.Now;
    }
    public void Tick(float delta)
    {
    }
    private float GetTimeElapsed()
    {
        var time = DateTime.Now.Subtract(_start);
        return time.Seconds + (time.Milliseconds * .001f);
    }
    
}
