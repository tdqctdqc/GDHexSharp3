using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class UnitTypeData 
{
    public Dictionary<int, UnitType> UnitTypes {get; private set;}
    public List<UnitType> List {get; private set;}
    public UnitType this[int id] => UnitTypes[id];
    public UnitType this[string name] => UnitTypes.Where(e => e.Value.Name == name).FirstOrDefault().Value;
    public UnitTypeData(List<UnitTypeModel> models)
    {
        UnitTypes = new Dictionary<int, UnitType>();
        List = new List<UnitType>();
        foreach (var model in models)
        {
            var type = new UnitType(model);
            UnitTypes.Add(type.ID, type);
            List.Add(type);
        }
    }
}
