using Godot;
using System;
using System.Collections.Generic;

public class UIEvents
{
    public Action<HexModel> MouseOverHexChanged { get; set; }
    public Action<HexModel> SecondClosestHexChanged { get; set; }
    public Action<HexModel> SelectedHexChanged { get; set; }
    public Action<List<UnitModel>> SelectedUnits { get; set; }
    public Action<List<UnitModel>> DeselectedUnits { get; set; }
    public Action<FactionModel> SelectedFaction { get; set; }
}
