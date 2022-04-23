using Godot;
using System;
using System.Collections.Generic;

public class ChangeTerrainAction : IEditorAction
{
    private List<int> _hexIDs, _oldTerrainIDs, _newTerrainIDs;
    public ChangeTerrainAction(List<int> hexIDs, List<int> oldFactionIDs, List<int> newFactionIDs)
    {
        _hexIDs = hexIDs;
        _oldTerrainIDs = oldFactionIDs;
        _newTerrainIDs = newFactionIDs;
    }
    public void DoAction()
    {
        GD.Print("changing terrains");
        var hexes = Cache<HexModel>.GetModels(_hexIDs);
        Func<HexModel, HexModel> action = (m) =>
        {
            int index = _hexIDs.IndexOf( m.ID );
            m.TerrainID = _newTerrainIDs[index];
            return m;
        };
        Game.I.Session.Server.StateInterface.UpdateModels<HexModel>(hexes, action);
    }

    public IEditorAction GetUndoAction()
    {
        return new ChangeHexesFactionAction(_hexIDs, _newTerrainIDs, _oldTerrainIDs);
    }
}
