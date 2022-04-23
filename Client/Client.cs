using Godot;
using System;

public class Client : Node, IClient
{
    public Events Events { get; private set; }
    public Graphics Graphics { get; private set; }
    public ClientUI UI { get; private set; }
    public ClientCommandManager Command { get; private set; }
    private int _factionID;
    public FactionModel Faction => Cache<FactionModel>.GetModel(_factionID);
    public Client()
    {
        _factionID = 2;
        Events = new Events();
        Graphics = new Graphics();
        AddChild(Graphics);
        Command = new ClientCommandManager();
        UI = new ClientUI();
        AddChild(UI);
    }
    
    public void Setup()
    {
        Graphics.Setup();
        Game.I.Session.Server.LoadStateRequest();
        UI.Setup();
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        UI.UIModeManager.HandleInput(@event);
    }
    public override void _Process(float delta)
    {
        UI.UIModeManager.DeltaInput(delta);
    }
    public void SetFaction(FactionModel faction)
    {
        if(faction.ID == _factionID) return;
        _factionID = faction.ID;
        Events.UI.SelectedFaction?.Invoke(faction);
    }
}
