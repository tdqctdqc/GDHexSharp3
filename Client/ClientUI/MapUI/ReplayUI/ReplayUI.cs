using Godot;
using System;

public class ReplayUI : Node
{
    private FuncButton _nextRound, _prevRound, _toggle; 
    private Panel _roundNumberPanel;
    private Label _roundNumberLabel; 
    private int _round; 
    private ReplayGraphics _replayGraphics => Game.I.Session.Client.Graphics.MapGraphics.ReplayGraphics;
    private MapGraphics _mapGraphics => Game.I.Session.Client.Graphics.MapGraphics;
    private bool _toggled;
    public override void _Ready()
    {
        _nextRound = GetNode<FuncButton>("HBox/NextRound");
        _nextRound.Set(ShowNextRound);
        _prevRound = GetNode<FuncButton>("HBox/PreviousRound");
        _prevRound.Set(ShowPreviousRound);
        _toggle = GetNode<FuncButton>("HBox/Toggle");
        _toggle.Set(Toggle);
        _roundNumberPanel = GetNode<Panel>("Round");
        _roundNumberLabel = _roundNumberPanel.GetNode<Label>("Label");
        _roundNumberPanel.Visible = false; 
    }
    public void Toggle()
    {
        if(_toggled)
        {
            _toggle.Text = "View Replay";
            _toggled = false; 
            _roundNumberPanel.Visible = false; 
            _mapGraphics.HideReplay();
            _round = 0;

        }
        else
        {
            _toggle.Text = "Hide Replay";
            _toggled = true; 
            _roundNumberPanel.Visible = true; 
            _roundNumberLabel.Text = "Round: 0";
            _mapGraphics.ShowReplay();
            _round = 0;
        }
    }
    public void ShowNextRound()
    {
        if(_toggled)
        {
            if(_round + 1 > TurnManager.NumRounds) return; 
            _round = _round + 1;
            _roundNumberLabel.Text = $"Round: {_round}";
            _replayGraphics.ShowRound(_round);
        }
    }
    public void ShowPreviousRound()
    {
        if(_toggled)
        {
            if(_round - 1 < 0) return; 
            _round = _round - 1;
            _roundNumberLabel.Text = $"Round: {_round}";
            _replayGraphics.ShowRound(_round);
        }
    }
}
