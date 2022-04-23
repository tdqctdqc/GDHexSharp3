using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class OrderManager 
{
    private UnitSelector _unitSelector => Game.I.Session.Client.UI.UnitSelector; 
    private HexSelector _hexSelector => Game.I.Session.Client.UI.HexSelector;
    private PathFinder _pathFinder => Game.I.Session.Utility.PathFinder;
    public OrderManager()
    {
        Cache<UnitModel>.ModelsAdded += AddOrdersForUnits;
    }
    public void AddOrdersForUnits(List<UnitModel> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            int id = units[i].ID;
            var order = new BlankOrder(id);
            Game.I.Session.Server.StateInterface.OrderManager.SetOrder(units[i], order);
        }
        Game.I.Session.Client.Events.Order.OrdersUpdated?.Invoke();
    }
    public void TryGiveGoToOrder()
    {
        var units = _unitSelector.CurrentList;
        if(units.Count == 0) return;
        var dest = _hexSelector.MouseOverHex;
        if(dest == null) return; 
        var orders = new List<IOrder>();
        for(int i = 0; i < units.Count; i++)
        {
            var unit = units[i];
            var start = unit.Hex; 
            var path = _pathFinder.FindUnitPath(unit, start, dest);
            if(path == null) return; 
            var order = new GoToOrder(path.Select(h => h.ID).ToList(), unit.ID);
            orders.Add(order);
        }
        SetOrders(units, orders);
    }
    public void DoOrders(float ap, Logic logic)
    {
        var units = Cache<UnitModel>.GetModels();
        foreach (var unit in units)
        {
            unit.Order.Do(ap * unit.ReadinessAPMultiplier, logic);
        }
    }
    public void SetOrders(List<UnitModel> units, List<IOrder> orders)
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].Order = orders[i];
        }
        Cache<UnitModel>.UpdateModels(units);
        Game.I.Session.Client.Events.Order.OrdersUpdated?.Invoke();
    }
    public void SetOrder(UnitModel unit, IOrder order)
    {
        unit.Order = order;
        Cache<UnitModel>.UpdateModel(unit);
        Game.I.Session.Client.Events.Order.OrdersUpdated?.Invoke();
    }
}
