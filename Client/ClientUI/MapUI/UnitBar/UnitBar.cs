using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class UnitBar : Control, IBar
{
    private IClient _client => Game.I.Session.Client;
    private UnitMouseInput _mouse; 
    private SelectedUnitPanel _single; 
    private SelectedUnitsPanel _multi;
    private FuncButton _setFormation, _bombard, _buildRoad, _merge, _split,
                        _defend, _attack, _buildUnit, _buildFormation; 
    private ColorPickerButton _changePrimaryColor, _changeSecondaryColor; 
    private ListMenuButton _roadTypes; 
    private ListContainer<RoadType> _roadList; 
    public IUIMode UIMode { get; private set; }
    public override void _Ready()
    {
        _single = GetNode<SelectedUnitPanel>("Single");
        _multi = GetNode<SelectedUnitsPanel>("MultiUnit");
        _setFormation = GetNode<FuncButton>("Buttons/SetFormation");
        _setFormation.Set(SetFormation);
        _bombard = GetNode<FuncButton>("Buttons/Bombard");
        _bombard.Set(Bombard);
        _buildRoad = GetNode<FuncButton>("Buttons/BuildRoad");
        _buildRoad.Set(BuildRoad);
        _roadTypes = GetNode<ListMenuButton>("Buttons/RoadTypes");
        _merge = GetNode<FuncButton>("Buttons/Merge");
        _merge.Set(Merge);
        _split = GetNode<FuncButton>("Buttons/Split");
        _split.Set(Split);
        _defend = GetNode<FuncButton>("Buttons/Defend");
        _defend.Set(SetDefenseLine);
        _attack = GetNode<FuncButton>("Buttons/Attack");
        _attack.Set(SetAttackObjective);
        _buildUnit = GetNode<FuncButton>("Buttons/BuildUnit");
        _buildUnit.Set(BuildUnit);
        _buildFormation = GetNode<FuncButton>("Buttons/BuildFormation");
        _buildFormation.Set(BuildFormation);
        _changePrimaryColor = GetNode<ColorPickerButton>("Buttons/ChangePrimaryColor");
        _changePrimaryColor.Connect("color_changed", this, nameof(ChangePrimaryColor));
        _changeSecondaryColor = GetNode<ColorPickerButton>("Buttons/ChangeSecondaryColor");
        _changeSecondaryColor.Connect("color_changed", this, nameof(ChangeSecondaryColor));

        UIMode = new UnitUIMode();
    }

    

    public void Setup()
    {
        _mouse = new UnitMouseInput();
        _roadList = new ListContainer<RoadType>( Game.I.Session.Data.RoadTypes.RoadTypes.Values.ToList());
        _roadTypes.Set(_roadList);
    }
    public void Open()
    {
        Visible = true;
        Game.I.Session.Client.UI.UIModeManager.SetUIMode(UIMode);
        Game.I.Session.Client.Events.UI.SelectedUnits += SelectUnits;
    }
    public void Close()
    {
        Visible = false; 
        Game.I.Session.Client.Events.UI.SelectedUnits -= SelectUnits;
        Game.I.Session.Client.Command.CancelCommand();

    }
    public void SelectUnits(List<UnitModel> units)
    {
        _single.Visible = false;
        _multi.Visible = false; 
        CheckHideBombard(units);
        CheckHideSetFormation(units);
        CheckHideBuildRoad(units);
        CheckHideMerge(units);
        CheckHideSplit(units);
        CheckHideDefendAttack(units);
        CheckHideBuildUnits();
        if(units.Count == 0) return; 
        if(units.Count == 1) SelectSingleUnit(units[0]);
        else SelectMultipleUnits(units);
    }


    private void SelectSingleUnit(UnitModel unit)
    {
        _single.Visible = true;
        _multi.Visible = false; 
        _single.Setup(unit);
    }
    private void SelectMultipleUnits(List<UnitModel> unit)
    {
        _single.Visible = false;
        _multi.Visible = true; 
        _multi.Setup(unit);
    }
    public void BuildUnit()
    {
        _client.UI.MapUI.BuildUnitWindow.Open();
    }
    public void BuildFormation()
    {
        _client.UI.MapUI.BuildFormationWindow.Open();
    }
    public void SetFormation()
    {
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;
        var command = new ChangeUnitFormationCommand(units);
        Game.I.Session.Client.Command.SetCommand(command);
    }
    private void CheckHideSetFormation(List<UnitModel> units)
    {
        if(units.Count == 0) _setFormation.Visible = false;
        else _setFormation.Visible = true; 
    }
    private void CheckHideBombard(List<UnitModel> units)
    {
        var bombardAbility = Game.I.Session.Data.UnitAbilities.Bombard;
        if(units.Count == 0)
        {
            _bombard.Visible = false; 
            return;
        }
        foreach (var unit in units)
        {
            if(unit.UnitType.Abilities.ContainsKey(bombardAbility) == false)
            {
                _bombard.Visible = false;
                return; 
            } 
        }
        _bombard.Visible = true; 
    }
    private void CheckHideBuildRoad(List<UnitModel> units)
    {
        if(units.Count > 1 || units.Count == 0)
        {
            _roadTypes.Visible = false;
            _buildRoad.Visible = false;
            return;
        } 
        var eng = Game.I.Session.Data.UnitAbilities.Engineer;
        if(units[0].UnitType.Abilities.ContainsKey(eng) == false)
        {
            _roadTypes.Visible = false;
            _buildRoad.Visible = false;
            return;
        }
        _roadTypes.Visible = true;
        _buildRoad.Visible = true;
    }
    private void CheckHideMerge(List<UnitModel> units)
    {
        if(units.Count != 3)
        {
            _merge.Visible = false; 
            return;
        }
        var unit1 = units[0];
        var ranks = Game.I.Session.Data.UnitRanks.Ranks;
        var rank = unit1.UnitRank;
        var type = unit1.UnitType;
        var hex = unit1.Hex;
        if(rank.Rank >= ranks.Count - 1)
        {
            _merge.Visible = false; 
            return;
        }
        foreach (var unit in units)
        {
            if(unit.UnitRank != rank || unit.UnitType != type || unit.Hex != hex)
            {
                _merge.Visible = false; 
                return;
            }
        }
        _merge.Visible = true; 
    }
    
    private void CheckHideBuildUnits()
    {
        _buildFormation.Visible = false;
        _buildUnit.Visible = false; 
        var hex = _client.UI.HexSelector.SelectedHex;
        if(hex == null) return;
        if(hex.FactionID != _client.Faction.ID) return; 
        if(hex.Terrain.IsWater) return;
        _buildFormation.Visible = true;
        _buildUnit.Visible = true; 
    }
    private void CheckHideSplit(List<UnitModel> units)
    {
        if(units.Count != 1)
        {
            _split.Visible = false; 
            return;
        }
        var unit = units[0];
        if(unit.Hex.Units.Count > Constants.MaxUnitsInHex - 2) 
        {
            _split.Visible = false; 
            return;
        }
        if(unit.UnitRank.Rank < 2) 
        {
            _split.Visible = false; 
            return;
        }
        _split.Visible = true; 
    }
    private void CheckHideDefendAttack(List<UnitModel> units)
    {
        _defend.Visible = false;
        _attack.Visible = false;
        if(units.Count != 1)
        {
            return;
        }
        var unit = units[0];
        if(unit.UnitType.IsHQ == false) 
        {
            return;
        }
        _defend.Visible = true; 

        if(unit.Formation.DefenseLine == null) return;
        _attack.Visible = true; 
    }
    public void Bombard()
    {
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;
        if(units.Count == 0) return; 
        var faction = units[0].Faction;
        var command = new GiveBombardOrderCC(units, faction.ID);
        Game.I.Session.Client.Command.SetCommand(command);
    }

    public void BuildRoad()
    {
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;
        if(units.Count == 0) return; 
        if(units.Count > 1) return; 
        var faction = units[0].Faction;
        var command = new GiveBuildRoadOrderCC(units[0].ID, _roadList.Selected);
        Game.I.Session.Client.Command.SetCommand(command);
    }
    public void Split()
    {
        var unit = Game.I.Session.Client.UI.UnitSelector.Current;
        Game.I.Session.Server.LogicInterface.Unit.SplitUnit(unit);
    }
    public void Merge()
    {
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;
        Game.I.Session.Server.LogicInterface.Unit.MergeUnits(units);
    }
    public void SetDefenseLine()
    {
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;

        if(units.Count != 1) return;
        var unit = units[0];
        if(unit.UnitType.IsHQ == false) return;
        var formation = unit.Formation;
        var command = new GiveFormationDefendOrderCC(formation);
        Game.I.Session.Client.Command.SetCommand(command);
    }
    private void SetAttackObjective()
    {
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;
        if(units.Count != 1) return;
        var unit = units[0];
        if(unit.UnitType.IsHQ == false) return;
        var formation = unit.Formation;
        var command = new GiveFormationAttackObjectiveCC(formation);
        Game.I.Session.Client.Command.SetCommand(command);
    }
    private void ChangePrimaryColor(Color color)
    {
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;
        if(units.Count != 1) return;
        var unit = units[0];
        var formation = unit.Formation;
        if(formation == null) return; 
        formation.PrimaryColorString = _changePrimaryColor.Color.ToHtml();
        Cache<UnitModel>.UpdateModels(formation.Units);
    }
    private void ChangeSecondaryColor(Color color)
    {
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;
        if(units.Count != 1) return;
        var unit = units[0];
        var formation = unit.Formation;
        if(formation == null) return; 
        formation.SecondaryColorString = _changeSecondaryColor.Color.ToHtml();
        Cache<UnitModel>.UpdateModels(formation.Units);
    }
}
