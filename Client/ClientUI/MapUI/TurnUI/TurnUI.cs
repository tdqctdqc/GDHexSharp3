using Godot;
using System;

public class TurnUI : Control
{
    private FuncButton _doTurn;
    public override void _Ready()
    {
        _doTurn = GetNode<FuncButton>("DoTurn");
    }   
    public void Setup()
    {
        _doTurn.Set(Game.I.Session.Client.Events.Turn.DoRounds);
    }
}
