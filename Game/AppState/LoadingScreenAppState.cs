using Godot;
using System;
using System.Threading.Tasks;

public class LoadingScreenAppState : IAppState
{
    private LoadingScreen _loadingScreen; 
    private SessionAppState _sessionState; 
    public LoadingScreenAppState(SessionAppState sessionState)
    {
        _sessionState = sessionState;
    }
    public async void Enter()
    {
        await Task.Run(_sessionState.Build);
        
        Game.I.AppStateController.SetState(_sessionState);
    }
    public void Exit()
    {
    }
}
