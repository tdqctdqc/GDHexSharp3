using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Brush : IBrush
{
    public static HexBrush<Terrain> GetTerrainBrush()
    {
        Func<HexModel, bool> terrainCond = (h) => true; 
        Func<List<HexModel>, IEditorAction> terrainBrushAction = (h) => 
        {
            var hexIDs = h.Select(m => m.ID).ToList();
            int newTerrainID = Game.I.Session.Editor.TerrainBrush.Current.ID; 
            var newTerrainIDs = new List<int>();
            for (int i = 0; i < h.Count; i++)
            {
                newTerrainIDs.Add(newTerrainID);
            }
            var oldTerrainIDs = h.Select(m => m.TerrainID).ToList();
            return new ChangeTerrainAction(hexIDs, oldTerrainIDs, newTerrainIDs);
        };
        var terrainBrush = new HexBrush<Terrain>(terrainBrushAction, terrainCond);
        return terrainBrush;
    }
    public static HexBrush<FactionModel> GetFactionBrush()
    {
        Func<HexModel, bool> factionCond = (h) => h.Terrain.IsWater == false; 
        Func<List<HexModel>, IEditorAction> factionBrushAction = (h) => 
        {
            var hexIDs = h.Select(m => m.ID).ToList();
            int newFactionID = Game.I.Session.Editor.FactionBrush.Current.ID; 
            var newFactionIDs = new List<int>();
            for (int i = 0; i < h.Count; i++)
            {
                newFactionIDs.Add(newFactionID);
            }
            var oldFactionIDs = h.Select(m => m.FactionID).ToList();
            return new ChangeHexesFactionAction(hexIDs, oldFactionIDs, newFactionIDs);
        };
        var factionBrush = new HexBrush<FactionModel>(factionBrushAction, factionCond);
        return factionBrush;
    }

    public static BoundaryBrush<RoadType> GetRoadBrush()
    {
        Func<HexModel, bool> roadCond = (h) => h.Terrain.IsWater == false; 
        Func<List<HexModel>, IEditorAction> roadStrokeFunc = (h) => 
        {
            var h1 = h[0];
            var h2 = h[1];
            int roadTypeID;
            if(Game.I.Session.Editor.RoadBrush.Current != null)
            {
                roadTypeID = Game.I.Session.Editor.RoadBrush.Current.ID;
            }
            else
            {
                roadTypeID = 0;
            }
            return new DrawRoadAction(h1.ID, h2.ID, roadTypeID);
        };
        Func<List<HexModel>, IEditorAction> roadStrokePathFunc = (h) => 
        {
            int roadTypeID;
            if(Game.I.Session.Editor.RoadBrush.Current != null)
            {
                roadTypeID = Game.I.Session.Editor.RoadBrush.Current.ID;
            }
            else
            {
                roadTypeID = 0;
            }
            return new DrawRoadPathAction(h.Select(h => h.ID).ToList(), roadTypeID);
        };
        var roadBrush = new BoundaryBrush<RoadType>(roadStrokeFunc, roadStrokePathFunc, roadCond, true);
        return roadBrush;
    }

    public static BoundaryBrush<RiverType> GetRiverBrush()
    {
        Func<HexModel, bool> riverCond = (h) => h.Terrain.IsWater == false; 
        Func<List<HexModel>, IEditorAction> riverStrokeFunc = (h) => 
        {
            var h1 = h[0];
            var h2 = h[1];
            int riverTypeID;
            if(Game.I.Session.Editor.RiverBrush.Current != null)
            {
                riverTypeID = Game.I.Session.Editor.RiverBrush.Current.ID;
            }
            else
            {
                riverTypeID = 0;
            }
            return new DrawRiverAction(h1.ID, h2.ID, riverTypeID);
        };
        Func<List<HexModel>, IEditorAction> riverStrokePathFunc = (h) => 
        {
            int riverTypeID;
            if(Game.I.Session.Editor.RiverBrush.Current != null)
            {
                riverTypeID = Game.I.Session.Editor.RiverBrush.Current.ID;
            }
            else
            {
                riverTypeID = 0;
            }
            return new DrawRiverPathAction(h.Select(h => h.ID).ToList(), riverTypeID);
        };
        var riverBrush = new BoundaryBrush<RiverType>(riverStrokeFunc, riverStrokePathFunc, riverCond, false);
        return riverBrush;
    }
}
