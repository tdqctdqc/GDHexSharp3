using Godot;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class HostServer : Node, IServer
{
    private NetworkedMultiplayerENet _network; 
    private string _ip = "127.0.0.1";
    private int _port = 3306;
    private int _maxPlayers = 100;
    private Logic _logic;
    //private IState _state; 
    public StateLogicInterface StateInterface {get; private set;}
    public LogicInterface LogicInterface { get; private set; }
    //private ServerEventRouter _router;
    public void Setup()
    {
        DataBinding.Setup();
        //Game.I.Timers.SetTimer("creating state");

        // var connectionString = @$"server={_ip};port={_port.ToString()};userid=mysqldemo;password=Doot1234;SSL Mode=None";
        // _state = new State<MySqlConnection>(_ip, 
        //                                     _port, 
        //                                     "gameState", 
        //                                     true, 
        //                                     (s) => new MySqlConnection(s), 
        //                                     connectionString,
        //                                     new MySQLUtility());
        //Game.I.Timers.CallTimer("creating state");


        //_router = new ServerEventRouter(this);

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
        // var now = DateTime.Now;
        // int requesterID = GetTree().GetRpcSenderId();
        // if(requesterID == 0) LoadStatePush();
        // else RpcId(requesterID, nameof(LoadStatePush));
        // float seconds = DateTime.Now.Subtract(now).Seconds;
        // GD.Print("loading state time: " + seconds);
    }
    private void LoadStatePush()
    {
        // CacheManager.LoadCache();
    }
    public void BroadcastUpdateModels(string update)
    {
        // Rpc(nameof(ReceiveUpdateModels), update);
        // ReceiveUpdateModels(update);
    }
    [Remote] public void ReceiveUpdateModels(string update)
    {
        //_router.ReceiveModelsUpdateEvent(update);
    }
    public T LoadModel<T>(int id) where T : IModel, new()
    {
        return default(T);//_state.LoadModel<T>(id);
    }
    public List<T> LoadModels<T>(List<int> ids = null) where T : IModel, new()
    {
        return null;
        // if(ids == null) return _state.LoadModels<T>();
        // return _state.LoadModels<T>(ids);
    }
    [Remote] public void BroadcastDeleteModels(string update)
    {
        // Rpc(nameof(ReceiveUpdateModels), update);
        // ReceiveDeleteModels(update);
    }
    [Remote] public void ReceiveDeleteModels(string update)
    {
        //_router.ReceiveModelsDeleteEvent(update);
    }
    private void PeerConnected(int id)
    {
        GD.Print($"Peer {id} connected");
    }
    private void PeerDisconnected(int id)
    {
        GD.Print($"Peer {id} disconnected");
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
