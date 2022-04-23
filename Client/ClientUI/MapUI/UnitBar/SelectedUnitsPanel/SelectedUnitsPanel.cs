using Godot;
using System;
using System.Collections.Generic;

public class SelectedUnitsPanel : Control
{
	private GridContainer _grid; 
	public override void _Ready()
	{
		_grid = GetNode<GridContainer>("ScrollContainer/GridContainer");

	}
	public void Setup(List<UnitModel> units)
	{
		while(_grid.GetChildCount() > 0)
		{
			_grid.GetChild(0).Free();
		}
		foreach (var unit in units)
		{
			var uiCounter = Scenes.UIUnit;


			_grid.AddChild(uiCounter);
			uiCounter.Setup(unit, null, () => Game.I.Session.Client.UI.UnitSelector.DeselectUnit(unit), true);

			uiCounter.Select();
		}
		_grid.RectScale = new Vector2(.5f,.5f);
	}
}
