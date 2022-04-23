using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class BuildRoadOrder : IOrder
{
    public int ID {get; private set;}
    private List<int> _roadPath;
    private int _roadTypeID;
    public string Name => "BuildRoad";
    public UnitModel Unit => Cache<UnitModel>.GetModel(ID);
    public RoadType RoadType => Game.I.Session.Data.RoadTypes[_roadTypeID];
    public bool Completed => _roadPath.Count == 0;
    private float _storedAP; 
    private float _storedEP;
    public BuildRoadOrder(){}
    public BuildRoadOrder(int unitID, List<HexModel> path, RoadType roadType)
    {
        ID = unitID;
        _roadPath = path.Select(h => h.ID).ToList();
        _roadTypeID = roadType.ID;
    }
    public BuildRoadOrder(int unitID, List<int> path, int roadType)
    {
        ID = unitID;
        _roadPath = new List<int>(path);
        _roadTypeID = roadType;
    }

    public void Do(float ap, Logic logic)
    {
        var unit = Unit; 
        if(Completed) return; 
        _storedAP += ap; 
        logic.Unit.DoBuildRoadLogic(unit, ref _storedAP, ref _storedEP, 
        RoadType, _roadPath, logic);
    }
}
