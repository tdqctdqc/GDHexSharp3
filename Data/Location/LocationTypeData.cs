using Godot;
using System;
using System.Collections.Generic;

public class LocationTypeData 
{
    public Dictionary<int, LocationType> LocationTypes {get; private set;}
    public LocationType this[int id] => LocationTypes[id];
    public LocationTypeData(List<LocationTypeModel> models)
    {
        LocationTypes = new Dictionary<int, LocationType>();
        foreach (var model in models)
        {
            LocationTypes.Add(model.ID, new LocationType(model));
        }
    }
}
