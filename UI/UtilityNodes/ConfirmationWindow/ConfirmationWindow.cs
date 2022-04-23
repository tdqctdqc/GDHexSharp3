using Godot;
using System;

public class ConfirmationWindow : WindowDialog
{
    private Label _label; 
    private FuncButton _yes, _no;
    private Action _action;

    public override void _Ready()
    {
        _label = GetNode<Label>("VBox/Label");
        _yes = GetNode<FuncButton>("VBox/Yes");
        _yes.Set(Confirm);
        _no = GetNode<FuncButton>("VBox/No");
        _no.Set(() => Visible = false);
    }
    public void Set(Action action, string prompt)
    {
        _label.Text = prompt;
        _action = action;
    }

    public void SetText(string text)
    {
        _label.Text = text;
    }

    public void Confirm()
    {
        _action?.Invoke();
        Visible = false; 
    }
}
