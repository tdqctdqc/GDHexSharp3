using Godot;
using System;
using System.Collections.Generic;

public class UIBarController 
{
    private Dictionary<FuncButton, IBar> _panels = new Dictionary<FuncButton, IBar>();

    public void AddPanel(FuncButton button, IBar bar)
    {
        _panels.Add(button, bar);
        Action action = () => {HidePanels(bar); bar.Open();};
        button.Add(action);
    }
    public void HidePanels(IBar openBar)
    {
        foreach (var entry in _panels)
        {
            var bar = entry.Value;

            if (bar != openBar) bar.Close();
        }
    }
}
