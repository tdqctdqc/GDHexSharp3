using Godot;
using System;
namespace HexWargame
{
public static class FactionModelExt 
{
    public static bool CheckIfFactionHostile(this FactionModel a, FactionModel b)
    {
        if(a.ID == 1) return false; 
        if(b.ID == 1) return false; 
        if(a.ID != b.ID) return true;
        return false; 
    }
}
}
