using Godot;
using System;

public class Ticker : Node
{
    public Action Tick {get; set;}
    private float _tickPeriod; 
    private float _tickCounter; 
    public void Setup(float period)
    {
        _tickPeriod = period; 
        _tickCounter = 0f;
    }
    public override void _Process(float delta)
    {
        _tickCounter += delta; 
        if(_tickCounter > _tickPeriod)
        {
            _tickCounter = 0f;
            Tick?.Invoke();
        }
    }
}
