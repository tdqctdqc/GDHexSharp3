using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class GiveBombardOrderCC : IClientCommand
{
    private List<int> _unitIDs; 
    private List<UnitModel> Units => Cache<UnitModel>.GetModels(_unitIDs);
    private int _factionID;
    private FactionModel Faction => Cache<FactionModel>.GetModel(_factionID);

    public string Hint => "Select target for bombardment";
    public GiveBombardOrderCC(List<UnitModel> units, int factionID)
    {
        _unitIDs = units.Select(u => u.ID).ToList();
        _factionID = factionID;
    }
    public bool Condition(out string warning)
    {
        var selectedUnit = Game.I.Session.Client.UI.UnitSelector.Current;
        var bombardAbility = Game.I.Session.Data.UnitAbilities.Bombard;
        
        if(selectedUnit == null)
        {
            warning = "No unit selected";
            return false;
        }
        if(selectedUnit.Faction.CheckIfFactionHostile(Faction) == false)
        {
            warning = "Unit selected is not hostile";
            return false;
        }
        var units = Units; 
        foreach (var unit in Units)
        {
            if(unit.UnitType.Abilities.ContainsKey(bombardAbility) == false)
            {
                warning = "Units cannot bombard";
                return false;
            }
            if(unit.UnitType.Abilities[bombardAbility] < unit.Hex.GetHexDistance(selectedUnit.Hex))
            {
                warning = "Unit not in range";
                return false;
            }
        }
        warning = "";
        return true; 
    }

    public void Do()
    {
        if(Condition(out _))
        {
            var units = Units; 
            var hex = Game.I.Session.Client.UI.UnitSelector.Current.Hex;
            var orders = new List<IOrder>();
            foreach (var unit in Units)
            {
                var order = new BombardOrder(hex.ID, unit.ID);
                orders.Add(order);
            }
            Game.I.Session.Server.StateInterface.OrderManager.SetOrders(units, orders);
        }
    }
}
