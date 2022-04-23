using Godot;
using System;

public class UnitGraphic : Node2D
{
	private Label _rank, _stats, _ordinal;
	private MeshInstance2D _counter, _border, _natoIcon, _select, _force, _readiness, _supply;
	public override void _Ready()
	{
		_rank = GetNode<Label>("Rank");
		_stats = GetNode<Label>("Stats");
		_ordinal = GetNode<Label>("Ordinal");
		_border = GetNode<MeshInstance2D>("Border");
		_counter = GetNode<MeshInstance2D>("Counter");
		_natoIcon = GetNode<MeshInstance2D>("NATOIcon");
		_select = GetNode<MeshInstance2D>("Select");
		_force = GetNode<MeshInstance2D>("Force");
		_readiness = GetNode<MeshInstance2D>("Readiness");
		_supply = GetNode<MeshInstance2D>("Supply");
	}
	public void Setup(UnitModel model)
	{
		_rank.Text = model.UnitRank.Marker;

		var formation = model.Formation;
		if(formation != null)
		{
			_border.Modulate = formation.SecondaryColor;
			_natoIcon.Modulate = formation.SecondaryColor;
			_natoIcon.Texture = model.UnitType.NATOIcon;
			_rank.Modulate = formation.SecondaryColor;
			_stats.Modulate = formation.SecondaryColor;
			_ordinal.Modulate = formation.SecondaryColor;
			_counter.Modulate = formation.PrimaryColor;
		}
		else
		{
			_border.Modulate = model.Faction.SecondaryColor;
			_natoIcon.Modulate = model.Faction.SecondaryColor;
			_natoIcon.Texture = model.UnitType.NATOIcon;
			_rank.Modulate = model.Faction.SecondaryColor;
			_stats.Modulate = model.Faction.SecondaryColor;
			_ordinal.Modulate = model.Faction.SecondaryColor;
			_counter.Modulate = model.Faction.PrimaryColor;
		}
		
		_ordinal.Text = model.ID.ToString();

		_force.Modulate = GraphicsUtility.GetColorForPercent(model.Strength);
		_readiness.Modulate = GraphicsUtility.GetColorForPercent(model.Readiness);
		_supply.Modulate = GraphicsUtility.GetColorForPercent(model.Supply);
		_stats.Text = $"{(int)model.AttackPower / 10}-{(int)model.DefensePower / 10}";
	}	
	public void SetupMock(UnitModel model, Color prim, Color sec)
	{
		_rank.Text = model.UnitRank.Marker;
		
		_border.Modulate = sec;
		_natoIcon.Modulate = sec;
		_natoIcon.Texture = model.UnitType.NATOIcon;
		_rank.Modulate = sec;
		_stats.Modulate = sec;
		_ordinal.Modulate = sec;
		_counter.Modulate = prim;
		_ordinal.Text = model.ID.ToString();

		_force.Modulate = GraphicsUtility.GetColorForPercent(model.Strength);
		_readiness.Modulate = GraphicsUtility.GetColorForPercent(model.Readiness);
		_supply.Modulate = GraphicsUtility.GetColorForPercent(model.Supply);
		_stats.Text = $"{(int)model.AttackPower / 10}-{(int)model.DefensePower / 10}";
	}

	public void SetStats(float strength, float readiness, float supply)
	{
		_force.Modulate = GraphicsUtility.GetColorForPercent(strength);
		_readiness.Modulate = GraphicsUtility.GetColorForPercent(readiness);
		_supply.Modulate = GraphicsUtility.GetColorForPercent(supply);
	}
	public void Select()
	{
		_select.Visible = true; 
	}
	public void Deselect()
	{
		_select.Visible = false; 
	}
}
