using Godot;
using System;
using System.Collections.Generic;

public class LocationGraphics : Node2D
{
    private Dictionary<int, LocationGraphic> _graphics; 
    public override void _Ready()
    {
        _graphics = new Dictionary<int, LocationGraphic>();
    }

    public void Setup()
    {
        CacheManager.LoadedState += LoadState;
    }

    public void LoadState()
    {
        var locations = Cache<LocationModel>.GetModels();
        foreach (var location in locations)
        {
            var graphic = new LocationGraphic();
            graphic.Setup(location);
            graphic.Position = location.Hex.WorldPos;
            AddChild(graphic);
            _graphics.Add(location.ID, graphic);
        }
        Cache<LocationModel>.ModelsChanged += ModelsChanged;
        Cache<LocationModel>.ModelsAdded += ModelsChanged;
        Cache<LocationModel>.ModelsDeleted += ModelsDeleted;
    }

    public void ModelsChanged(List<LocationModel> models)
    {
        foreach (var model in models)
        {
            if(_graphics.ContainsKey(model.ID) == false)
            {
                var newGraphic = new LocationGraphic();
                AddChild(newGraphic);
                _graphics.Add(model.ID, newGraphic);
            }
            var graphic = _graphics[model.ID];
            graphic.Setup(model);
            graphic.Position = model.Hex.WorldPos;
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
