using Godot;
using System;
using System.Collections.Generic;

public class CacheManager
                            
{ 
    public static Dictionary<string, Action<List<int>, List<string[]>>> SyncFuncs { get; private set; }
    public static Action LoadState {get; set;}
    public static Action LoadedState {get; set;}
    public static Action ClearDeleteQueues {get; set;}
    public static RoadsCache Roads { get; private set; }
    public static RiversCache Rivers { get; private set; }
    public static LocationCache Locations { get; private set; }
    public static AICache AI { get; private set; }

    public static void Setup()
    {
        Roads = new RoadsCache();
        Rivers = new RiversCache();
        Locations = new LocationCache();
        AI = new AICache();
        SyncFuncs = new Dictionary<string, Action<List<int>, List<string[]>>>();
    }
    public static void LoadCache()
    {
        Game.I.Timers.SetTimer("loading cache");
        LoadState?.Invoke();
        LoadedState?.Invoke();
        Game.I.Timers.CallTimer("loading cache");
    }
    public static void SetupCache<T>() where T : IModel, new()
    {
        string tableName = new T().TableNameI;
        Cache<T>.TableName = tableName;
        Cache<T>.Setup();
        ClearDeleteQueues += Cache<T>.ClearDeletionQueue;

        Action<List<int>, List<string[]>> syncFunc = (i,f) =>
        {
            Cache<T>.SyncModels(i,f);
            Cache<T>.ModelsChanged?.Invoke(Cache<T>.GetModels(i));
        };
        SyncFuncs.Add(tableName, syncFunc);
    }
}
