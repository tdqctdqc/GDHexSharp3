using Godot;
using System;

public class TurnEvents
{
    public Action DoRounds { get; set; }
    public Action<TurnRecord> NewTurnRecord { get; set; }
    public Action TurnDone { get; set; }
    public Action NewTurn { get; set; }
}
