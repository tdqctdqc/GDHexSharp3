using Godot;
using System;
using System.Threading.Tasks;

public class MainMenu : Node
{
    private FuncButton _newLocal, _loadLocal, _host, _join;
    private GameGenerationParameters _defaultParams; 
    private GameParameterSetupScreen _setupScreen; 
    public override void _Ready()
    {
        _setupScreen = Scenes.GameParameterSetupScreen;
        AddChild(_setupScreen);
        _newLocal = GetNode<FuncButton>("VBox/NewLocalGame");
        _newLocal.Set( () => OpenSetupScreen(CreateNewLocalGame) );
        _loadLocal = GetNode<FuncButton>("VBox/LoadLocalGame");
        _loadLocal.Set(LoadLocalGame);
        _host = GetNode<FuncButton>("VBox/HostGame");
        _host.Set( () => OpenSetupScreen(HostGame) );
        _join = GetNode<FuncButton>("VBox/JoinGame");
        _join.Set(JoinGame);
    }

    private void OpenSetupScreen(Action<GameGenerationParameters> action)
    {
        _setupScreen.Setup(action);
        _setupScreen.Popup_();
    }

    private void CreateNewLocalGame(GameGenerationParameters prms)
    {
        var sessionState = new SessionAppState(Session.LocalString, Data.DefaultGameDataPath, prms);
        var loadingScreenState = new LoadingScreenAppState(sessionState);
        Game.I.AppStateController.SetState(loadingScreenState);
    }

    private void LoadLocalGame()
    {

    }

    private void HostGame(GameGenerationParameters prms)
    {
        var sessionState = new SessionAppState(Session.HostString, Data.DefaultGameDataPath, prms);
        var loadingScreenState = new LoadingScreenAppState(sessionState);
        Game.I.AppStateController.SetState(loadingScreenState);
    }

    private void JoinGame()
    {
        var sessionState = new SessionAppState(Session.ClientString, 
                                                Data.DefaultGameDataPath, 
                                                GameGenerationParameters.GetDefaultParams());
        var loadingScreenState = new LoadingScreenAppState(sessionState);
        Game.I.AppStateController.SetState(loadingScreenState);
    }
}
