using Godot;
using System;
using System.Collections.Generic;

public class Session : Node, ISession
{
    public IServer Server { get; private set; }
    public IClient Client { get; private set; }
    public IData Data { get; private set; }
    public IEditor Editor { get; private set; }
    public IUtility Utility { get; private set; }
    public GameGenerationParameters Params {get; private set;}
    public MapGenPackage MapGenPackage { get; set; }
    public static readonly string HostString = "host";
    public static readonly string ClientString = "client";
    public static readonly string LocalString = "local";
    private Dictionary<string, Func<IServer>> _serverSetupDic;

    public Session(string serverType, string gameDataPath, GameGenerationParameters parameters)
    {
        Params = parameters;
        GD.Print(serverType);
        Data = new Data(gameDataPath);
        SetupDic();
        Server = _serverSetupDic[serverType].Invoke();
        ((Node)Server).Name = "Server";
        AddChild((Node)Server);

        var c = new Client();
        Client = c;
        c.Name = "Client";
        //AddChild(c);

        Editor = new Editor();
        Utility = new Utility();
    }

    public void Setup()
    {
        var sw = new System.Diagnostics.Stopwatch();
        var sw2 = new System.Diagnostics.Stopwatch();
        sw.Start();

        
        Data.Setup();

        Server.Setup();
        Server.StartServer();
        
        Editor.Setup();
        //AwakeClient();
        sw.Stop();
        GD.Print("session setup time: " + sw.Elapsed);
    }
    public void AwakeClient()
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        AddChild((Node)Client);
    
        Client.Setup();

        sw.Stop();
        GD.Print("client setup time: " + sw.Elapsed);
    }
    private void SetupDic()
    {
        _serverSetupDic = new Dictionary<string, Func<IServer>>()
                                {   
                                    {HostString, SetupHostServer},
                                    {LocalString, SetupLocalServer},
                                    {ClientString, SetupClientServer}
                                };
    }
    private IServer SetupLocalServer()
    {
        var server = new LocalServer();
        
        return server; 
    }
    private IServer SetupHostServer()
    {
        var server = new HostServer();

        return server; 
    }
    private IServer SetupClientServer()
    {
        var server = new ClientServer();

        return server; 
    }
}
