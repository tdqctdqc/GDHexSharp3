using Godot;
using System;
using System.Collections.Generic;

public class RiverInterface
{
    private Logic _logic;
    public RiverInterface(Logic logic)
    {
        _logic = logic; 
    }
    public void BuildRiver(RiverModel river)
    {
        _logic.River.PushRiversToServer(new List<RiverModel>(){river});
    }
    public void ClearRiver(RiverModel river)
    {
        Game.I.Session.Server.StateInterface.DeleteModel(river);
    }
    public void ClearRivers(List<RiverModel> rivers)
    {
        Game.I.Session.Server.StateInterface.DeleteModels(rivers);
    }
    public void BuildRivers(List<RiverModel> rivers)
    {
        _logic.River.PushRiversToServer(rivers);
    }
}
