using Godot;
using System;

public interface ISession
{
    IServer Server {get;}
    IClient Client {get;}
    IData Data {get;}
    IEditor Editor {get;}
    IUtility Utility {get;}
    
    GameGenerationParameters Params {get;}
    MapGenPackage MapGenPackage { get; set; }
    void AwakeClient();
}
