using Godot;
using System;
using System.Collections.Generic;

public class Front
{
    public List<FormationModel> Formations { get; private set; }
    public List<Cell> FrontCells { get; private set; }
    public FrontObjective Objective { get; private set; }
}
