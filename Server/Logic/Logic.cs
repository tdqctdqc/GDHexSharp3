using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class Logic
{
    public RandomNumberGenerator Random { get; private set; }
    private StateLogicInterface _interface;
    public TurnManager TurnManager { get; private set; }
    public UnitLogic Unit { get; private set; }
    public FactionLogic Faction { get; private set; }
    public ProductionLogic Production { get; private set; }
    public SupplyLogic Supply { get; private set; }
    public CombatLogic Combat { get; private set; }
    public RoadLogic Road { get; private set; }
    public RiverLogic River { get; private set; }
    public FormationLogic Formation { get; private set; }
    public AILogic AI { get; private set; }
    public Logic(StateLogicInterface intrface)
    {
        Random = new RandomNumberGenerator();
        Unit = new UnitLogic(this);
        Faction = new FactionLogic();
        Production = new ProductionLogic();
        Supply = new SupplyLogic(this);
        Combat = new CombatLogic(this);
        _interface = intrface;
        TurnManager = new TurnManager(this);
        Road = new RoadLogic(this);
        River = new RiverLogic(this);
        Formation = new FormationLogic();
        UnitGenerator.Setup(this);
        AI = new AILogic();
    }
    public void BuildNewGameSession()
    {
        var sw = new System.Diagnostics.Stopwatch();

        sw.Start();
        var factions = FactionGenerator.GenerateFactions();
        sw.Stop();
        GD.Print($"faction gen time: {sw.Elapsed}");
        sw.Reset();
        
        sw.Start();
        var hexes = MapGenerator.GenerateMap(Game.I.Session.Params, factions);
        sw.Stop();
        GD.Print($"map gen time: {sw.Elapsed}");
        sw.Reset();
        
        sw.Start();
        var locs = LocationGenerator.GenerateLocations(hexes);
        sw.Stop();
        GD.Print($"loc gen time: {sw.Elapsed}");
        sw.Reset();
        
        sw.Start();
        var units = UnitGenerator.GenerateUnits(factions, hexes);
        sw.Stop();
        GD.Print($"unit gen time: {sw.Elapsed}");
        sw.Reset();
        
        sw.Start();
        RoadGenerator.GenerateRoadsAggreg(this, locs);
        sw.Stop();
        GD.Print($"road gen time: {sw.Elapsed}");
        sw.Reset();

        sw.Start();

        //CacheManager.Orders.AddOrdersForUnits(units);
        sw.Stop();
        GD.Print($"order gen time: {sw.Elapsed}");
        sw.Reset();

        AI.Setup();
        
        TurnManager.StartTurn();
    }
}
