using Godot;
using HexWargame;
using System;

public class MapOptionButtons : Node
{
    private FuncButton _faction, _terrain, _cell, _plate, 
                        _continent, _temp, _moist, _showUnits,
                        _influence;
    public override void _Ready()
    {
        _faction = GetNode<FuncButton>("Faction");
        _faction.Set(Faction);
        _terrain = GetNode<FuncButton>("Terrain");
        _terrain.Set(Terrain);
        _cell = GetNode<FuncButton>("Cell");
        _cell.Set(Cell);
        _plate = GetNode<FuncButton>("Plate");
        _plate.Set(Plate);
        _continent = GetNode<FuncButton>("Continent");
        _continent.Set(Continent);
        _temp = GetNode<FuncButton>("Temperature");
        _temp.Set(Temperature);
        _moist = GetNode<FuncButton>("Moisture");
        _moist.Set(Moisture);
        _showUnits = GetNode<FuncButton>("ShowHideUnits");
        _showUnits.Set(ShowHideUnits);
        _influence = GetNode<FuncButton>("Influence");
        _influence.Set(Influence);
    }


    public void Faction()
    {
        Func<HexModel, Color> colorFunc = (h) => 
        { 
            if(h.FactionID > 1) return h.Faction.PrimaryColor;
            return h.Terrain.BaseColor;
        };
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);
    }
    public void Terrain()
    {
        Func<HexModel, Color> colorFunc = (h) => h.Terrain.BaseColor;
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);
    }
    public void Cell()
    {
        Func<HexModel,Color> colorFunc = (h) => 
        {
            return h.GetCell().Color;
        };
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);
    }
    public void Plate()
    {
        Func<HexModel,Color> colorFunc = (h) => 
        {
            return h.GetCell().Plate.Color;
        };
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);
    }
    public void Continent()
    {
        Func<HexModel,Color> colorFunc = (h) => 
        {
            return h.GetCell().Plate.Continent.Color;
        };
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);
    }

    public void Temperature()
    {
        Func<HexModel,Color> colorFunc = (h) => 
        {
            float temp = h.GetCell().Temperature;

            if(h.GetCell().Plate.Land) return new Color(0f,temp / 100f,0f);
            return new Color(0f,0f,temp / 100f);
        };
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);
    }

    public void Moisture()
    {
        Func<HexModel,Color> colorFunc = (h) => 
        {
            float moist = h.GetCell().Moisture;

            if(h.GetCell().Plate.Land) return new Color(moist / 100f,0,moist / 100f);
            return new Color(0f,0f,moist / 100f);
        };
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);
    }

    public void Influence()
    {
        Func<HexModel,Color> colorFunc = (h) => 
        {
            if(h.GetCell().Plate.Land == false) return Colors.Blue;

            float power = CacheManager.AI.InfluenceMap.GetPowerForHex(h.ID);
            var fac = h.Faction;
            return new Color(fac.PrimaryColor, power / 5000f);
        };
        Game.I.Session.Client.Graphics.MapGraphics.SetBaseMeshColorFunc(colorFunc);
    }

    public void ShowHideUnits()
    {
        var unitGraphics = Game.I.Session.Client.Graphics.MapGraphics.UnitGraphics;
        bool visible = unitGraphics.Visible;
        if(visible)
        {
            unitGraphics.Visible = false; 
            _showUnits.Text = "Show Units";
        }
        else
        {
            unitGraphics.Visible = true; 
            _showUnits.Text = "Hide Units";
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
