using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Cache<T> where T : IModel, new()
{
    public static string TableName { get; set; }
    public static int Size => _modelsList.Count; 
    public static List<int> IDs => _modelsDic.Keys.ToList();
    public static Action<List<T>> ModelsAdded {get; set;}
    public static Action<List<T>> ModelsChanged {get; set;}
    public static Action<List<int>> ModelsDeleted {get; set;}
    private static Dictionary<int, T> _modelsDic;
    private static List<T> _modelsList; 
    private static List<int> _modelsDeleteQueue; 
    private static int _idIndex = 0;

    public static void Setup()
    {
        _idIndex = 0;
        _modelsDic = new Dictionary<int, T>();
        _modelsList = new List<T>();
        _modelsDeleteQueue = new List<int>();
    }
    public static int TakeID()
    {
        int id = _idIndex;
        _idIndex++;
        return id;
    }
    public static T GetModel(int id)
    {
        if(_modelsDic.ContainsKey(id)) return _modelsDic[id];
        else return default(T);
    }
    public static T GetModel(Predicate<T> cond)
    {
        return _modelsList.Where(m => cond(m)).FirstOrDefault();
    }
    public static List<T> GetModels(List<int> ids)
    {
        var list = new List<T>(ids.Count);
        foreach (var id in ids)
        {
            if(_modelsDic.ContainsKey(id)) list.Add(_modelsDic[id]);
        }
        return list;
    }
    public static List<T> GetModels(Predicate<T> cond)
    {
        return _modelsList.Where(m => cond(m)).ToList();
    }
    public static List<T> GetModels()
    {
        return new List<T>(_modelsList);
    }

    public static void AddModel(T model)
    {
        _modelsDic.Add(model.ID, model);
        _modelsList.Add(model);
        ModelsAdded?.Invoke(new List<T>(){model});
    }
    public static void AddModels(List<T> models)
    {
        foreach (var model in models)
        {
            _modelsDic.Add(model.ID, model);
            _modelsList.Add(model);
        }
        ModelsAdded?.Invoke(models);
    }
    public static void UpdateModel(T model)
    {
        //Game.I.Session.Server.BroadcastUpdateModels(TableName, new List<BackingModel>(){model.Backing});
        
        ModelsChanged?.Invoke(new List<T>(){model});
    }

    public static void UpdateModels(List<T> models)
    {
        //Game.I.Session.Server.BroadcastUpdateModels(TableName, models.Select(m => m.Backing).ToList());
        ModelsChanged?.Invoke(models);
    }
    public static void SyncModels(List<int> ids, 
                            List<string[]> fieldsList)
    {
        for (int i = 0; i < ids.Count; i++)
        {
            if(_modelsDic.ContainsKey(ids[i]))
            {
                _modelsDic[ids[i]].Backing.Sync(fieldsList[i]);
            }
        }
    }
    public static void DeleteModel(T model)
    {
        if(_modelsDic.ContainsKey(model.ID))
        {
            var toRemove = _modelsDic[model.ID];
            _modelsDic.Remove(model.ID);
            _modelsList.Remove(toRemove);
        }
        ModelsDeleted?.Invoke(new List<int>(){model.ID});
    }

    public static void DeleteModels(List<T> models)
    {
        foreach (var model in models)
        {
            if(_modelsDic.ContainsKey(model.ID))
            {
                var toRemove = _modelsDic[model.ID];
                _modelsDic.Remove(model.ID);
                _modelsList.Remove(toRemove);
            }
        }
        ModelsDeleted?.Invoke(models.Select(m => m.ID).ToList());
    }
    public static void QueueModelsDeletion(List<int> models)
    {
        _modelsDeleteQueue.AddRange(models);
    }
    public static void QueueModelDeletion(int model)
    {
        _modelsDeleteQueue.Add(model);
    }
    public static void ClearDeletionQueue()
    {
        DeleteModels(GetModels(_modelsDeleteQueue));
        _modelsDeleteQueue = new List<int>();
    }
}
