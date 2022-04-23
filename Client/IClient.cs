using Godot;
using System;

public interface IClient 
{
    Events Events {get;}
    Graphics Graphics {get;}
    ClientUI UI {get;}
    FactionModel Faction { get; }
    ClientCommandManager Command {get;}

    void Setup();
    void SetFaction(FactionModel faction);
}
