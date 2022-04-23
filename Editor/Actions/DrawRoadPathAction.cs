using Godot;
using HexWargame;
using System;
using System.Linq;
using System.Collections.Generic;

public class DrawRoadPathAction : IEditorAction
{
    private List<int> _path;
    private List<int> _oldRoadTypeIDs;
    private List<int> _newRoadTypeIDs;
    public DrawRoadPathAction(List<int> path, int roadType)
    {
        _oldRoadTypeIDs = new List<int>();
        _newRoadTypeIDs = new List<int>();
        _path = path.ToList();
        for (int i = 0; i < _path.Count - 1; i++)
        {
            var hex1 = Cache<HexModel>.GetModel(_path[i]);
            var hex2 = Cache<HexModel>.GetModel(_path[i + 1]);
            var oldRoad = hex1.GetRoadToHex(hex2);
            if(oldRoad == null) _oldRoadTypeIDs.Add(0);
            else _oldRoadTypeIDs.Add(oldRoad.RoadTypeID);
            _newRoadTypeIDs.Add(roadType);
        }
    }
    public DrawRoadPathAction(List<int> path, List<int> newRoadTypeIDs)
    {
        _path = path.ToList();
        _newRoadTypeIDs = newRoadTypeIDs.ToList();
        _oldRoadTypeIDs = new List<int>();

        for (int i = 0; i < _path.Count - 1; i++)
        {
            var hex1 = Cache<HexModel>.GetModel(_path[i]);
            var hex2 = Cache<HexModel>.GetModel(_path[i + 1]);
            var oldRoad = hex1.GetRoadToHex(hex2);
            if(oldRoad == null) _oldRoadTypeIDs.Add(0);
            else _oldRoadTypeIDs.Add(oldRoad.RoadTypeID);
        }
    }
    public void DoAction()
    {
        var roads = new List<RoadModel>();
        var toClear = new List<RoadModel>();
        for (int i = 0; i < _newRoadTypeIDs.Count; i++)
        {
            var newRoadType = _newRoadTypeIDs[i];
            var h1 = _path[i];
            var h2 = _path[i+1];
            var hex1 = Cache<HexModel>.GetModel(h1);
            var hex2 = Cache<HexModel>.GetModel(h2);
            if(hex1.Terrain.IsWater || hex2.Terrain.IsWater)
            {
                continue; 
            }
            if(newRoadType != 0)
            {
                var road = RoadGenerator.BuildRoad(newRoadType, h1, h2);
                roads.Add(road);
            }
            else
            {
                var oldRoad = hex1.GetRoadToHex(hex2);
                if(oldRoad != null)
                {
                    toClear.Add(oldRoad);
                }
            }
        }
        Game.I.Session.Server.LogicInterface.Road.BuildRoads(roads);
        Game.I.Session.Server.LogicInterface.Road.ClearRoads(toClear);
    }

    public IEditorAction GetUndoAction()
    {
        return new DrawRoadPathAction(_path, _oldRoadTypeIDs);
    }
}
