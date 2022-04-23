using Godot;
using System;
using System.Collections.Generic;

public interface IGraphNode<T>
{
    List<T> Neighbors {get;}
    Vector2 WorldPos {get;}
}
