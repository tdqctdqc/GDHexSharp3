using Godot;
using System;

public interface IBar
{
    void Open();
    void Close();
    IUIMode UIMode {get;}
}
