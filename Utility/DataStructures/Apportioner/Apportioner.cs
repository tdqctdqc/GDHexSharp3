using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

public class Apportioner<T,O>
{
    public static Dictionary<O, List<T>> Apportion(List<O> owners, List<T> elements, List<float> ownerWeights, int minElements)
    {
        // if(minElements > elements.Count / owners.Count)
        // {
        //     throw new Exception("not enough elements to fulfill minimum");
        // }
        var apportion = new Dictionary<O, List<T>>();
        
        var queue = new SimplePriorityQueue<O, float>();
        for (int i = 0; i < owners.Count; i++)
        {
            var o = owners[i];
            apportion.Add(o, new List<T>());
            queue.Enqueue(o, -ownerWeights[i]);
        }
        var els = new List<T>(elements);

        while(els.Count > 0)
        {
            var e = els[0];
            els.RemoveAt(0);
            var o = queue.First;
            int i = owners.IndexOf(o);
            apportion[o].Add(e);
            queue.UpdatePriority(o, -ownerWeights[i] / (apportion[o].Count + 1));
        }
        return apportion; 
    }
}
