using Godot;
using System;

public class Scenes
{
    //public static Type Type => (Type)((PackedScene)(GD.Load(""))).Instance();
    public static PackedScene Counter => (PackedScene)GD.Load("res://Graphics/UnitGraphics/Counter/Counter.tscn");
    public static MainMenu MainMenu => (MainMenu)((PackedScene)(GD.Load("res://UI/MainMenu/MainMenu.tscn"))).Instance();
    public static ConfirmationWindow ConfirmationWindow => (ConfirmationWindow)((PackedScene)(GD.Load("res://UI/UtilityNodes/ConfirmationWindow/ConfirmationWindow.tscn"))).Instance();
    public static StringEntry StringEntry => (StringEntry)((PackedScene)(GD.Load("res://UI/UtilityNodes/StringEntry/StringEntry.tscn"))).Instance();
    public static NumEntry NumEntry => (NumEntry)((PackedScene)(GD.Load("res://UI/UtilityNodes/NumEntry/NumEntry.tscn"))).Instance();
    public static ColorEntry ColorEntry => (ColorEntry)((PackedScene)(GD.Load("res://UI/UtilityNodes/ColorEntry/ColorEntry.tscn"))).Instance();
    public static ValueEntryWindow ValueEntryWindow => (ValueEntryWindow)((PackedScene)(GD.Load("res://UI/UtilityNodes/ValueEntryWindow/ValueEntryWindow.tscn"))).Instance();
    public static GameParameterSetupScreen GameParameterSetupScreen => (GameParameterSetupScreen)((PackedScene)(GD.Load("res://UI/MainMenu/GameParameterSetupScreen/GameParameterSetupScreen.tscn"))).Instance();
    public static MapUI MapUI => (MapUI)((PackedScene)(GD.Load("res://Client/ClientUI/MapUI/MapUI.tscn"))).Instance();
    public static UIHex UIHex => (UIHex)((PackedScene)(GD.Load("res://UI/UtilityNodes/UIHex/UIHex.tscn"))).Instance();
    public static UnitGraphic UnitGraphic => (UnitGraphic)((PackedScene)(GD.Load("res://Client/Graphics/UnitGraphics/UnitGraphic.tscn"))).Instance();
    public static UIUnit UIUnit => (UIUnit)((PackedScene)(GD.Load("res://UI/UtilityNodes/UIUnit/UIUnit.tscn"))).Instance();
    public static LoadingScreen LoadingScreen => (LoadingScreen)((PackedScene)(GD.Load("res://UI/LoadingScreen/LoadingScene.tscn"))).Instance();

}
