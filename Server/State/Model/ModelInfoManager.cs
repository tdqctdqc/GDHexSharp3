using Godot;
using System;
using System.Collections.Generic;
using System.Data;

public class ModelInfoManager
{
    public static Dictionary<string, IModelInfo> ModelInfos { get; private set; }
    private static string _tableSetupCommandString; 
    public static void SetupModelInfos()
    {
        ModelInfos = new Dictionary<string, IModelInfo>();
    }

    public static void SetupModelInfo<T>() where T : IModel, new()
    {
        // var t = new T();
        // // ModelInfo<T>.SetInfo(   t.TableNameI, 
        // //                         t.PrimaryKeyNameI, 
        // //                         t.ColumnNameListI, 
        // //                         t.ColumnTypeListI, 
        // //                         t.ColumnInitFlagListI); 
        // // var modelInfo = new ModelInfo<T>();
        // ModelInfos.Add(t.TableNameI, modelInfo);
    }
}
