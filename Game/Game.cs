using Godot;
using System;
using System.Collections.Generic;
using HexWargame;
using System.Linq;

public class Game : Node
{
    public static Game I { get; private set; }
    public ISession Session { get; private set; }
    public AppStateController AppStateController { get; private set; }
    public Timers Timers { get; private set; }
    public RandomNumberGenerator Random { get; private set; }
    private bool _init = false; 
    public override void _Ready()
    {
        if(I != null)
        {
            QueueFree();
            return;
        }
        I = this; 
        AppStateController = new AppStateController();
        AppStateController.SetState(new MainMenuAppState());
        Random = new RandomNumberGenerator();
        Random.Seed = (ulong)DateTime.Now.Millisecond;
        Timers = new Timers();
        AddChild(Timers);
    }
    public override void _Process(float delta)
    {
        if(Session == null) return; 
        if(Input.IsActionJustPressed("trigger2"))
        {
        }
    }
    public void SetSession(ISession session)
    {
        Session = session;
    }
}
