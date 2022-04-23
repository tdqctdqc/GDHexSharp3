using Godot;
using System;

public class BlankOrder : IOrder
{
    public int ID {get; private set;}
    public string Name => "Blank";
    public UnitModel Unit => Cache<UnitModel>.GetModel(ID);
    public bool Completed { get; private set; }

    public BlankOrder(){}
    public BlankOrder(int id)
    {
        ID = id;
    }
    public void Do(float ap, Logic logic)
    {
    }
}
