using Godot;
using System;
using System.Collections.Generic;

public class Timers : Node
{
    private Dictionary<string, Stopwatch> _timers; 
    public Timers()
    {
        _timers = new Dictionary<string, Stopwatch>();
    }
    public void SetTimer(string tag)
    {
        if(_timers.ContainsKey(tag)) _timers.Remove(tag);
        _timers.Add(tag, new Stopwatch());
    }
    public void CallTimer(string tag)
    {
        float time = _timers[tag].TimeElapsed;
        GD.Print($"{tag} time was {time} seconds");
        _timers.Remove(tag);
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        foreach (var item in _timers)
        {
            item.Value.Tick(delta);
        }
    }
}
