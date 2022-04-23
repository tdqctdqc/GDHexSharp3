using Godot;
using System;

public class ScrollList : ScrollContainer
{
    public IListContainer List;
    private Func<int, string> _getEntryName; 
    private VBoxContainer _vBox;
    public Action SelectAction {get; set;}
    public void Setup(IListContainer list, Func<int, Control> getChildScene, Action selectAction = null)
    {
        Clear();
        List = list;
        SelectAction = selectAction;
        for(int i = 0; i < List.List.Count; i++)
        {
            var control = getChildScene(i);

            int j = i;
            Action select = () => Select(j);
            var funcControl = new FuncControl(select, control);
            _vBox.AddChild(funcControl);
        }
    }

    public override void _Ready()
    {
        _vBox = new VBoxContainer();
        AddChild(_vBox);
        _vBox.Name = "VBox";
    }

    public void Select(int i)
    {
        List.Select(i);
        HighlightEntry(i);
        //SelectAction();
    }

    public void HighlightEntry(int j)
    {
        for (int i = 0; i < List.List.Count; i++)
        {
            var entry = (Control)_vBox.GetChild(i);
            if(i == j) entry.Modulate = Colors.Red;
            else entry.Modulate = new Color(1f,1f,1f,.5f);
        }
    }
    public void Clear()
    {
        while(_vBox.GetChildCount() > 0)
        {
            _vBox.GetChild(0).Free();
        }
    }

}
