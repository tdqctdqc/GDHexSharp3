using Godot;
using HexWargame;
using System;

public class DrawRiverAction : IEditorAction
{
    private int _h1, _h2, _oldRiverType, _newRiverType;
    public DrawRiverAction(int h1, int h2, int riverType)
    {
        _h1 = h1;
        _h2 = h2;
        var hex1 = Cache<HexModel>.GetModel(h1);
        var hex2 = Cache<HexModel>.GetModel(h2);
        var oldRiver = hex1.GetRiverToHex(hex2);
        if(oldRiver == null) _oldRiverType = 0;
        else _oldRiverType = oldRiver.RiverType.ID;
        _newRiverType = riverType;
    }
    public void DoAction()
    {
        var hex1 = Cache<HexModel>.GetModel(_h1);
        var hex2 = Cache<HexModel>.GetModel(_h2);
        if(hex1.Terrain.IsWater || hex2.Terrain.IsWater)
        {
            return; 
        }
        if(_newRiverType != 0)
        {
            var river = RiverGenerator.BuildRiver(_newRiverType, _h1, _h2);
            Game.I.Session.Server.LogicInterface.River.BuildRiver(river);
        }
        else
        {
            var oldRiver = hex1.GetRiverToHex(hex2);
            if(oldRiver != null)
            {
                Game.I.Session.Server.LogicInterface.River.ClearRiver(oldRiver);
            }
        }
    }

    public IEditorAction GetUndoAction()
    {
        return new DrawRiverAction(_h1, _h2, _oldRiverType);
    }
}
