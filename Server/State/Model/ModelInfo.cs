using Godot;
using System;
using System.Collections.Generic;

public class ModelInfo<T> : IModelInfo where T : IModel, new()
{
    // public static string TableName {get; private set;}
    // public static string ColumnNames {get; private set;}
    // public static string ColumnNamesNoID {get; private set;}
    // public static string DapperParameterNames {get; private set;}
    // public static string DapperParameterNamesNoID {get; private set;}
    // public static string PrimaryKeyName {get; private set;}
    // public static List<string> ColumnList {get; private set;}
    // public static List<string> ColumnTypeList {get; private set;}
    // public static List<string> ColumnInitFlags {get; private set;}
    // public static List<string> DapperParameterList {get; private set;}

    // public static void SetInfo( string tableName, 
    //                             string primaryKeyName, 
    //                             List<string> columnNames, 
    //                             List<string> columnTypeNames,
    //                             List<string> columnInitFlags)
    // {
    //     TableName = tableName; 
    //     PrimaryKeyName = primaryKeyName;
    //     ColumnNames = "";
    //     ColumnNamesNoID = "";
    //     DapperParameterNames = "";
    //     DapperParameterNamesNoID = "";
    //     ColumnList = new List<string>(columnNames);
    //     ColumnTypeList = new List<string>(columnTypeNames);
    //     ColumnInitFlags = new List<string>(columnInitFlags);
    //     DapperParameterList = new List<string>();
    //     for (int i = 0; i < columnNames.Count; i++)
    //     {
    //         string name = columnNames[i];
    //         if(i > 0)
    //         {
    //             ColumnNamesNoID += name;
    //             DapperParameterNamesNoID += "@"+name;
    //             if(i != columnNames.Count - 1)
    //             {
    //                 ColumnNamesNoID += ", ";
    //                 DapperParameterNamesNoID += ", ";
    //             }
    //         }
    //         ColumnNames += name;
    //         DapperParameterNames += "@"+name;
    //         DapperParameterList.Add("@"+name);
    //         if(i != columnNames.Count - 1)
    //         {
    //             ColumnNames += ", ";
    //             DapperParameterNames += ", ";
    //         }
    //     }
    // }
}
