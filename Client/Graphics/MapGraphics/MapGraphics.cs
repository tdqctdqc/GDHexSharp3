using Godot;
using System;
using System.Threading.Tasks;

public class MapGraphics : Node2D
{
    public BaseMesh _baseMesh;
    private FactionMesh _factionMesh;
    private HexSelectorGraphics _hexSelectorGraphics; 
    private LocationGraphics _locationGraphics; 
    private RoadGraphics _roadGraphics; 
    private RiverGraphics _riverGraphics; 
    public UnitGraphics UnitGraphics { get; private set; }
    public PathDrawer PathDrawer { get; private set; }
    public ReplayGraphics ReplayGraphics { get; private set; }
    private FormationMapDisplay _formationDisplay; 
    public HexHighlighter HexHighlighter { get; private set; }
    public HexBorderHighlighter HexBorderHighlighter { get; private set; }
    public MapHighlightManager MapHighlightManager { get; private set; }
    public MapGraphics()
    {
        MapHighlightManager = new MapHighlightManager();

        _baseMesh = new BaseMesh((h) => Game.I.Session.Data.Terrain[h.TerrainID].BaseColor);
        AddChild(_baseMesh);
        _factionMesh = new FactionMesh();
        AddChild(_factionMesh);
        _hexSelectorGraphics = new HexSelectorGraphics();
        AddChild(_hexSelectorGraphics);
        _locationGraphics = new LocationGraphics();
        AddChild(_locationGraphics);
        _riverGraphics = new RiverGraphics();
        AddChild(_riverGraphics);
        _roadGraphics = new RoadGraphics();
        AddChild(_roadGraphics);

        

        UnitGraphics = new UnitGraphics();
        AddChild(UnitGraphics);
        PathDrawer = new PathDrawer();
        AddChild(PathDrawer);
        ReplayGraphics = new ReplayGraphics();
        ReplayGraphics.Visible = false; 
        AddChild(ReplayGraphics);

        HexHighlighter = new HexHighlighter();
        AddChild(HexHighlighter);
        HexBorderHighlighter = new HexBorderHighlighter();
        AddChild(HexBorderHighlighter);
        _formationDisplay = new FormationMapDisplay();
        AddChild(_formationDisplay);

        ZAsRelative = true; 
        _baseMesh.ZIndex = 1;
        _factionMesh.ZIndex = 2;
        
        _riverGraphics.ZIndex = 3;
        _roadGraphics.ZIndex = 4;
        _locationGraphics.ZIndex = 5;
        HexHighlighter.ZIndex = 6; 
        HexBorderHighlighter.ZIndex = 6; 
        _formationDisplay.ZIndex = 6;

        UnitGraphics.ZIndex = 7;
        _hexSelectorGraphics.ZIndex = 8; 
        PathDrawer.ZIndex = 9;

        ReplayGraphics.ZIndex = 10; 

        
    }
    public void Setup()
    {
        CacheManager.LoadedState += LoadState;
        _hexSelectorGraphics.Setup();
        _factionMesh.Setup();
        _locationGraphics.Setup();
        _riverGraphics.Setup();

        _roadGraphics.Setup();
        UnitGraphics.Setup();
        ReplayGraphics.Setup();
        _formationDisplay.Setup();
        PathDrawer.Setup();

    }

    public void LoadState()
    {
        var hexes = Cache<HexModel>.GetModels();
        
        HexHighlighter.Setup(hexes);
        HexBorderHighlighter.Setup(hexes);
        _baseMesh.Setup(hexes);
    }
    public void ShowReplay()
    {
        ReplayGraphics.ShowReplay();
        _factionMesh.Visible = false; 
        UnitGraphics.Visible = false;
    }
    public void HideReplay()
    {
        ReplayGraphics.HideReplay();
        _factionMesh.Visible = true; 
        UnitGraphics.Visible = true;
    }

    public void SetBaseMeshColorFunc(Func<HexModel, Color> colorFunc)
    {
        _baseMesh.SetColorFunc(colorFunc);
    }
}
