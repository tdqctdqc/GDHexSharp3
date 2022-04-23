using Godot;
using System;
using System.Collections.Generic;

public class CompositeOrder : IOrder
{
    public string Name => "Composite";
    public bool Completed { get; private set; }

    public int ID => throw new NotImplementedException();
    public UnitModel Unit => Cache<UnitModel>.GetModel(ID);

    public CompositeOrder(List<IOrder> orders)
    {
        
    }

    public CompositeOrder()
    {
    }

    public void Do(float ap, Logic logic)
    {
        throw new NotImplementedException();
    }
}
