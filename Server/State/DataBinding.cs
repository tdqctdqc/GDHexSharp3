using Godot;
using System;
using System.Collections.Generic;
using System.Data;

public class DataBinding 
{
    public static void Setup()
    {
        ModelInfoManager.SetupModelInfos();
        CacheManager.Setup();
        // OrderInterpreter.Setup();
        SetupModel<UnitModel>();
        SetupModel<HexModel>();
        SetupModel<FactionModel>();
        SetupModel<LocationModel>();
        SetupModel<RoadModel>();
        SetupModel<FormationModel>();
        SetupModel<OrderModel>();
        SetupModel<RiverModel>();
    }

    private static void SetupModel<T>() where T : IModel, new()
    {
        CacheManager.SetupCache<T>();
        ModelInfoManager.SetupModelInfo<T>();
    }
}
