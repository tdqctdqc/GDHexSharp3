using Godot;
using System;
using System.Collections.Generic;

public interface IBoundaryBrush : IBrush
{
    bool LinkNotBorder {get;}
    void Stroke(HexModel h1, HexModel h2);
    void Stroke(List<HexModel> path);
}
