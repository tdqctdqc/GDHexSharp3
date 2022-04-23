using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class BombardOrder : IOrder
{
    public int ID {get; private set;}
    public string Name => "Bombard";
    public int TargetHexID { get; private set; }
    public UnitModel Unit => Cache<UnitModel>.GetModel(ID);
    public HexModel TargetHex => Cache<HexModel>.GetModel(TargetHexID);
    public bool Completed => CheckCompletion();
    private float _storedAP;
    private float _apToFire = TurnManager.APPerRound;
    
    public BombardOrder(){}
    public BombardOrder(int targetHexID, int unitID)
    {
        ID = unitID;
        TargetHexID = targetHexID;
    }
    public void Do(float ap, Logic logic)
    {
        _storedAP += ap;
        if(_storedAP >= _apToFire)
        {
            _storedAP = 0f;
            logic.Combat.RegisterForBombard(Unit, TargetHex);
        }
    }

    public string GetString()
    {
        return TargetHexID.ToString();
    }

    private bool CheckCompletion()
    {
        var bombardAbility = Game.I.Session.Data.UnitAbilities.Bombard;

        return Unit.UnitType.Abilities[bombardAbility] < Unit.Hex.GetHexDistance(TargetHex);
    }
}
