using Godot;
using System;

public class MapUI : Node
{
	private SelectedHexPanel _selectedHex; 
	public EditorBar EditorBar {get; private set;}
	public UnitBar UnitBar { get; private set; }
	private SidebarController _sidebarController; 
	public TurnUI TurnUI { get; private set; }
	public TopBar TopBar { get; private set; }
	public BuildUnitWindow BuildUnitWindow { get; private set; }
	public BuildFormationWindow BuildFormationWindow { get; private set; }
	public OptionsWindow OptionsWindow { get; private set; }
	public BattleSimWindow BattleSimWindow { get; private set; }
    public Hint Hint { get; private set; }
	
	public override void _Ready()
	{
		_selectedHex = GetNode<SelectedHexPanel>("Right/VBox/SelectedHexPanel");
		EditorBar = GetNode<EditorBar>("Left/EditorBar");
		UnitBar = GetNode<UnitBar>("Left/UnitBar");
		_sidebarController = GetNode<SidebarController>("Top/SidebarController");
		TurnUI = GetNode<TurnUI>("TurnUI");
		TopBar = GetNode<TopBar>("Top/TopBar");
		BuildUnitWindow = GetNode<BuildUnitWindow>("Windows/BuildUnitWindow");
		BuildFormationWindow = GetNode<BuildFormationWindow>("Windows/BuildFormationWindow");
		Hint = GetNode<Hint>("Top/Hint");
		OptionsWindow = GetNode<OptionsWindow>("Windows/OptionsWindow");
		BattleSimWindow = GetNode<BattleSimWindow>("Windows/BattleSimWindow");
	}
	public void Setup()
	{
		_selectedHex.Setup();
		EditorBar.Setup();
		UnitBar.Setup();
		_sidebarController.Setup();
		TurnUI.Setup();
		TopBar.Setup();
	}
}
