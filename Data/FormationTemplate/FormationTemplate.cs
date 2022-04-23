using Godot;
using System;
using System.Collections.Generic;

public class FormationTemplate
{
    public int ID { get; private set; }
    public int Rank { get; private set; }
    public string Name { get; private set; }
    public List<Tuple<UnitType, int>> UnitsRanks { get; private set; }
    public FormationTemplate(FormationTemplateModel model)
    {
        ID = model.ID;
        Rank = model.Rank;
        Name = model.Name; 
        UnitsRanks = new List<Tuple<UnitType, int>>();
        var unitsRanks = model.Units;
        foreach (var item in unitsRanks)
        {
            var values = (Godot.Collections.Array)item;
            string unitTypeName = (string)values[0];
            UnitType unitType = Game.I.Session.Data.UnitTypes[unitTypeName];
            if(unitType == null)
            {
                throw new ArgumentException($"{unitTypeName} is not a valid unit type name");
            }
            int rank = (int)values[1];
            int num = (int)values[2];
            for (int i = 0; i < num; i++)
            {
                var tuple = new Tuple<UnitType, int>(unitType, rank);
                UnitsRanks.Add(tuple);
            }
        }
    }
}
