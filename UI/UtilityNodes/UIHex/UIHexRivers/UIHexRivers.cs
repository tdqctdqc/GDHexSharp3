using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class UIHexRivers : Node
{
    private RiverGraphic _nRiver, _neRiver, _seRiver, _sRiver, _swRiver, _nwRiver;
    public override void _Ready()
    {
        _nRiver = GetNode<RiverGraphic>("NRiver");
        _neRiver = GetNode<RiverGraphic>("NERiver");
        _seRiver = GetNode<RiverGraphic>("SERiver");
        _sRiver = GetNode<RiverGraphic>("SRiver");
        _swRiver = GetNode<RiverGraphic>("SWRiver");
        _nwRiver = GetNode<RiverGraphic>("NWRiver");
    }
    public void Setup(HexModel hex)
    {
        Clear();
        var rivers = CacheManager.Rivers;

        var n = hex.GetNorth();
        if(n != null){ var r = rivers.GetRiverByHexes(hex, n); _nRiver.Setup(r); }

        var ne = hex.GetNorthEast();
        if(ne != null){ var r = rivers.GetRiverByHexes(hex, ne); _neRiver.Setup(r); }

        var se = hex.GetSouthEast();
        if(se != null){ var r = rivers.GetRiverByHexes(hex, se); _seRiver.Setup(r); }

        var s = hex.GetSouth();
        if(s != null){ var r = rivers.GetRiverByHexes(hex, s); _sRiver.Setup(r); }

        var sw = hex.GetSouthWest();
        if(sw != null){ var r = rivers.GetRiverByHexes(hex, sw); _swRiver.Setup(r); }
        
        var nw = hex.GetNorthWest();
        if(nw != null){ var r = rivers.GetRiverByHexes(hex, nw); if(r != null) _nwRiver.Setup(r); }
    }
    public void Setup(List<RiverType> rivers)
    {
        Clear();
        if(rivers.Count != 6) return;
        float increment = 2f * Mathf.Pi / 3f;

        var n = rivers[0];
        if(n != null){ _nRiver.Setup(n, 0f); }

        var ne = rivers[1];
        if(ne != null){ _neRiver.Setup(ne, increment); }

        var se = rivers[2];
        if(se != null){ _seRiver.Setup(se, increment * 2f); }

        var s = rivers[3];
        if(s != null){ _sRiver.Setup(s, increment * 3f); }

        var sw = rivers[4];
        if(sw != null){ _swRiver.Setup(sw, increment * 4f); }
        
        var nw = rivers[5];
        if(nw != null){ _nwRiver.Setup(nw, increment * 5f); }
    }
    public void Clear()
    {
        _nRiver.Clear();
        _neRiver.Clear();
        _seRiver.Clear();
        _sRiver.Clear();
        _swRiver.Clear();
        _nwRiver.Clear();

    }
}
