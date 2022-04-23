using Godot;
using System;

public class UIUnit : Control
{
    private UnitGraphic _graphic;
    private bool _selectable, _selected, _mouseOver; 
    private Action _selectAction, _deselectAction; 
    public override void _Ready()
    {   
        Connect("mouse_entered", this, nameof(MouseEnter));
        Connect("mouse_exited", this, nameof(MouseExit));
        _graphic = GetNode<UnitGraphic>("UnitGraphic");
    }
    public override void _Process(float delta)
    {
        if(Input.IsActionJustReleased("leftClick") && _mouseOver)
        {
            Click();
        }
    }
    public void Setup(UnitModel model, Action selectAction = null, Action deselectAction = null, bool selectable = false)
    {
        _graphic.Setup(model);
        _selectAction = selectAction;
        _deselectAction = deselectAction;
        _selectable = selectable;
    }
    public void SetupMock(UnitModel model, Color prim, Color sec, Action selectAction = null, Action deselectAction = null, bool selectable = false)
    {
        _graphic.SetupMock(model, prim, sec);
        _selectAction = selectAction;
        _deselectAction = deselectAction;
        _selectable = selectable;
    }
    public void Select()
    {
        _selected = true; 
        if(_selectable)
        {
            _graphic.Select();
            _selectAction?.Invoke();
        }
    }
    public void Deselect()
    {
        _selected = false; 

        if(_selectable)
        {
            _graphic.Deselect();
            _deselectAction?.Invoke();
        }
    }
    public void MouseEnter()
    {
        _mouseOver = true;
    }
    public void MouseExit()
    {
        _mouseOver = false;
    }
    public void Click()
    {
        if(_selectable)
        {
            if(_selected == false) Select();
            else Deselect();
        }
    }
}
