using Godot;
using System;
using System.Collections.Generic;

public class RoadInterface 
{
    private Logic _logic;
    public RoadInterface(Logic logic)
    {
        _logic = logic; 
    }
    public void BuildRoad(RoadModel road)
    {
        _logic.Road.PushRoadsToServer(new List<RoadModel>(){road});
    }
    public void ClearRoad(RoadModel road)
    {
        Game.I.Session.Server.StateInterface.DeleteModel(road);
    }
    public void ClearRoads(List<RoadModel> roads)
    {
        Game.I.Session.Server.StateInterface.DeleteModels(roads);
    }
    public void BuildRoads(List<RoadModel> roads)
    {
        _logic.Road.PushRoadsToServer(roads);
    }
}
