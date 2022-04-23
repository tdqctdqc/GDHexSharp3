using Godot;
using System;
using System.Collections.Generic;

public class UIUnitHolder : Control
{
    private UIUnit _u1, _u2, _u3, _u4, _u5, _u6, _u7;
    private List<UIUnit> _units;
    public override void _Ready()
    {
        _u1 = GetNode<UIUnit>("U1");
        _u2 = GetNode<UIUnit>("U2");
        _u3 = GetNode<UIUnit>("U3");
        _u4 = GetNode<UIUnit>("U4");
        _u5 = GetNode<UIUnit>("U5");
        _u6 = GetNode<UIUnit>("U6");
        _u7 = GetNode<UIUnit>("U7");
        _units = new List<UIUnit>(){_u1,_u2,_u3,_u4,_u5,_u6,_u7};
        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].RectPosition = GraphicsUtility.UnitIconOffsets[7][i];
        }
        Clear();
    }
    public void Setup(List<UnitModel> models)
    {
        Clear();
        int count = models.Count;
        if(count > 7) return; 
        var offsets = GraphicsUtility.UnitIconOffsets[count];
        for (int i = 0; i < models.Count; i++)
        {
            var unit = _units[i];
            unit.Visible = true;
            unit.Setup(models[i], null, null, true);
            unit.RectPosition = offsets[i];
        }
    }
    public void SetupMock(List<UnitModel> models, Action<UnitModel> select, Action<UnitModel> deselect, Color prim, Color sec)
    {
        Clear();
        int count = models.Count;
        if(count > 7) return; 
        var offsets = GraphicsUtility.UnitIconOffsets[count];
        for (int i = 0; i < models.Count; i++)
        {
            var unit = _units[i];
            var model = models[i];
            unit.Visible = true;
            unit.SetupMock(models[i], prim, sec, () => select(model), () => deselect(model), true);
            unit.RectPosition = offsets[i];
        }
    }
    private void Clear()
    {
        _units.ForEach(u => u.Visible = false);
    }
}
