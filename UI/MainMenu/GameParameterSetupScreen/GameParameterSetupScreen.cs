using Godot;
using System;

public class GameParameterSetupScreen : WindowDialog
{
    private NumEntry _seed, _width, _height, _numContinents, _altOctaves, _altPeriod, _altPersistence, 
                    _moistOctaves, _moistPeriod, _moistPersistence, _percentLand;
    private FuncButton _confirmButton;
    private Action<GameGenerationParameters> _confirmAction; 
    public override void _Ready()
    {
        _seed = GetNode<NumEntry>("VBox/Seed");
        _seed.GetNode<SpinBox>("SpinBox").Value = new RandomNumberGenerator().Randi();

        _width = GetNode<NumEntry>("VBox/Width");
        _height = GetNode<NumEntry>("VBox/Height");
        _numContinents = GetNode<NumEntry>("VBox/NumContinents");

        _altOctaves = GetNode<NumEntry>("VBox/AltitudeOctaves");
        _altPeriod = GetNode<NumEntry>("VBox/AltitudePeriod");
        _altPersistence = GetNode<NumEntry>("VBox/AltitudePersistence");
        _moistOctaves = GetNode<NumEntry>("VBox/MoistureOctaves");
        _moistPeriod = GetNode<NumEntry>("VBox/MoisturePeriod");
        _moistPersistence = GetNode<NumEntry>("VBox/MoisturePersistence");
        _percentLand = GetNode<NumEntry>("VBox/PercentLand");
        
        _confirmButton = GetNode<FuncButton>("VBox/Confirm");
        _confirmButton.Set(ConfirmButtonPressed);
    }

    public void Setup(Action<GameGenerationParameters> confirmAction)
    {
        _confirmAction = confirmAction;
    }


    public void ConfirmButtonPressed()
    {
        var prms = new GameGenerationParameters(_seed.GetIntValue(),
                                                _width.GetIntValue(),
                                                _height.GetIntValue(), 
                                                _numContinents.GetIntValue(),
                                                _percentLand.GetFloatValue() / 100f, 
                                                _altOctaves.GetIntValue(), 
                                                _altPeriod.GetFloatValue(), 
                                                _altPersistence.GetFloatValue(),
                                                _moistOctaves.GetIntValue(), 
                                                _moistPeriod.GetFloatValue(), 
                                                _moistPersistence.GetFloatValue());
        _confirmAction?.Invoke(prms);
    }
}
