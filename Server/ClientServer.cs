using Godot;
using System;
using System.Collections.Generic;

public class ClientServer : IServer
{
    public StateLogicInterface StateInterface {get; private set;}

    public Logic Logic => throw new NotImplementedException();

    public LogicInterface LogicInterface => throw new NotImplementedException();

    public void Setup()
    {

    }
    public void StartServer()
    {
        
    }
    public void LoadStateRequest()
    {
        
    }
    public void BroadcastUpdateModels(string update)
    {
        
    }
    public void ReceiveUpdateModels(string update)
    {
        
    }
    public void BroadcastModelStateChangeRequest(string stateChangeString)
    {
        
    }
    public void ReceiveModelStateChangeRequest(string stateChangeString)
    {
        
    }
    public void BroadcastDeleteModels(string update)
    {

    }
    public void ReceiveDeleteModels(string update)
    {

    }
    public List<T> LoadModels<T>(List<int> ids = null) where T : IModel, new()
    {
        return null; 
    }
    public T LoadModel<T>(int id) where T : IModel, new()
    {
        return default(T); 
    }

    public List<T> LoadModelsByValue<T>(string columnName, int value) where T : IModel, new()
    {
        return null;
    }
    public List<T> LoadModelsByValue<T>(string columnName, string value) where T : IModel, new()
    {
        return null;
    }

    public void BroadcastUpdateModels(string table, List<BackingModel> backings)
    {
        throw new NotImplementedException();
    }

    public void ReceiveUpdateModels(string table, List<int> ids, List<string[]> fieldsList)
    {
        throw new NotImplementedException();
    }
}
