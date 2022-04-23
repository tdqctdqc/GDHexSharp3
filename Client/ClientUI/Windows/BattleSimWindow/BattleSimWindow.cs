using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class BattleSimWindow : WindowDialog
{
    private UIHex _n, _s, _ne, _se, _nw, _sw, _def;
    private List<UIHex> _hexes;
    private List<List<UnitModel>> _unitLists;
    private List<List<UnitModel>> _selectedUnitLists;
    private List<ListContainer<RiverType>> _riverTypes;
    private ListMenuButton _defTerrain;
    private ListContainer<Terrain> _defTerrainListContainer; 
    private CreateUnitWindow _createWindow;
    private BattleSimHexes _hexesDisplay;
    private Dictionary<UnitModel, Vector3> _dmgTaken;
    private Dictionary<UnitModel, Vector3> _dmgDealt;
    private FuncButton _doRound, _doTurn;
    public override void _Ready()
    {
        _dmgDealt = new Dictionary<UnitModel, Vector3>();
        _dmgTaken = new Dictionary<UnitModel, Vector3>();
        _doRound = GetNode<FuncButton>("Controls/DoRound");
        _doRound.Set(DoRound);
        _doTurn = GetNode<FuncButton>("Controls/DoTurn");
        _doTurn.Set(DoTurn);
        _createWindow = GetNode<CreateUnitWindow>("CreateUnitWindow");
        _n = GetNode<UIHex>("VBoxContainer/BattleSimHexes/Hexes/North");
        _s = GetNode<UIHex>("VBoxContainer/BattleSimHexes/Hexes/South");
        _ne = GetNode<UIHex>("VBoxContainer/BattleSimHexes/Hexes/NorthEast");
        _se = GetNode<UIHex>("VBoxContainer/BattleSimHexes/Hexes/SouthEast");
        _nw = GetNode<UIHex>("VBoxContainer/BattleSimHexes/Hexes/NorthWest");
        _sw = GetNode<UIHex>("VBoxContainer/BattleSimHexes/Hexes/SouthWest");
        _def = GetNode<UIHex>("VBoxContainer/BattleSimHexes/Hexes/Def");
        _hexes = new List<UIHex>(){_n, _ne, _se, _s, _sw, _nw, _def};
        SetupLists();
        var terrainList = Game.I.Session.Data.Terrain.Terrains.Values.ToList();
        _defTerrainListContainer = new ListContainer<Terrain>(terrainList, SelectDefTerrain);
        _defTerrain = GetNode<ListMenuButton>("Scroll/VBox/DefTerrain/MenuButton");
        _defTerrain.Set(_defTerrainListContainer);
        SetupRivers();

        _hexesDisplay = GetNode<BattleSimHexes>("VBoxContainer/BattleSimHexes");
        _hexesDisplay.Setup(_unitLists, _selectedUnitLists, _createWindow, _dmgTaken, _dmgDealt);
    }
    private void SetupLists()
    {
        _unitLists = new List<List<UnitModel>>();
        _selectedUnitLists = new List<List<UnitModel>>();

        for (int i = 0; i < 7; i++)
        {
            _unitLists.Add(new List<UnitModel>());
            _selectedUnitLists.Add(new List<UnitModel>());
        }
    }
    private void SetupRivers()
    {
        var riverList = Game.I.Session.Data.RiverTypes.RiverTypes.Values.ToList();
        _riverTypes = new List<ListContainer<RiverType>>();
        for (int i = 0; i < 6; i++)
        {
            int index = i;
            Action<RiverType> select = (r) => 
            {
                var riverTypes = new List<RiverType>(){null,null,null,null,null,null};
                riverTypes[(6 - index) % 6] = r;
                _hexes[index].Setup(_defTerrainListContainer.Selected, null, null, riverTypes);
            };
            var river = new ListContainer<RiverType>(riverList, select);
            var riverMenu = GetNode<ListMenuButton>($"Scroll/VBox/River{index}/MenuButton");
            _riverTypes.Add(river);
            riverMenu.Set(river, new List<string>(){"none"}, new List<Action>(){() => select(null)});
        }
    }
    public void SelectDefTerrain(Terrain terrain)
    {
        foreach (var h in _hexes)
        {
            h.Setup(terrain);
        }
    }
    public void DoRound()
    {
        _dmgTaken.Clear();
        _dmgDealt.Clear();
        var attackers = new List<UnitModel>();
        for (int i = 0; i < 6; i++)
        {
            attackers.AddRange(_unitLists[i]);
        }
        var defenders = new List<UnitModel>(_unitLists[6]);
        CombatLogic.DoGroundAttack(defenders, 
                                    attackers, 
                                    _defTerrainListContainer.Selected, 
                                    (u,v) => GetRiverType(u), 
                                    (u) => {},
                                    ReportDamage);
        _hexesDisplay.UpdateUnits();
        _hexesDisplay.ShowDamage();
    }
    public void DoTurn()
    {
        _dmgTaken.Clear();
        _dmgDealt.Clear();
        var attackers = new List<UnitModel>();
        for (int i = 0; i < 6; i++)
        {
            attackers.AddRange(_unitLists[i]);
        }
        var defenders = new List<UnitModel>(_unitLists[6]);
        for (int i = 0; i < TurnManager.NumRounds; i++)
        {
            CombatLogic.DoGroundAttack(defenders, 
                                    attackers, 
                                    _defTerrainListContainer.Selected, 
                                    (u,v) => GetRiverType(u), 
                                    (u) => {},
                                    ReportDamage);
        }
        
        _hexesDisplay.UpdateUnits();
        _hexesDisplay.ShowDamage();
    }
    public void ReportDamage(UnitModel hitter, UnitModel target, Vector3 damage)
    {
        if(_dmgTaken.ContainsKey(target) == false) _dmgTaken.Add(target, Vector3.Zero);
        if(_dmgDealt.ContainsKey(hitter) == false) _dmgDealt.Add(hitter, Vector3.Zero);
        _dmgTaken[target] += damage;
        _dmgDealt[hitter] += damage; 
    }
    public RiverType GetRiverType(UnitModel attacker)
    {
        for (int i = 0; i < 6; i++)
        {
            if(_unitLists[i].Contains(attacker))
            {
                return _riverTypes[i].Selected;
            }
        }
        return null;
    }
}
