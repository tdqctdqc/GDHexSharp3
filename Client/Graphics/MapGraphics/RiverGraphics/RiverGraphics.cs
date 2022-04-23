using Godot;
using System;
using System.Collections.Generic;

public class RiverGraphics : Node2D
{
    private Dictionary<int, RiverGraphic> _graphics; 
    public override void _Ready()
    {
        _graphics = new Dictionary<int, RiverGraphic>();
    }

    public void Setup()
    {
        CacheManager.LoadedState += LoadState;
    }

    public void LoadState()
    {
        var rivers = Cache<RiverModel>.GetModels();
        foreach (var river in rivers)
        {
            var graphic = new RiverGraphic();
            graphic.Setup(river);
            graphic.Position = river.Position;
            AddChild(graphic);
            _graphics.Add(river.ID, graphic);
        }

        Cache<RiverModel>.ModelsAdded += ModelsChanged;
        Cache<RiverModel>.ModelsChanged += ModelsChanged;
        Cache<RiverModel>.ModelsDeleted += ModelsDeleted;
    }

    public void ModelsChanged(List<RiverModel> riverModels)
    {
        foreach (var model in riverModels)
        {
            if(_graphics.ContainsKey(model.ID) == false)
            {
                var newGraphic = new RiverGraphic();
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
