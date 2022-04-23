using Godot;
using System;
using System.Collections.Generic;

public interface IGraphNodeAggregation<P,C> : IGraphNode<P> 
{
    List<C> Children {get;}
}
