using Godot;
using System;
using System.Linq;

public class CreateUnitWindow : WindowDialog
{
    private IData _data => Game.I.Session.Data;
    private Action<UnitModel> _createAction;
    private FuncButton _create; 
    private ListMenuButton _rank, _type; 
    private ListContainer<UnitType> _typeList; 
    private ListContainer<UnitRank> _rankList; 
	private Label _industrial, _recruits; 
    private float _indCost; 
    private float _recruitCost; 
    public override void _Ready()
    {
        _create = GetNode<FuncButton>("VBox/Create");
        _create.Set(Create);
        _rank = GetNode<ListMenuButton>("VBox/Rank");
        _type = GetNode<ListMenuButton>("VBox/Type");
        _industrial = GetNode<Label>("VBox/Industrial");
        _recruits = GetNode<Label>("VBox/Recruits");

        _typeList = new ListContainer<UnitType>(_data.UnitTypes.List.Where(t => t.IsHQ == false).ToList());
        _rankList = new ListContainer<UnitRank>(_data.UnitRanks.Ranks);

        _typeList.SetSelectAction((u) => SelectedParam());
        _rankList.SetSelectAction((u) => SelectedParam());
        
        _type.Set(_typeList);
        _rank.Set(_rankList);
    }
    public void Setup(Action<UnitModel> create)
    {
        _createAction = create; 
    }
    public void Create()
    {
        var faction = Game.I.Session.Client.Faction;
        var hex = Game.I.Session.Client.UI.HexSelector.SelectedHex;
        var unit = UnitGenerator.CreateMockUnit(_typeList.Selected.ID, _rankList.Selected.ID);
        SelectedParam();
        _createAction(unit);
    }
    public void SelectedParam()
    {
        var faction = Game.I.Session.Client.Faction;
        _indCost = _typeList.Selected.IndustrialCost * _rankList.Selected.SizeMultiplier;
        _industrial.Text = $"Industrial Points: {faction.IndustrialPoints} / {_indCost}";
        _recruitCost = _typeList.Selected.RecruitCost * _rankList.Selected.SizeMultiplier;
        _recruits.Text = $"Recruits: {faction.Recruits} / {_recruitCost}";
    }
}
