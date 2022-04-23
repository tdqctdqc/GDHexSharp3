using Godot;
using HexWargame;
using System;
using System.Collections.Generic;
using System.Linq;

public class DrawRiverPathAction : IEditorAction
{
    private List<int> _path;
    private List<int> _oldRiverTypeIDs;
    private List<int> _newRiverTypeIDs;
    public DrawRiverPathAction(List<int> path, int riverType)
    {
        _oldRiverTypeIDs = new List<int>();
        _newRiverTypeIDs = new List<int>();
        _path = path.ToList();
        for (int i = 0; i < _path.Count - 1; i += 2)
        {
            var hex1 = Cache<HexModel>.GetModel(_path[i]);
            var hex2 = Cache<HexModel>.GetModel(_path[i + 1]);
            var oldRiver = hex1.GetRiverToHex(hex2);
            if(oldRiver == null) _oldRiverTypeIDs.Add(0);
            else _oldRiverTypeIDs.Add(oldRiver.RiverTypeID);
            _newRiverTypeIDs.Add(riverType);
        }
    }
    public DrawRiverPathAction(List<int> path, List<int> newRiverTypeIDs)
    {
        _path = path.ToList();
        _newRiverTypeIDs = newRiverTypeIDs.ToList();
        _oldRiverTypeIDs = new List<int>();

        for (int i = 0; i < _path.Count - 1; i += 2)
        {
            var hex1 = Cache<HexModel>.GetModel(_path[i]);
            var hex2 = Cache<HexModel>.GetModel(_path[i + 1]);
            var oldRiver = hex1.GetRiverToHex(hex2);
            if(oldRiver == null) _oldRiverTypeIDs.Add(0);
            else _oldRiverTypeIDs.Add(oldRiver.RiverTypeID);
        }
    }
    public void DoAction()
    {
        var rivers = new List<RiverModel>();
        var toClear = new List<RiverModel>();
        for (int i = 0; i < _newRiverTypeIDs.Count; i++)
        {
            var newRiverType = _newRiverTypeIDs[i];
            var h1 = _path[2 * i];
            var h2 = _path[2 * i+1];
            
            var hex1 = Cache<HexModel>.GetModel(h1);
            var hex2 = Cache<HexModel>.GetModel(h2);
            if(hex1.Terrain.IsWater || hex2.Terrain.IsWater)
            {
                continue; 
            }
            if(newRiverType != 0)
            {
                var river = RiverGenerator.BuildRiver(newRiverType, h1, h2);
                rivers.Add(river);
            }
            else
            {
                var oldRiver = hex1.GetRiverToHex(hex2);
                if(oldRiver != null)
                {
                    toClear.Add(oldRiver);
                }
            }
        }
        Game.I.Session.Server.LogicInterface.River.BuildRivers(rivers);
        Game.I.Session.Server.LogicInterface.River.ClearRivers(toClear);
    }

    public IEditorAction GetUndoAction()
    {
        return new DrawRiverPathAction(_path, _oldRiverTypeIDs);
    }
}
