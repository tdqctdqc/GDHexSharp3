using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class UIHexRoads : Node
{
    private RoadGraphic _nRoad, _neRoad, _seRoad, _sRoad, _swRoad, _nwRoad;
    public override void _Ready()
    {
        _nRoad = GetNode<RoadGraphic>("NRoad");
        _neRoad = GetNode<RoadGraphic>("NERoad");
        _seRoad = GetNode<RoadGraphic>("SERoad");
        _sRoad = GetNode<RoadGraphic>("SRoad");
        _swRoad = GetNode<RoadGraphic>("SWRoad");
        _nwRoad = GetNode<RoadGraphic>("NWRoad");
    }
    public void Setup(HexModel hex)
    {
        Clear();
        var roads = CacheManager.Roads;

        var n = hex.GetNorth();
        if(n != null){ var r = roads.GetRoadByHexes(hex, n); _nRoad.Setup(r); }

        var ne = hex.GetNorthEast();
        if(ne != null){ var r = roads.GetRoadByHexes(hex, ne); _neRoad.Setup(r); }

        var se = hex.GetSouthEast();
        if(se != null){ var r = roads.GetRoadByHexes(hex, se); _seRoad.Setup(r); }

        var s = hex.GetSouth();
        if(s != null){ var r = roads.GetRoadByHexes(hex, s); _sRoad.Setup(r); }

        var sw = hex.GetSouthWest();
        if(sw != null){ var r = roads.GetRoadByHexes(hex, sw); _swRoad.Setup(r); }
        
        var nw = hex.GetNorthWest();
        if(nw != null){ var r = roads.GetRoadByHexes(hex, nw); if(r != null) _nwRoad.Setup(r); }
    }
    public void Setup(List<RoadType> roads)
    {
        Clear();
        if(roads.Count != 6) return;
        float increment = Mathf.Pi / 3f;

        var n = roads[0];
        if(n != null){ _nRoad.Setup(n, 0f); }

        var ne = roads[1];
        if(ne != null){ _neRoad.Setup(ne, increment); }

        var se = roads[2];
        if(se != null){ _seRoad.Setup(se, increment * 2f); }

        var s = roads[3];
        if(s != null){ _sRoad.Setup(s, increment * 3f); }

        var sw = roads[4];
        if(sw != null){ _swRoad.Setup(sw, increment * 4f); }
        
        var nw = roads[5];
        if(nw != null){ _nwRoad.Setup(nw, increment * 5f); }
    }
    public void Clear()
    {
        _nRoad.Clear();
        _neRoad.Clear();
        _seRoad.Clear();
        _sRoad.Clear();
        _swRoad.Clear();
        _nwRoad.Clear();

    }
}
