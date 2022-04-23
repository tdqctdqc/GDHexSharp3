using Godot;
using System;
using System.Collections.Generic;

public class MapEvents
{
    public Action<List<HexModel>, FactionModel> ChangedHexesFaction { get; set; }
    
}
