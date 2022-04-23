using Godot;
using System;
using System.Collections.Generic;

public class RoadTypeData 
{
    public Dictionary<int, RoadType> RoadTypes {get; private set;}
    public RoadType this[int id] => RoadTypes[id];
    public RoadTypeData(List<RoadTypeModel> models)
    {
        RoadTypes = new Dictionary<int, RoadType>();
        foreach (var model in models)
        {
            RoadTypes.Add(model.ID, new RoadType(model));
        }
    }
}
