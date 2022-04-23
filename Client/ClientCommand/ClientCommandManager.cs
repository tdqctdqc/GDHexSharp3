using Godot;
using System;

public class ClientCommandManager 
{
    public IClientCommand Command { get; private set; }
    private Hint _hint => Game.I.Session.Client.UI.MapUI.Hint;
    public void SetCommand(IClientCommand com)
    {
        Command = com;
        _hint.SetCurrentCommand(com.Hint);

    }
    public void DoCommand()
    {
        if(Command == null) return;
        string warning;  
        bool go = Command.Condition(out warning);
        _hint.SetWarning(warning);
        if(go)
        {
            Command.Do();
            _hint.SetCurrentCommand("");
            Command = null;
        }
    }
    public void CancelCommand()
    {
        _hint.SetWarning("");
        _hint.SetCurrentCommand("");
        Command = null;
    }
}
