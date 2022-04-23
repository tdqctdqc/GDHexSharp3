using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class HexBorder : MultiMeshInstance2D
{
    private Color _color; 
    private float _width; 


    public void Setup(Color color, float width)
    {
        _color = color; 
        Modulate = color; 
        _width = width; 
        Multimesh = new MultiMesh();
        
        Multimesh.TransformFormat =  MultiMesh.TransformFormatEnum.Transform2d;
        Multimesh.ColorFormat = MultiMesh.ColorFormatEnum.Float;
        Multimesh.CustomDataFormat = MultiMesh.CustomDataFormatEnum.None;

        var mesh = new HexBorderMesh();
        
    
        mesh.Setup(width);
        Multimesh.Mesh = mesh;
    }
    public void Setup(List<HexModel> hexes, float width)
    {
        _width = width; 
        Multimesh = new MultiMesh();
        Multimesh.TransformFormat =  MultiMesh.TransformFormatEnum.Transform2d;
        Multimesh.ColorFormat = MultiMesh.ColorFormatEnum.Float;
        Multimesh.CustomDataFormat = MultiMesh.CustomDataFormatEnum.None;
        var mesh = new HexBorderMesh();
        mesh.Setup(width);
        Multimesh.Mesh = mesh;

        Multimesh.InstanceCount = hexes.Count * 6;
        for (int i = 0; i < hexes.Count; i++)
        {
            var hex = hexes[i];
            int index = (hex.ID - 1) * 6;
            var neighbors = hex.GetNeighbors();
            Vector2 center = hex.WorldPos; 

            for (int j = 0; j < 6; j++)
            {
                int instance = index + j;
                if(neighbors.Count > j)
                {
                    var outside = neighbors[j];
                    float angle = (hex.WorldPos - outside.WorldPos).AngleTo(Vector2.Right) + Mathf.Pi / 2f;
                    Multimesh.SetInstanceTransform2d(instance, new Transform2D(-angle, center));
                    if(outside.Faction != hex.Faction)
                    {
                        Multimesh.SetInstanceColor(instance, hex.Faction.PrimaryColor);
                    }
                    else 
                    {
                        Multimesh.SetInstanceColor(instance, Colors.Transparent);
                    }
                }
                else
                {
                    Multimesh.SetInstanceTransform2d(instance, new Transform2D(0, center));
                    Multimesh.SetInstanceColor(instance, Colors.Transparent);
                }
            }
        }
    }

    public void TestMesh()
    {
        //just to stop disappearing bug
        int count = Game.I.Session.Params.Width * Game.I.Session.Params.Height;
        Multimesh.InstanceCount = 4; 
        int iter = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Vector2 pos = Vector2.Right * Game.I.Session.Params.RealWidth * i + Vector2.Down * Game.I.Session.Params.RealHeight * j;
                Multimesh.SetInstanceTransform2d(iter, new Transform2D(0, pos));
                iter++;
            }
        }
        //Visible = false; 
    }

    public void SetBorder(List<HexModel> hexes)
    {
        Visible = true; 
        foreach (var hex in hexes)
        {
            int index = (hex.ID - 1) * 6;
            var neighbors = hex.GetNeighbors(); 
            for (int j = 0; j < 6; j++)
            {
                int instance = index + j;
                if(neighbors.Count > j)
                {
                    var outside = neighbors[j];
                    if(outside.Faction != hex.Faction)
                    {
                        Multimesh.SetInstanceColor(instance, hex.Faction.PrimaryColor);
                    }
                    else 
                    {
                        Multimesh.SetInstanceColor(instance, Colors.Transparent);
                    }
                }
                else
                {
                    Multimesh.SetInstanceColor(instance, Colors.Transparent);
                }
            }
        }

        /*
        var borderHexPairs = new List<HexPair>();
        
        var insides = new List<HexModel>();
        var outsides = new List<HexModel>();
        foreach (var hex in hexes)
        {
            var neighbors = HexUtility.GetHexNeighors(hex);
            foreach (var n in neighbors)
            {
                if(hexes.Contains(n) == false) 
                {
                    insides.Add(hex);
                    outsides.Add(n);
                }
            }
        }

        Multimesh.InstanceCount = insides.Count;
        

        for (int i = 0; i < insides.Count; i++)
        {
            var inside = insides[i];
            var outside = outsides[i];

            float angle = (inside.WorldPos - outside.WorldPos).AngleTo(Vector2.Right) + Mathf.Pi / 2f;
            Vector2 center = inside.WorldPos; 
            Multimesh.SetInstanceTransform2d(i, new Transform2D(-angle, center));
        }
        */
    }
}
