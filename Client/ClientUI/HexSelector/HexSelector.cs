using Godot;
using HexWargame;
using System;
using System.Collections.Generic;

public class HexSelector
{
    public int MouseOverHexID { get; private set; }
    public int SecondClosestHexID { get; private set; }
    public int SelectedHexID { get; private set; }
    public HexModel SelectedHex => Cache<HexModel>.GetModel(SelectedHexID);
    public HexModel MouseOverHex => Cache<HexModel>.GetModel(MouseOverHexID);
    public HexModel SecondClosestHex => Cache<HexModel>.GetModel(SecondClosestHexID);
    private CancellableTask _mouseOver, _secondClosest;
    public HexSelector()
    {
        MouseOverHexID = 0;
        SecondClosestHexID = 0;
        SelectedHexID = 0;
        _mouseOver = new CancellableTask();
        _secondClosest = new CancellableTask();
    }

    public void Setup()
    { 
        Game.I.Session.Client.UI.Ticker.Tick += Tick; 
    }
    public void Tick()
    {
        _mouseOver.SetTask(FindMouseOverHexTask);
    }

    private void FindMouseOverHexTask()
    {
        Vector2 mousePos = Game.I.Session.Client.Graphics.GetGlobalMousePosition();
        var result = mousePos.FindTwoClosestHexes();
        
        var mouseOverHexModel = result.Item1;//FindHexFromWorldPos(mousePos);
        if(mouseOverHexModel == null) return; 
        if(mouseOverHexModel != null) 
        {
            if(mouseOverHexModel.ID != MouseOverHexID) 
            {
                MouseOverHexID = mouseOverHexModel.ID;
                Game.I.Session.Client.Events.UI.MouseOverHexChanged?.Invoke(mouseOverHexModel);
            }
        }
        var secondClosestHexModel = result.Item2;//FindSecondClosestHexFromWorldPos(mousePos, MouseOverHexID);
        if(secondClosestHexModel != null)
        {
            if(secondClosestHexModel.ID != SecondClosestHexID) 
            {
                SecondClosestHexID = secondClosestHexModel.ID;
                Game.I.Session.Client.Events.UI.SecondClosestHexChanged?.Invoke(secondClosestHexModel);
            }
        }
    }
    public void TrySelectHex()
    {
        Vector2 mousePos = Game.I.Session.Client.Graphics.GetGlobalMousePosition();
        var selectedHexModel = mousePos.FindHexFromWorldPos();
        if(selectedHexModel == null) return;
        var old = Cache<HexModel>.GetModel(SelectedHexID);
        SelectedHexID = selectedHexModel.ID;
        Game.I.Session.Client.Events.UI.SelectedHexChanged?.Invoke(selectedHexModel);
    }
}
