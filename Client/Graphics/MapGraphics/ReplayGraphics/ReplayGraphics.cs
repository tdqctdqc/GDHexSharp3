using Godot;
using System;

public class ReplayGraphics : Node2D
{
    private TurnRecord _record; 
    private ReplayUnitGraphics _units;
    public int Round { get; private set; }
    public override void _Ready()
    {
        
    }
    public void ShowReplay()
    {
        Visible = true; 
        ShowRound(0);
    }
    public void HideReplay()
    {
        Visible = false; 
    }
    public void Setup()
    {
        _units = new ReplayUnitGraphics();
        AddChild(_units);
        Game.I.Session.Client.Events.Turn.NewTurnRecord += LoadTurnRecord;
    }
    public void LoadTurnRecord(TurnRecord record)
    {
        _record = record; 
        _units.Set(_record.UnitRecords);
        ShowRound(0);
    }
    public void ShowRound(int i)
    {
        if(_record == null) return; 
        Round = i;
        _units.ShowRound(i);
    }
    public void ShowNextRound()
    {
        if(_record == null) return; 
        if(Round + 1 > TurnManager.NumRounds) return; 
        ShowRound(Round + 1);
    }
    public void ShowPreviousRound()
    {
        if(_record == null) return; 
        if(Round - 1 < 0) return; 
        ShowRound(Round - 1);
    }
}