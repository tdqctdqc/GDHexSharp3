using Godot;
using System;

public class OptionsWindow : WindowDialog
{
    private FuncButton _terrain, _faction;

    public override void _Ready()
    {
        _terrain = GetNode<FuncButton>("VBoxContainer/TerrainColors");
        _terrain.Set(SetTerrainColors);
        _faction = GetNode<FuncButton>("VBoxContainer/FactionColors");
        _faction.Set(SetFactionColors);
    }

    public void SetTerrainColors()
    {
        Func<HexModel, Color> colorFunc = (h) => h.Terrain.BaseColor;
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);
    }
    public void SetFactionColors()
    {
        Func<HexModel, Color> colorFunc = (h) => 
        { 
            if(h.FactionID > 1) return h.Faction.PrimaryColor;
            return h.Terrain.BaseColor;
        };
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);

    }
}
