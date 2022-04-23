using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HexWargame;

public class HexPanel : Control
{
    private UIHex _hex;
    private Label _coords, _terrain, _faction, _supply, _prod, _recruits; 
    public override void _Ready()
    {
        _hex = GetNode<UIHex>("VBox/UIHex");
        _coords = GetNode<Label>("VBox/Labels/Coords");
        _terrain = GetNode<Label>("VBox/Labels/Terrain");
        _faction = GetNode<Label>("VBox/Labels/Faction");
        _supply = GetNode<Label>("VBox/Labels/Supply");
        _prod = GetNode<Label>("VBox/Labels/Prod");
        _recruits = GetNode<Label>("VBox/Labels/Recruits");
    }

    public void Setup(HexModel model)
    {
        _hex.Setup(model);
        _coords.Text = "Coords: " + new Vector2(model.X, model.Y).ToString();
        _terrain.Text = "Coords: " + model.Terrain.Name;
        _faction.Text = "Faction: " + model.Faction.Name;

        var loc = Cache<LocationModel>.GetModels().Where(l => l.HexID == model.ID).FirstOrDefault();
        if(loc != null)
        {
            _supply.Text = $"Supply: {loc.SupplyProd}";
            _prod.Text = $"Production: {loc.ProdPoints}";
            _recruits.Text = $"Recruits: {loc.Recruits}";
        }
        else
        {
            _supply.Text = "";
            _prod.Text = "";
            _recruits.Text = "";
        }
    }
}
