using Godot;
using System;

public interface IClientCommand
{
    bool Condition(out string warning);
    string Hint {get;}
    void Do();
}
