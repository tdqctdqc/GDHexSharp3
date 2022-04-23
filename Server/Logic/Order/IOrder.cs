using Godot;
using System;

public interface IOrder
{
    int ID {get;}
    string Name { get; }
    void Do(float ap, Logic logic);
    UnitModel Unit {get;}
    bool Completed {get;}
}
