using Godot;
using System;
using System.Collections.Generic;

public class TopBar : Control
{
    private IClient _client => Game.I.Session.Client;
    private Label _industrial, _recruits;
    private ListContainer<FactionModel> _factionList; 
    private ListMenuButton _factionMenu; 
    private FuncButton _options, _battleSim; 
    public override void _Ready()
    {
        _options = GetNode<FuncButton>("HBoxContainer/Options");
        _options.Set(OpenOptionsWindow);
        _battleSim = GetNode<FuncButton>("HBoxContainer/BattleSim");
        _battleSim.Set(OpenBattleSimWindow);
        _factionMenu = GetNode<ListMenuButton>("HBoxContainer/Faction");
        _industrial = GetNode<Label>("HBoxContainer/IndustrialPoints");
        _recruits = GetNode<Label>("HBoxContainer/Recruits");
    }

    public void Setup()
    {
        _factionList = new ListContainer<FactionModel>(Cache<FactionModel>.GetModels(), SelectFaction);
        _factionMenu.Set(_factionList);
        _client.Events.UI.SelectedFaction += _factionList.Select;
        _client.Events.Turn.TurnDone += SetStats;
        Cache<FactionModel>.ModelsChanged += SetStats;
    }
    public void SelectFaction(FactionModel faction)
    {
        _client.SetFaction(faction);
        SetStats();
    }
    public void SetStats()
    {
        _industrial.Text = $"Industrial Points: {(int)_client.Faction.IndustrialPoints}";
        _recruits.Text = $"Recruits: {(int)_client.Faction.Recruits}";
    }
    public void SetStats(List<FactionModel> facs)
    {
        if(facs.Contains(_client.Faction))
        {
            _industrial.Text = $"Industrial Points: {(int)_client.Faction.IndustrialPoints}";
            _recruits.Text = $"Recruits: {(int)_client.Faction.Recruits}";
        }
    }

    public void OpenOptionsWindow()
    {
        Game.I.Session.Client.UI.MapUI.OptionsWindow.Popup_();
    }
    public void OpenBattleSimWindow()
    {
        Game.I.Session.Client.UI.MapUI.BattleSimWindow.Popup_();
    }
}
