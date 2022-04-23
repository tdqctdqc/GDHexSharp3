using Godot;
using System;
using System.Collections.Generic;

public class GraphicsUtility
{
    public static Dictionary<int, List<Vector2>> UnitIconOffsets; 

    public static ShaderMaterial TransparencyShaderMaterial => (ShaderMaterial)GD.Load("res://Client/Graphics/Utility/TransparencyShader/TransparencyShaderMaterial.tres");
    public static Color Good = new Color(0f, .6f, 0f, 1f), 
                        GoodOK = new Color(.5f, .75f, 0f, 1f),
                        OK = new Color(1f, 1f, 0f, 1f),
                        OKBad = new Color(1f, .5f, 0f, 1f),
                        Bad = new Color(1f, 0f, 0f, 1f),
                        VBad = new Color(0f, 0f, 0f, 1f);
    public static Color GetColorForPercent(float percent)
    {
        if(percent < .2f) return VBad;
        else if(percent < .4f) return Bad;
        else if(percent < .5f) return OKBad;
        else if(percent < .6f) return OK;
        else if(percent < .8f) return GoodOK;
        else return Good; 
    }
    public static void Setup()
    {
        UnitIconOffsets = new Dictionary<int, List<Vector2>>();
        var r = Constants.HexRadius; 
        var result = new List<Vector2>();
        UnitIconOffsets.Add(0, null);
        UnitIconOffsets.Add(1, new List<Vector2>(){Vector2.Zero});


        Vector2 shift2 = new Vector2(r / 3f, 0f);
        UnitIconOffsets.Add(2, new List<Vector2>(){-shift2, shift2});

        Vector2 shift3 = new Vector2(0f, 3f * r / 7f);
        UnitIconOffsets.Add(3, new List<Vector2>(){shift3, 
                                                    shift3.Rotated(2f * Mathf.Pi / 3f),
                                                    shift3.Rotated(4f * Mathf.Pi / 3f)});


        Vector2 shift4 = new Vector2(0f, r / 2f);
        UnitIconOffsets.Add(4, new List<Vector2>(){shift4.Rotated(Mathf.Deg2Rad(45f)), 
                                                    shift4.Rotated(Mathf.Deg2Rad(135f)),
                                                    shift4.Rotated(Mathf.Deg2Rad(225f)),
                                                    shift4.Rotated(Mathf.Deg2Rad(315f))});

        Vector2 shift5 = new Vector2(0f, 4f * r / 7f);
        Vector2 shift52 = new Vector2(0f, r / 12f);
        UnitIconOffsets.Add(5, new List<Vector2>(){Vector2.Zero, 
                                                    shift5.Rotated(Mathf.Deg2Rad(30f)) + shift52,
                                                    shift5.Rotated(Mathf.Deg2Rad(330f)) + shift52,
                                                    shift5.Rotated(Mathf.Deg2Rad(210f)) - shift52,
                                                    shift5.Rotated(Mathf.Deg2Rad(150f)) - shift52});

        Vector2 shift61 = new Vector2(4f * r / 7f, 0f);
        Vector2 shift62 = new Vector2(0f, 4f * r / 7f);
        Vector2 shift63 = new Vector2(0f, r / 12f);
        UnitIconOffsets.Add(6, new List<Vector2>(){-shift61,
                                                    shift61,
                                                    shift62.Rotated(Mathf.Deg2Rad(30f)) + shift63,
                                                    shift62.Rotated(Mathf.Deg2Rad(330f)) + shift63,
                                                    shift62.Rotated(Mathf.Deg2Rad(210f)) - shift63,
                                                    shift62.Rotated(Mathf.Deg2Rad(150f)) - shift63});

        Vector2 shift71 = new Vector2(4f * r / 7f, 0f);
        Vector2 shift72 = new Vector2(0f, 4f * r / 7f);
        Vector2 shift73 = new Vector2(0f, r / 12f);
        UnitIconOffsets.Add(7, new List<Vector2>(){Vector2.Zero,
                                                    -shift71,
                                                    shift71,
                                                    shift72.Rotated(Mathf.Deg2Rad(30f)) + shift73,
                                                    shift72.Rotated(Mathf.Deg2Rad(330f)) + shift73,
                                                    shift72.Rotated(Mathf.Deg2Rad(210f)) - shift73,
                                                    shift72.Rotated(Mathf.Deg2Rad(150f)) - shift73});
    }
    public static List<Vector2> GetPathArrowMeshPoints(List<HexModel> hexes, float width)
    {
        var points = new List<Vector2>();
        for (int i = 0; i < hexes.Count - 1; i++)
        {
            var from = hexes[i];
            var to = hexes[i + 1];
            var angle = (to.WorldPos - from.WorldPos).Angle();
            float nextAngle = 0f;
            if(i < hexes.Count - 2)
            {
                nextAngle = (hexes[i + 2].WorldPos - to.WorldPos).Angle();
            }

            var p1 = from.WorldPos + Vector2.Up.Rotated(angle) * width; 
            var p2 = from.WorldPos + Vector2.Up.Rotated(angle + Mathf.Pi) * width; 
            var p3 = to.WorldPos +  Vector2.Up.Rotated(angle) * width; 
            var p4 = to.WorldPos +  Vector2.Up.Rotated(angle + Mathf.Pi) * width; 
            var p5 = to.WorldPos +  Vector2.Up.Rotated(nextAngle) * width; 
            var p6 = to.WorldPos +  Vector2.Up.Rotated(nextAngle + Mathf.Pi) * width; 
            points.Add(p1);
            points.Add(p2);
            points.Add(p3);

            points.Add(p3);
            points.Add(p4);
            points.Add(p2);

            if(nextAngle != 0f)
            {
                points.Add(p3);
                points.Add(p5);
                points.Add(to.WorldPos);

                points.Add(p6);
                points.Add(p4);
                points.Add(to.WorldPos);
            }
            
            if(i == hexes.Count - 2)
            {
                var a1 = to.WorldPos +  Vector2.Up.Rotated(angle) * width * 2f; 
                var a2 = to.WorldPos +  Vector2.Up.Rotated(angle + Mathf.Pi / 2f) * width * 3f; 
                var a3 = to.WorldPos +  Vector2.Up.Rotated(angle + Mathf.Pi) * width * 2f; 
                points.Add(a1);
                points.Add(a2);
                points.Add(a3);
            }
        }
        return points; 
    }

    public static List<Vector2> GetDefPathMeshPoints(List<HexModel> hexes, float width)
    {
        var points = new List<Vector2>();
        for (int i = 0; i < hexes.Count - 1; i++)
        {
            var from = hexes[i];
            var to = hexes[i + 1];
            var angle = (to.WorldPos - from.WorldPos).Angle();
            float nextAngle = 0f;
            if(i < hexes.Count - 2)
            {
                nextAngle = (hexes[i + 2].WorldPos - to.WorldPos).Angle();
            }

            var p1 = from.WorldPos + Vector2.Up.Rotated(angle) * width; 
            var p2 = from.WorldPos + Vector2.Up.Rotated(angle + Mathf.Pi) * width; 
            var p3 = to.WorldPos +  Vector2.Up.Rotated(angle) * width; 
            var p4 = to.WorldPos +  Vector2.Up.Rotated(angle + Mathf.Pi) * width; 
            var p5 = to.WorldPos +  Vector2.Up.Rotated(nextAngle) * width; 
            var p6 = to.WorldPos +  Vector2.Up.Rotated(nextAngle + Mathf.Pi) * width; 
            points.Add(p1);
            points.Add(p2);
            points.Add(p3);

            points.Add(p3);
            points.Add(p4);
            points.Add(p2);

            if(nextAngle != 0f)
            {
                points.Add(p3);
                points.Add(p5);
                points.Add(to.WorldPos);

                points.Add(p6);
                points.Add(p4);
                points.Add(to.WorldPos);
            }
            var point1 =  from.WorldPos + (to.WorldPos - from.WorldPos) / 3f; 
            var push = Vector2.Up.Rotated(angle) * width;
            var a1 = push + point1 +  Vector2.Up.Rotated( -(angle + Mathf.Pi / 2f) ) * width * 2f; 
            var a2 = push + point1 +  Vector2.Up.Rotated( -(angle + Mathf.Pi) ) * width * 3f; 
            var a3 = push + point1 +  Vector2.Up.Rotated( -(angle + 3f * Mathf.Pi / 2f) ) * width * 2f; 
            
            var point2 = from.WorldPos + (to.WorldPos - from.WorldPos) * 2f / 3f; 
            var a4 = push + point2 +  Vector2.Up.Rotated( -(angle + Mathf.Pi / 2f) ) * width * 2f; 
            var a5 = push + point2 +  Vector2.Up.Rotated( -(angle + Mathf.Pi) ) * width * 3f; 
            var a6 = push + point2 +  Vector2.Up.Rotated( -(angle + 3f * Mathf.Pi / 2f) ) * width * 2f; 
            
            var point3 = from.WorldPos; 
            var a7 = push + point3 +  Vector2.Up.Rotated( -(angle + Mathf.Pi / 2f) ) * width * 2f; 
            var a8 = push + point3 +  Vector2.Up.Rotated( -(angle + Mathf.Pi) ) * width * 3f; 
            var a9 = push + point3 +  Vector2.Up.Rotated( -(angle + 3f * Mathf.Pi / 2f) ) * width * 2f;
            
            
            points.Add(a1);
            points.Add(a2);
            points.Add(a3);
            points.Add(a4);
            points.Add(a5);
            points.Add(a6);
            points.Add(a7);
            points.Add(a8);
            points.Add(a9);
        }
        return points; 
    }
}
