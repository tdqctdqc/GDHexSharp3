using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;
public class GoToOrder : IOrder
{
    public string Name => _name;
    private static string _name = "GoTo";
    public int ID { get; private set; }
    private List<int> _path; 
    public List<HexModel> Path => Cache<HexModel>.GetModels(_path);
    public HexModel Destination => Cache<HexModel>.GetModel(_path.Last());
    public UnitModel Unit => Cache<UnitModel>.GetModel(ID);
    public bool Completed => CheckCompletion();
    private float _storedAP; 
    public GoToOrder(){}
    public GoToOrder(List<int> path, int unitID)
    {
        _storedAP = 0f;
        _path = new List<int>(path);
        ID = unitID; 
    }
    private bool CheckCompletion()
    {
        if(_path.Count == 1) return true; 
        if(Unit.Hex.ID == _path.Last()) return true; 
        else return false; 
    }
    public void Do(float ap, Logic logic)
    {
        if(Completed) return; 
        _storedAP += ap; 
        logic.Unit.UnitDoMoveLogic(Unit, _path, ref _storedAP);
    }
    public void AddToPath(int newHexID)
    {
        if(_path.Count > 0)
        {
            if(_path.Last().GetNeighborIDs().Contains(newHexID) == false)
            return; 
        }
        _path.Add(newHexID);
    }
    public void AddToPath(List<int> newHexID)
    {
        if(newHexID == null || newHexID.Count == 0) return; 
        if(_path.Count > 0)
        {
            if(_path.Last().GetNeighborIDs().Contains(newHexID.First()) == false)
            return; 
        }
        _path.AddRange(newHexID);
    }
}
