using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class StateLogicInterface
{
    public OrderManager OrderManager { get; private set; }
    //private IState _state;
    //private ServerEventRouter _router;
    public StateLogicInterface(/*IState state, ServerEventRouter router*/)
    {
        OrderManager = new OrderManager();
        //_state = state;
        //_router = router;
    }

    public void AddModels<T>(List<T> models) where T : IModel, new()
    {
        Cache<T>.AddModels(models);
        // _state.SaveModels<T>(models);
        // List<T> newModels;
        // if(Cache<T>.Size == 0) newModels = _state.LoadModels<T>();
        // else newModels = _state.LoadModelsInverse<T>(Cache<T>.IDs);
        // _router.BroadcastModelsUpdateEvent<T>(newModels);
    }
    public void AddModel<T>(T model) where T : IModel, new()
    {
        Cache<T>.AddModel(model);
        // _state.SaveModel<T>(model);
        // List<T> newModels; 
        // if(Cache<T>.Size == 0) newModels = _state.LoadModels<T>();
        // else newModels = _state.LoadModelsInverse<T>(Cache<T>.IDs);
        // _router.BroadcastModelsUpdateEvent<T>(newModels);
        // return newModels[0];
    }
    public void UpdateModel<T>(T model, Func<T,T> updateFunc = null) where T : IModel, new()
    {
        Cache<T>.UpdateModel(model);
        // _state.UpdateModel<T>(model, updateFunc);
        // _router.BroadcastModelsUpdateEvent<T>(new List<T>(){model});
    }
    public void UpdateModels<T>(List<T> models, Func<T,T> updateFunc = null) where T : IModel, new()
    {
        Cache<T>.UpdateModels(models);

        // _state.UpdateModels<T>(models, updateFunc);
        // _router.BroadcastModelsUpdateEvent<T>(models);
    }

    public void DeleteModels<T>(List<T> models) where T : IModel, new()
    {
        Cache<T>.DeleteModels(models);
        // _state.DeleteModels<T>(models);
        // _router.BroadcastModelsDeleteEvent<T>(models);
    }
    public void DeleteModels<T>(List<int> ids) where T : IModel, new()
    {
        var models = Cache<T>.GetModels(ids);
        Cache<T>.DeleteModels(models);
        // _state.DeleteModels<T>(models);
        // _router.BroadcastModelsDeleteEvent<T>(models);
    }
    public void DeleteModel<T>(T model) where T : IModel, new()
    {
        Cache<T>.DeleteModel(model);
        // var models = new List<T>(){model};
        // _state.DeleteModels(models);
        // _router.BroadcastModelsDeleteEvent<T>(models);
    }
}
