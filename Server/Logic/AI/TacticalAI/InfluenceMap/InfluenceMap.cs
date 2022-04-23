using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class InfluenceMap 
{
    private float[] _totalPowerMap;
    public InfluenceMap()
    {
    }
    public void Setup()
    {
        int width = Game.I.Session.MapGenPackage.Parameters.Width;
        int height = Game.I.Session.MapGenPackage.Parameters.Height;
        _totalPowerMap = new float[width * height];
    }
    public void SetPowerMap()
    {
        var hexes = Cache<HexModel>.GetModels();
        var facs = Cache<FactionModel>.GetModels();
        foreach (var hex in hexes)
        {
            var units = hex.Units; 
            if(units == null || units.Count == 0f) 
            {
                _totalPowerMap[hex.ID - 1] = 0f;
                continue; 
            } 
            _totalPowerMap[hex.ID - 1] = units.Sum(u => u.AttackPower + u.DefensePower);
        }
    }

    
    public float GetPowerForHex(int id)
    {
        return _totalPowerMap[id - 1]; 
    }
}
