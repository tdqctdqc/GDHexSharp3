using Godot;
using System;
using System.Collections.Generic;

public interface IServer 
{
    StateLogicInterface StateInterface {get;}
    LogicInterface LogicInterface {get;}
    void Setup();
    void StartServer();
    void LoadStateRequest();
    void BroadcastUpdateModels(string table, List<BackingModel> backings);
    void ReceiveUpdateModels(string table, List<int> ids, 
                                            List<string[]> fieldsList);
    void BroadcastDeleteModels(string update);
    void ReceiveDeleteModels(string update);
    List<T> LoadModels<T>(List<int> ids = null) where T : IModel, new();
    T LoadModel<T>(int id) where T : IModel, new();
}
