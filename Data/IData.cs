using Godot;
using System;
using System.Collections.Generic;

public interface IData
{
    TerrainData Terrain {get;}
    LocationTypeData LocationTypes { get;}
    RoadTypeData RoadTypes {get;}
    RiverTypeData RiverTypes {get;}
    MovementTypeData MovementTypes {get;}
    UnitTypeData UnitTypes { get; }
    UnitRankData UnitRanks { get; }
    FormationTemplateData FormationTemplates { get; }
    UnitAbilityData UnitAbilities { get; }
    DefaultFactions DefaultFactions { get; }
    SessionConstants SessionConstants { get; }
    void Setup();
}
