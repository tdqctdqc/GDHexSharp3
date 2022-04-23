using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class BattleSimHexes : Control
{
    private List<List<UnitModel>> _unitLists;
    private List<List<UnitModel>> _selectedUnitLists;
    private CreateUnitWindow _createWindow; 
    private UIHex _n, _s, _ne, _se, _nw, _sw, _def;
    private List<UIHex> _hexes;
    private List<Label> _dmgReports; 
    private Dictionary<UnitModel, Vector3> _dmgTaken, _dmgDealt; 

    public override void _Ready()
    {
        _n = GetNode<UIHex>("Hexes/North");
        var nDmg = GetNode<Label>("Controllers/0/Label");
        
        _ne = GetNode<UIHex>("Hexes/NorthEast");
        var neDmg = GetNode<Label>("Controllers/1/Label");
        _se = GetNode<UIHex>("Hexes/SouthEast");
        var seDmg = GetNode<Label>("Controllers/2/Label");
        _s = GetNode<UIHex>("Hexes/South");
        var sDmg = GetNode<Label>("Controllers/3/Label");
        _sw = GetNode<UIHex>("Hexes/SouthWest");
        var swDmg = GetNode<Label>("Controllers/4/Label");
        _nw = GetNode<UIHex>("Hexes/NorthWest");
        var nwDmg = GetNode<Label>("Controllers/5/Label");
        _def = GetNode<UIHex>("Hexes/Def");
        var dDmg = GetNode<Label>("Controllers/6/Label");
        _hexes = new List<UIHex>(){_n, _ne, _se, _s, _sw, _nw, _def};
        _dmgReports = new List<Label>(){nDmg, neDmg, seDmg, sDmg, swDmg, nwDmg, dDmg};
    }
    public void Setup(List<List<UnitModel>> units, 
                        List<List<UnitModel>> selected, 
                        CreateUnitWindow createWindow,
                        Dictionary<UnitModel, Vector3> dmgTaken,
                        Dictionary<UnitModel, Vector3> dmgDealt)
    {
        _dmgDealt = dmgDealt;
        _dmgTaken = dmgTaken;
        _unitLists = units;
        _selectedUnitLists = selected;
        _createWindow = createWindow;
        SetupUnitControls();
    }
    private void SetupUnitControls()
    {
        for (int i = 0; i < 7; i++)
        {
            int index = i;
            var control = GetNode<UIUnitHolderControl>($"Controllers/{i}");
            Action<UnitModel> create = (u) =>
            {
                var list = _unitLists[index];
                if(list.Count > 6) return;
                list.Add(u);
                UpdateUnits();
            };
            Action add = () =>
            {
                _createWindow.Setup(create);
                _createWindow.Popup_();
            };
            Action remove = () =>
            {
                var list = _unitLists[index];
                var selectList = _selectedUnitLists[index];
                selectList.ForEach(u => list.Remove(u));
                selectList.Clear();
                UpdateUnits();
            };
            control.Setup(add, null, remove);
        }
    }
    public void UpdateUnits()
    {
        for (int i = 0; i < 7; i++)
        {
            int index = i;
            Color prim;
            Color sec;
            if(i == 6)
            {
                prim = Colors.Blue;
                sec = Colors.Red;
            }
            else
            {
                prim = Colors.Red;
                sec = Colors.Blue;
            }
            var selectList = _selectedUnitLists[index];
            var list = _unitLists[index];
            var hex = _hexes[index];
            Action<UnitModel> select = (u) =>
            {
                selectList.Add(u);
                ShowDamage();
            };
            Action<UnitModel> deselect = (u) =>
            {
                selectList.Remove(u);
                ShowDamage();
            };
            hex.SetupUnitsMock(_unitLists[index], select, deselect, prim, sec);
        }
    }
    public void ShowDamage()
    {
        for (int i = 0; i < 7; i++)
        {
            List<UnitModel> units = (_selectedUnitLists[i].Count > 0) ?
                                    _selectedUnitLists[i] : _unitLists[i];
            var dmgLabel = _dmgReports[i];
            if(units.Count == 0) 
            {
                ClearDamage(dmgLabel);
                continue;
            }

            float hpDmgTaken = 0f;
            float ipDmgTaken = 0f;
            float recruitsDmgTaken = 0f;
            foreach (var u in units)
            {
                if(_dmgTaken.ContainsKey(u) == false) continue; 
                var taken = _dmgTaken[u];
                hpDmgTaken += taken.x;
                ipDmgTaken += taken.y;
                recruitsDmgTaken += taken.z;
            }

            float hpDmgDealt = 0f;
            float ipDmgDealt = 0f;
            float recruitsDmgDealt = 0f;
            foreach (var u in units)
            {
                if(_dmgDealt.ContainsKey(u) == false) continue; 
                var dealt = _dmgDealt[u];
                hpDmgDealt += dealt.x;
                ipDmgDealt += dealt.y;
                recruitsDmgDealt += dealt.z;
            }

            dmgLabel.Text = $"Damage dealt: {(int)hpDmgDealt}/{(int)ipDmgDealt}/{(int)recruitsDmgDealt} \n Damage taken: {(int)hpDmgTaken}/{(int)ipDmgTaken}/{(int)recruitsDmgTaken}";
        }
    }
    private void ClearDamage(Label dmgLabel)
    {
            dmgLabel.Text = $"Damage dealt: \n Damage taken: ";
    }
}
