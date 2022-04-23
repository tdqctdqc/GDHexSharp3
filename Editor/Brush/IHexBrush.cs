using Godot;
using System;
using System.Collections.Generic;

public interface IHexBrush : IBrush
{
    void Stroke(List<HexModel> strokeHexes);
}
