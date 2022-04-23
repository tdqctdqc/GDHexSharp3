using Godot;
using System;
using HexWargame;
using System.Collections.Generic;

public class UIHex : Control
{
    private MeshInstance2D _meshInstance; 
    private LocationGraphic _loc;
    private UIHexRoads _roads;
    private UIHexRivers _rivers; 
    private UIUnitHolder _units;
    public override void _Ready()
    {
        _roads = GetNode<UIHexRoads>("UIHexRoads");
        _rivers = GetNode<UIHexRivers>("UIHexRivers");
        _loc = GetNode<LocationGraphic>("LocationGraphic");
        _units = GetNode<UIUnitHolder>("UIUnitHolder");
        _meshInstance = GetNode<MeshInstance2D>("MeshInstance2D");
        var mesh = new HexMesh();
        _meshInstance.Mesh = mesh; 
    }

    public void Setup(HexModel hex)
    {
        var loc = hex.GetLocation();
        if(loc != null) _loc.Setup(loc);
        else _loc.Clear();
        _meshInstance.Modulate = hex.Terrain.BaseColor;
        _roads.Setup(hex);
        _rivers.Setup(hex);
    }
    public void SetupUnits(List<UnitModel> units)
    {
        _units.Setup(units);
    }
    public void Setup(Terrain terrain,  
                        LocationType loc = null, 
                        List<RoadType> roads = null, 
                        List<RiverType> rivers = null)
    {
        if(loc != null) _loc.Setup(loc);
        else _loc.Clear();
        _meshInstance.Modulate = terrain.BaseColor;
        if(roads != null) _roads.Setup(roads);
        else _roads.Clear();
        if(rivers != null) _rivers.Setup(rivers);
        else _rivers.Clear();
    }
    public void SetupUnitsMock(List<UnitModel> units, Action<UnitModel> selectUnit, Action<UnitModel> deselectUnit, Color prim, Color sec)
    {
        _units.SetupMock(units, selectUnit, deselectUnit, prim, sec);
    }
}
