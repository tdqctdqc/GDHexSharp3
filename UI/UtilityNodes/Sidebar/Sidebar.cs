using Godot;
using System;
using System.Collections.Generic;

public class Sidebar : Control
{
    private Dictionary<FuncButton, Control> _panels = new Dictionary<FuncButton, Control>();

    public void AddPanel(FuncButton button, Control panel)
    {
        _panels.Add(button, panel);
        Action action = () => {HidePanels(); panel.Visible = true;};
        button.Add(action);
    }

    public void HidePanels()
    {
        foreach (var entry in _panels)
        {
            entry.Value.Visible = false; 
        }
    }
}
