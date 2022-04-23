using Godot;
using System;

public class GameGenerationParameters 
{
    
    public int Seed { get; private set; }
    public int NumContinents { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float PercentLand { get; private set; }
    public NoiseParameters AltNoiseParams { get; private set; }
    public NoiseParameters WetNoiseParams { get; private set; }

    public float RealWidth { get; private set; }
    public float RealHeight { get; private set; }
    public Vector2 RealSize => new Vector2(RealWidth, RealHeight);

    public GameGenerationParameters(int seed,
                                    int width, 
                                    int height, 
                                    int numContinents,
                                    float percentLand, 
                                    int altNoiseOctaves, 
                                    float altNoisePeriod, 
                                    float altNoisePersistence,
                                    int moistNoiseOctaves, 
                                    float moistNoisePeriod, 
                                    float moistNoisePersistence)
    {
        Seed = seed; 
        Width = width;
        RealWidth = width * 1.5f * Constants.HexRadius;
        NumContinents = numContinents;

        Height = height;
        RealHeight = (height + .5f) * .866f * 2f * Constants.HexRadius;

        PercentLand = percentLand;
        AltNoiseParams = new NoiseParameters(altNoiseOctaves, altNoisePeriod, altNoisePersistence);
        WetNoiseParams = new NoiseParameters(moistNoiseOctaves, moistNoisePeriod, moistNoisePersistence);
    }

    public static GameGenerationParameters GetDefaultParams()
    {
        return new GameGenerationParameters(seed: (int)Game.I.Random.Randi(),
                                            width: 100, 
                                            height: 100, 
                                            numContinents: 5, 
                                            percentLand: .5f, 
                                            altNoiseOctaves: 4, 
                                            altNoisePeriod: 20f, 
                                            altNoisePersistence: .5f,
                                            moistNoiseOctaves: 4, 
                                            moistNoisePeriod: 10f, 
                                            moistNoisePersistence: .5f);
    }
}
