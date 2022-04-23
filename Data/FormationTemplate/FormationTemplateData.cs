using Godot;
using System;
using System.Collections.Generic;

public class FormationTemplateData 
{
    public Dictionary<int, FormationTemplate> MovementTypes { get; private set; }
    public FormationTemplate this[int id] => MovementTypes[id];

    public FormationTemplateData(List<FormationTemplateModel> models)
    {
        MovementTypes = new Dictionary<int, FormationTemplate>(); 
        foreach (var model in models)
        {
            MovementTypes.Add(model.ID, new FormationTemplate(model));
        }
    }
}
