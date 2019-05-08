using System;
using System.Collections.Generic;
using System.Linq;

public static class WeightedChoice
{
    public static T SelectFromList<T>(IEnumerable<T> entries, Func<T, float> weightSelector)
    {
        var totalWeight = entries.Sum(weightSelector);
        var accumulatedWeight = 0f;
        
        var targetWeight = new Random().NextDouble() * totalWeight;
        
        foreach (var entry in entries)
        {
            accumulatedWeight += weightSelector(entry);

            if (accumulatedWeight >= targetWeight)
            {
                return entry;
            }
        }

        return default;
    }
}