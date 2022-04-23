using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class GiveBuildRoadOrderCC : IClientCommand
{
    private int _unitID; 
    private int _roadTypeID;
    public string Hint => "Select hex to build road to";
    public GiveBuildRoadOrderCC(int unitID, RoadType type)
    {
        _unitID = unitID;
        _roadTypeID = type.ID;
    }
    public bool Condition(out string warning)
    {
        var unit = Cache<UnitModel>.GetModel(_unitID);
        var from = unit.Hex;
        var to = Game.I.Session.Client.UI.HexSelector.SelectedHex;
        var pathFinder = Game.I.Session.Utility.PathFinder;
        //change to be path that is cheapest to build along
        var path = pathFinder.FindUnitPath(unit, from, to);


        if(path == null || path.Count == 0)
        {
            warning = "No valid path found";
            return false;
        }
        foreach (var h in path)
        {
            if(unit.Faction.CheckIfFactionHostile(h.Faction))
            {
                warning = "Cannot build through hostile territory";
                return false;
            }
        }
        warning = "";
        return true; 
    }

    public void Do()
    {
        if(Condition(out _))
        {
            var unit = Cache<UnitModel>.GetModel(_unitID);
            var from = unit.Hex;
            var to = Game.I.Session.Client.UI.HexSelector.SelectedHex;
            var pathFinder = Game.I.Session.Utility.PathFinder;
            var roadType = Game.I.Session.Data.RoadTypes[_roadTypeID];

            //change to be path that is cheapest to build along
            var path = pathFinder.FindBuildRoadPath(roadType, from, to);

            var order = new BuildRoadOrder(_unitID, path, roadType);
            Game.I.Session.Server.StateInterface.OrderManager.SetOrder(unit, order);
        }
    }
}
