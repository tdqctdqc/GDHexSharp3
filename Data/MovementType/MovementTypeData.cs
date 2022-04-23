using Godot;
using System;
using System.Collections.Generic;

public class MovementTypeData
{
    public Dictionary<int, MovementType> MovementTypes { get; private set; }
    private Dictionary<string, MovementType> _byName;
    public MovementType this[int id] => MovementTypes[id];
    public MovementType this[string name] => GetMovementTypeByName(name);

    public MovementTypeData(List<MovementTypeModel> models)
    {
        MovementTypes = new Dictionary<int, MovementType>(); 
        _byName = new Dictionary<string, MovementType>();
        foreach (var model in models)
        {
            var type =  new MovementType(model);
            MovementTypes.Add(model.ID, type);
            _byName.Add(model.Name, type);
        }
    }

    private MovementType GetMovementTypeByName(string name)
    {
        if(_byName.ContainsKey(name)) return _byName[name];
        return null;
    }
}
