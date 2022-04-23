using Godot;
using System;

public class TurnManager 
{
    private Logic _logic; 
    public static int NumRounds = 10;
    public static float APPerRound = 1f;
    public TurnManager(Logic logic)
    {
        _logic = logic;
        Game.I.Session.Client.Events.Turn.DoRounds += DoRounds; 
    }
    public void StartTurn()
    {
        Game.I.Session.Client.Events.Turn.NewTurn?.Invoke();
        CacheManager.AI.SetCache();
    }
    public void DoRounds()
    {
        var record = new TurnRecord();
        record.WriteRound(0);
        for (int i = 0; i < NumRounds; i++)
        {
            DoRound();
            record.WriteRound(i + 1);
        }
        _logic.Road.EndOfTurnBuildRoads();

        _logic.Supply.DoSupply();
        _logic.Production.DoProduction();

        
        Game.I.Session.Client.Events.Turn.TurnDone?.Invoke();
        Game.I.Session.Client.Events.Turn.NewTurnRecord?.Invoke(record);
        CacheManager.ClearDeleteQueues?.Invoke();

        StartTurn();
    }

    private void DoRound()
    {
        Game.I.Session.Server.StateInterface.OrderManager.DoOrders(APPerRound, _logic);
        _logic.Combat.DoRoundCombats();
    }
}
