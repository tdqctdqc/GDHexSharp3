using Godot;
using System;

public class MainMenuAppState : IAppState
{
    private MainMenu _menu; 
    public void Enter()
    {
        _menu = Scenes.MainMenu;
        Game.I.AddChild(_menu);
    }

    public void Exit()
    {
        Game.I.RemoveChild(_menu);
        _menu.QueueFree();
    }
}
