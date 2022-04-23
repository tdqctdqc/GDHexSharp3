using Godot;
using System;
using System.Collections.Generic;

public class Constants
{
    public static float HostileFactionTerritoryMoveCostMult = 1.5f;
    public static float HexRadius = 100f; 
    public static int MaxUnitsInHex = 7;
    public static Vector3 North = new Vector3(0,1,-1), NorthEast = new Vector3(1,0,-1), SouthEast = new Vector3(1,-1,0), South = new Vector3(0,-1,1), SouthWest = new Vector3(-1,0,1), NorthWest = new Vector3(-1,1,0);
    public static List<Vector3> HexDirs = new List<Vector3>(){North, NorthEast, SouthEast, South, SouthWest, NorthWest};
    public static List<Color> ColorList = new List<Color>(){
                                                            Colors.Red,
                                                            Colors.Blue,
                                                            Colors.Green,
                                                            Colors.Yellow,
                                                            Colors.Orange,
                                                            Colors.DarkBlue,
                                                            Colors.Cyan, 
                                                            Colors.Gold,
                                                            Colors.Gray,
                                                            Colors.Goldenrod,
                                                            Colors.Tan,
                                                            Colors.Teal,
                                                            Colors.Brown,
                                                            Colors.Azure,
                                                            Colors.Peru,
                                                            Colors.Purple,
                                                            Colors.BlanchedAlmond,
                                                            Colors.DarkMagenta,
                                                            Colors.DarkKhaki,
                                                            Colors.DarkGreen
                                                        };
}
