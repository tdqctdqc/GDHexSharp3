using Godot;
using System;
using HexWargame;

public class DrawRoadAction : IEditorAction
{
    private int _h1, _h2, _oldRoadType, _newRoadType;
    public DrawRoadAction(int h1, int h2, int roadType)
    {
        _h1 = h1;
        _h2 = h2;
        var hex1 = Cache<HexModel>.GetModel(h1);
        var hex2 = Cache<HexModel>.GetModel(h2);
        var oldRoad = hex1.GetRoadToHex(hex2);
        if(oldRoad == null) _oldRoadType = 0;
        else _oldRoadType = oldRoad.RoadType.ID;
        _newRoadType = roadType;
    }
    public void DoAction()
    {
        var hex1 = Cache<HexModel>.GetModel(_h1);
        var hex2 = Cache<HexModel>.GetModel(_h2);
        if(hex1.Terrain.IsWater || hex2.Terrain.IsWater)
        {
            return; 
        }
        if(_newRoadType != 0)
        {
            var road = RoadGenerator.BuildRoad(_newRoadType, _h1, _h2);
            Game.I.Session.Server.LogicInterface.Road.BuildRoad(road);
        }
        else
        {
            var oldRoad = hex1.GetRoadToHex(hex2);
            if(oldRoad != null)
            {
                Game.I.Session.Server.LogicInterface.Road.ClearRoad(oldRoad);
            }
        }
    }

    public IEditorAction GetUndoAction()
    {
        return new DrawRoadAction(_h1, _h2, _oldRoadType);
    }
}
