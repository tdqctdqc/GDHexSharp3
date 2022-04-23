using Godot;
using System;
using System.Collections.Generic;

public interface IModel : INamed
{
    int ID {get; set;}
    int NumFields {get;}
    string TableNameI {get;}
    BackingModel Backing {get;}
    void SyncFromBacking();
}
