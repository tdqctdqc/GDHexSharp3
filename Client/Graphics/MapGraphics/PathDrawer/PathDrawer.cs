using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class PathDrawer : Node2D
{
    private Task _findingMovementLineTask;
    private CancellableTask _movementLineTask; 
    private CancellationTokenSource _token;
    private PathMesh _movement, _plannedMove, _defend, _defendLine, _attack; 

    public override void _Ready()
    {
        _movement = new PathMesh();
        AddChild(_movement);
        _plannedMove = new PathMesh();
        AddChild(_plannedMove);

        _movement.Setup(Colors.Yellow);

        _plannedMove.Setup(Colors.Purple);  

        _defendLine = new PathMesh();
        AddChild(_defendLine);
        _defendLine.Setup(Colors.Black);

        _defend = new PathMesh();
        AddChild(_defend);
        _defend.Setup(Colors.Black);

        _attack = new PathMesh();
        AddChild(_attack);
        _attack.Setup(Colors.Red);

        _movementLineTask = new CancellableTask();

        ZAsRelative = true; 
        _defendLine.ZIndex = 1;
        _attack.ZIndex = 2;
        _plannedMove.ZIndex = 3;
        _movement.ZIndex = 4;
    }
    public void Setup()
    {
        Game.I.Session.Client.Events.UI.SelectedUnits += DrawPlannedMovePaths;
        Game.I.Session.Client.Events.UI.SelectedUnits += DrawDefenseLine;
        Game.I.Session.Client.Events.UI.SelectedUnits += DrawAttackAxis;
        Game.I.Session.Client.Events.Order.OrdersUpdated += () => DrawPlannedMovePaths(Game.I.Session.Client.UI.UnitSelector.CurrentList);
    }
    public void ClearPaths()
    {
        _movement.Clear();
        _plannedMove.Clear();
        _defendLine.Clear();
        _defend.Clear();
        _attack.Clear();
    }
    public void DrawUnitMovePath()
    {
        var units = Game.I.Session.Client.UI.UnitSelector.CurrentList;
        if(units.Count == 0)
        {
            _movement.Clear();
            return;
        }
        var mouseOverHex = Game.I.Session.Client.UI.HexSelector.MouseOverHex;
        if(mouseOverHex == null) return; 
        var selectedHex = Game.I.Session.Client.UI.HexSelector.SelectedHex;
        
        Action drawAction = () => DrawMovementLine(selectedHex, mouseOverHex, units);
        _movementLineTask.SetTask(drawAction, _movement.Clear);
    }
    private void DrawMovementLine(HexModel selectedHex, HexModel mouseOverHex, List<UnitModel> units)
    {
        var pathFinder = Game.I.Session.Utility.PathFinder;
        var paths = new List<List<HexModel>>();
        foreach (var unit in units)
        {
            var path = pathFinder.FindUnitPath(unit, unit.Hex, mouseOverHex);
            if(path == null || path.Count <= 1)
            {
                _movement.Clear();
                return;
            }
            else if(path.Count > 1)
            {
                paths.Add(path);
            }
        }
        _movement.DrawPathsArrow(paths);
    }

    public void DrawPlannedMovePaths(List<UnitModel> units)
    {
        _plannedMove.Clear();
        var goPaths = new List<List<HexModel>>();
        var defPaths = new List<List<HexModel>>();
        foreach (var unit in units)
        {
            var order = unit.Order;
            if(order is GoToOrder)
            {
                var goTo = (GoToOrder)order;
                goPaths.Add(goTo.Path);
            }
            else if (order is DefendOrder)
            {
                var def = (DefendOrder)order;
                defPaths.Add(def.Path);
            }
        }
        _plannedMove.DrawPathsArrow(goPaths);
        _defend.DrawPathsArrow(defPaths);
    }
    public void DrawDefenseLine(List<UnitModel> units)
    {
        var formations = units.Select(u => u.Formation).ToHashSet().ToList();
        if(formations == null) return;
        var paths = formations.Where(f => f != null).Where(f => f.DefenseLine != null).Select(f => f.DefenseLine).ToList();
        _defendLine.DrawPathsDef(paths);
    }
    public void DrawAttackAxis(List<UnitModel> units)
    {
        var formations = units.Select(u => u.Formation).ToHashSet().ToList();
        if(formations == null) return;
        var paths = formations.Where(f => f != null).Where(f => f.AttackAxis != null).Select(f => f.AttackAxis).ToList();
        _attack.DrawPathsArrow(paths);
    }
}