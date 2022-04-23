using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class EditorBar : Control, IBar
{
	private GenericPainterUI _terrain; 
	private FactionPainterUI _faction; 
	private RoadPainterUI _road; 
	private RiverPainterUI _river; 
	private BrushControls _brush;
	public IUIMode UIMode { get; private set; }
	private List<IPainterUI> _uis;
	public override void _Ready()
	{
		_faction = GetNode<FactionPainterUI>("VBox/FactionPainterUI");
		_terrain = GetNode<GenericPainterUI>("VBox/TerrainPainterUI");
		_road = GetNode<RoadPainterUI>("VBox/RoadPainterUI");
		_river = GetNode<RiverPainterUI>("VBox/RiverPainterUI");
		_brush = GetNode<BrushControls>("VBox/BrushControls");
		_uis = new List<IPainterUI>(){_faction,_road,_river,_terrain};
		UIMode = new EditorUIMode();
	}
	public void Open()
	{
		Visible = true;
		Game.I.Session.Client.UI.UIModeManager.SetUIMode(UIMode);
	}
	public void Toggle(IPainterUI active)
	{
		foreach (var ui in _uis)
		{
			if(ui != active) ui.Deactivate();
		}
	}

	public void Close()
	{
		foreach (var ui in _uis)
		{
			ui.Deactivate();
		}
		Visible = false;
	}
	public void Setup()
	{
		_faction.Setup(Toggle);
		_road.Setup(Toggle);
		_river.Setup(Toggle);

		_terrain.Setup<Terrain>("Terrain", 
						Toggle,
						Game.I.Session.Data.Terrain.Terrains.Values.ToList(),
						(t) => Game.I.Session.Editor.TerrainBrush.Current = t,
						() => Game.I.Session.Editor.TerrainBrush.Current = null,
						Game.I.Session.Editor.TerrainBrush);
		_brush.Setup();
	}
}
