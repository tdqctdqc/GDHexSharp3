using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class RiverTypeData 
{
    public Dictionary<int, RiverType> RiverTypes {get; private set;}
    public List<RiverType> RiverTypesByMinFlow { get; private set; }
    public RiverType this[int id] => RiverTypes[id];
    public RiverTypeData(List<RiverTypeModel> models)
    {
        RiverTypes = new Dictionary<int, RiverType>();
        foreach (var model in models)
        {
            RiverTypes.Add(model.ID, new RiverType(model));
        }
        RiverTypesByMinFlow = RiverTypes.Values.OrderBy(r => r.MinFlow).ToList();
    }
}
