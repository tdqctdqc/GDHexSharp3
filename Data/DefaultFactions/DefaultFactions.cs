using Godot;
using System;
using System.Collections.Generic;

public class DefaultFactions 
{
    public List<FactionModel> Factions { get; private set; }
    public DefaultFactions(List<FactionModel> factions)
    {
        Factions = new List<FactionModel>(factions);
    }
}
