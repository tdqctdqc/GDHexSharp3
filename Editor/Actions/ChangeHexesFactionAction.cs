using Godot;
using System;
using System.Collections.Generic;

public class ChangeHexesFactionAction : IEditorAction
{
    private List<int> _hexIDs, _oldFactionIDs, _newFactionIDs;
    public ChangeHexesFactionAction(List<int> hexIDs, List<int> oldFactionIDs, List<int> newFactionIDs)
    {
        _hexIDs = hexIDs;
        _oldFactionIDs = oldFactionIDs;
        _newFactionIDs = newFactionIDs;
    }
    public void DoAction()
    {
        var hexes = Cache<HexModel>.GetModels(_hexIDs);
        Func<HexModel, HexModel> action = (m) =>
        {
            int index = _hexIDs.IndexOf( m.ID );
            m.FactionID = _newFactionIDs[index];
            return m;
        };
        Game.I.Session.Server.StateInterface.UpdateModels<HexModel>(hexes, action);
    }

    public IEditorAction GetUndoAction()
    {
        return new ChangeHexesFactionAction(_hexIDs, _newFactionIDs, _oldFactionIDs);
    }
}
