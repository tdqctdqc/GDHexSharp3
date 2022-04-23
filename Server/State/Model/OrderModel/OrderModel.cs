using Godot;
using System;
using System.Collections.Generic;

public class OrderModel : IModel
{
    
    public string TableNameI => TableName; 
    public static string TableName = "UnitOrder";
    public static string PrimaryKeyName = "ID";
    
    public int ID { get; set; }
    public string Name { get; set; }
    public string OrdersText { get; set; }

    public int NumFields => throw new NotImplementedException();

    public BackingModel Backing => throw new NotImplementedException();

    public OrderModel(){}
    public OrderModel(IOrder order)
    {
        ID = order.ID;
        Name = order.Name;
    }

    public void SyncFromBacking()
    {
        throw new NotImplementedException();
    }
}
