using Dapper;
using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

public class Data : IData
{   
    public static readonly string DefaultGameDataPath = "gameData.db";

    public TerrainData Terrain { get; private set; }
    public LocationTypeData LocationTypes { get; private set; }
    public RoadTypeData RoadTypes { get; private set; }
    public RiverTypeData RiverTypes { get; private set; }
    public MovementTypeData MovementTypes { get; private set; }
    public UnitTypeData UnitTypes { get; private set; }
    public UnitRankData UnitRanks { get; private set; }
    public FormationTemplateData FormationTemplates { get; private set; }
    public UnitAbilityData UnitAbilities { get; private set; }
    public DefaultFactions DefaultFactions { get; private set; }
    public SessionConstants SessionConstants { get; private set; }
    private string _dataSrc = "Data Source=gamedata.db";
    public Data(string databasePath)
    {
        _dataSrc = "Data Source=" + databasePath;
    }

    public void Setup()
    {
        SessionConstants = LoadModels<SessionConstants>("Constants")[0];
        var terrains = LoadModels<TerrainModel>(TerrainModel.TableName);
        Terrain = new TerrainData(terrains.ToList());

        var locationTypes = LoadModels<LocationTypeModel>(LocationTypeModel.TableName);
        LocationTypes = new LocationTypeData(locationTypes);

        var roadTypes = LoadModels<RoadTypeModel>(RoadTypeModel.TableName);
        RoadTypes = new RoadTypeData(roadTypes);

        var riverTypes = LoadModels<RiverTypeModel>(RiverTypeModel.TableName);
        RiverTypes = new RiverTypeData(riverTypes);

        var movementTypes = LoadModels<MovementTypeModel>(MovementTypeModel.TableName);
        MovementTypes = new MovementTypeData(movementTypes);

        UnitAbilities = new UnitAbilityData();
        
        var unitTypes = LoadModels<UnitTypeModel>(UnitTypeModel.TableName);
        UnitTypes = new UnitTypeData(unitTypes);

        var formationTemplates = LoadModels<FormationTemplateModel>(FormationTemplateModel.TableName);
        FormationTemplates = new FormationTemplateData(formationTemplates);

        var unitRanks = LoadModels<UnitRankModel>(UnitRankModel.TableName);
        UnitRanks = new UnitRankData(unitRanks);

        var factions = LoadModels<FactionModel>(FactionModel.TableName);
        DefaultFactions = new DefaultFactions(factions);
    }

    private List<T> LoadModels<T>(string table) 
    {
        using (IDbConnection conn = new SQLiteConnection(_dataSrc))
        {
            var output = conn.Query<T>($"SELECT * FROM {table}", new DynamicParameters());
            return output.ToList();
        }
    }
    private T LoadModel<T>(string table, int id) 
    {
        using (IDbConnection conn = new SQLiteConnection(_dataSrc))
        {
            var output = conn.Query<T>($"SELECT * FROM {table}", new DynamicParameters());
            return output.ElementAt(id);
        }
    }
}
