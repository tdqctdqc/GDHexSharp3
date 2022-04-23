using Godot;
using System;
using System.Collections.Generic;

public class RoadGraphics : Node2D
{
    private Dictionary<int, RoadGraphic> _graphics; 
    public override void _Ready()
    {
        _graphics = new Dictionary<int, RoadGraphic>();
    }

    public void Setup()
    {
        CacheManager.LoadedState += LoadState;
    }

    public void LoadState()
    {
        var roads = Cache<RoadModel>.GetModels();
        foreach (var road in roads)
        {
            var graphic = new RoadGraphic();
            graphic.Setup(road);
            graphic.Position = road.Position;
            AddChild(graphic);
            _graphics.Add(road.ID, graphic);
        }

        Cache<RoadModel>.ModelsAdded += ModelsChanged;
        Cache<RoadModel>.ModelsChanged += ModelsChanged;
        Cache<RoadModel>.ModelsDeleted += ModelsDeleted;
    }

    public void ModelsChanged(List<RoadModel> roadModels)
    {
        foreach (var model in roadModels)
        {
            if(_graphics.ContainsKey(model.ID) == false)
            {
                var newGraphic = new RoadGraphic();
                AddChild(newGraphic);

                _graphics.Add(model.ID, newGraphic);
            }
            var graphic = _graphics[model.ID];
            graphic.Setup(model);
            graphic.Position = model.Position;
        }
    }

    public void ModelsDeleted(List<int> modelIDs)
    {
        foreach (var id in modelIDs)
        {
            if(_graphics.ContainsKey(id) == false) continue; 
            _graphics[id].Free();
            _graphics.Remove(id);
        }
    }
}
