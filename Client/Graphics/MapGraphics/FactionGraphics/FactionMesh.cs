using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class FactionMesh : Node2D
{
    //private BaseMesh _baseMesh;
    private ShaderMaterial _shaderMaterial;
    public float Transparency { get; private set; }
    private HexBorder _border; 
    public FactionMesh()
    {
        Func<HexModel,Color> getHexFactionColorFunc = (h) => new Color(h.Faction.PrimaryColorString);
        //_baseMesh = new BaseMesh(getHexFactionColorFunc);
        _shaderMaterial = GraphicsUtility.TransparencyShaderMaterial;
        //_baseMesh.Material = _shaderMaterial;
        SetTransparency(0f);

        //AddChild(_baseMesh);
        _border = new HexBorder();
        AddChild(_border);
    }
    public void Setup()
    {
        CacheManager.LoadedState += LoadState;
    }

    public void LoadState()
    {
        var facs = Cache<FactionModel>.GetModels();
        foreach (var fac in facs)
        {
            if(fac == null) GD.Print("null fac");
            if(fac.PrimaryColorString == null) GD.Print("null color");
        }
        var hexes = Cache<HexModel>.GetModels();
        //_baseMesh.Setup(hexes);
        _border.Setup(hexes, 20f);

        Cache<HexModel>.ModelsChanged += HexesChanged;
    }

    public void SetTransparency(float transparency)
    {
        Transparency = transparency / 100f;
        _shaderMaterial.SetShaderParam("transparency", Transparency);
    }
    public void HexesChanged(List<HexModel> hexes)
    {
        //_baseMesh.UpdateHexes(hexes);
        var hash = new HashSet<HexModel>(hexes);
        foreach (var hex in hexes)
        {
            foreach (var n in hex.GetNeighbors())
            {
                hash.Add(n);
            }
        }
        _border.SetBorder(hash.ToList());
    }
}
