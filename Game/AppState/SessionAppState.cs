using Godot;
using System;

public class SessionAppState : IAppState
{
    private ISession _session;
    private string _serverType;
    private string _gameDataPath; 
    private GameGenerationParameters _parameters; 
    public SessionAppState(string serverType, string gameDataPath, GameGenerationParameters parameters)
    {
        _parameters = parameters;
        _serverType = serverType;
        _gameDataPath = gameDataPath;

        
    }
    public void Enter()
    {
        _session.AwakeClient();
    }

    public void Build()
    {
        var s = new Session(_serverType, _gameDataPath, _parameters);
        _session = s;
        s.Name = "Session";
        Game.I.AddChild(s);

        Game.I.SetSession(_session);
        s.Setup();
    }

    public void Exit()
    {
       ((Node)_session).QueueFree();
    }
}
