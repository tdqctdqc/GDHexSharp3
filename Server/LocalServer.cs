using Godot;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Linq;

public class LocalServer : Node, IServer
{
    private NetworkedMultiplayerENet _network; 
    private string _ip = "127.0.0.1";
    private int _port = 3306;
    private int _maxPlayers = 100;
    public Logic _logic;
    //private IState _state; 
    public StateLogicInterface StateInterface {get; private set;}
    public LogicInterface LogicInterface { get; private set; }
    //private ServerEventRouter _router;
    public void Setup()
    {
        DataBinding.Setup();

        var connectionString = @$"Data Source=localgamestate.db;";
        StateInterface = new StateLogicInterface();
        _logic = new Logic(StateInterface);
        LogicInterface = new LogicInterface(_logic);
        _network = new NetworkedMultiplayerENet();
        _network.CreateServer(_port, _maxPlayers);
        GetTree().NetworkPeer = _network;
        _network.Connect("peer_connected", this, nameof(PeerConnected));
        _network.Connect("peer_disconnected", this, nameof(PeerDisconnected));
    }

    public void StartServer()
    {
        _logic.BuildNewGameSession();
    }

    [Remote] public void LoadStateRequest()
    {
        int requesterID = GetTree().GetRpcSenderId();
        if(requesterID == 0) LoadStatePush();
        else RpcId(requesterID, nameof(LoadStatePush));
    }
    private void LoadStatePush()
    {
        CacheManager.LoadCache();
    }
    public void BroadcastUpdateModels(string table, List<BackingModel> backings)
    {
        Rpc(nameof(ReceiveUpdateModels), 
            table, 
            backings.Select(b => b.ID).ToList(), 
            backings.Select(b => b.Fields).ToList());
        // ReceiveUpdateModels(update);
    }
    [Remote] public void ReceiveUpdateModels(string table, List<int> ids, 
                                            List<string[]> fieldsList)
    {
        CacheManager.SyncFuncs[table](ids, fieldsList);
        // _router.ReceiveModelsUpdateEvent(update);
    }

    [Remote] public void BroadcastDeleteModels(string update)
    {
        //Rpc(nameof(ReceiveUpdateModels), update);
        //ReceiveDeleteModels(update);
    }
    [Remote] public void ReceiveDeleteModels(string update)
    {
        // _router.ReceiveModelsDeleteEvent(update);
    }
    public List<T> LoadModels<T>(List<int> ids = null) where T : IModel, new()
    {
        return null;
        // if(ids == null) return _state.LoadModels<T>();
        // return _state.LoadModels<T>(ids);
    }
    public T LoadModel<T>(int id) where T : IModel, new()
    {
        return default(T);
        // return _state.LoadModel<T>(id);
    }
    private void PeerConnected(int id)
    {
        GD.Print($"Peer {id} connected");
    }
    private void PeerDisconnected(int id)
    {
        GD.Print($"Peer {id} disconnected");
    }
}
