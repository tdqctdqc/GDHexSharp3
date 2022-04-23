using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HexWargame
{
public static class IEnumerableExt
{
    public static List<T> GetNRandomElements<T>(this IEnumerable<T> enumer, int numSamples)
    {
        if(numSamples > enumer.Count()) 
        {
            throw new Exception("trying to select more random elements than there are elements in the collection");
        }
        var samples = new List<T>();
        var freeElements = enumer.ToList();
        while(samples.Count < numSamples) 
        {
            var sample = freeElements[Game.I.Random.RandiRange(0, freeElements.Count - 1)];
            samples.Add(sample);
            freeElements.Remove(sample);
        }
        return samples; 
    }
    public static T GetRandomElement<T>(this IEnumerable<T> enumer)
    {
        return enumer.ElementAt(Game.I.Random.RandiRange(0, enumer.Count() - 1));
    }
    public static T GetMiddleElement<T>(this IEnumerable<T> enumer)
    {
        if(enumer.Count() == 0) return default(T);
        int middle = enumer.Count() / 2;
        return enumer.ElementAt(middle);
    }
}
}
