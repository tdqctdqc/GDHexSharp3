using Godot;
using System;

public class MouseController : Node
{
    public Vector2 MousePos => Game.I.Session.Client.Graphics.GetGlobalMousePosition();
    private bool _rmb = false;
    private int _rmbFrames = 0;
    public bool HoldingRMB => (_rmbFrames >= _holdMinFrames);
    private bool _lmb = false;
    private int _lmbFrames = 0;
    public bool HoldingLMB => (_lmbFrames >= _holdMinFrames);

    private int _holdMinFrames = 10;


    public override void _Process(float delta)
    {
        if(Input.IsActionPressed("rightClick"))
        {
            _rmb = true; 
            _rmbFrames++;
        }
        else
        {
            _rmb = false; 
            _rmbFrames = 0;
        }

        if(Input.IsActionPressed("leftClick"))
        {
            _lmb = true; 
            _lmbFrames++;
        }
        else
        {
            _lmb = false; 
            _lmbFrames = 0;
        }
    }
}
