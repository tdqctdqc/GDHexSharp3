using Godot;
using System;

public class BackingModel 
{
    private IModel _model; 
    public int ID => _model.ID;
    public string Table { get; private set; }
    public string[] Fields { get; private set; }
    public BackingModel(IModel model)
    {
        _model = model;
        Table = model.TableNameI;
        Fields = new string[model.NumFields];
    }
    public void Sync(string[] newFields)
    {
        for (int i = 0; i < Fields.Length; i++)
        {
            Fields[i] = newFields[i];
        }
        _model.SyncFromBacking();
    }
}
