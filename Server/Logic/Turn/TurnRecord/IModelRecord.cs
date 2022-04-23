using Godot;
using System;

public interface IModelRecord<T> where T : IModel 
{
    void WriteRound(int i);
}
