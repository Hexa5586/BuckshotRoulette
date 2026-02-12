using System;
using System.Collections.Generic;
using System.Linq;

namespace BuckshotRoulette.Simplified.Utilities;

/// <summary>
/// Provides various randomization algorithms for game logic.
/// </summary>
public static class RandomizeTools
{
    private static readonly Random _rand = new();

    /// <summary>
    /// Now uses RandomChoose to select between two items.
    /// </summary>
    public static T DrawAOrB<T>(double probabilityOfA, T objA, T objB)
    {
        var candidates = new List<T> { objA, objB };
        var weights = new List<double> { probabilityOfA, 1.0 - probabilityOfA };

        return RandomChoose(candidates, weights, 1).FirstOrDefault();
    }

    /// <summary>
    /// Fills a list while respecting minimum quotas, using RandomChoose for the probabilistic parts.
    /// </summary>
    public static List<T> FillWithAOrB<T>(double probabilityOfA, int count, int leastACount, int leastBCount, T objA, T objB)
    {
        if (leastACount + leastBCount > count)
            throw new ArgumentException("Sum of minimum counts exceeds total count.");

        var result = new List<T>(count);
        int currentACount = 0;
        int currentBCount = 0;

        for (int i = 0; i < count; i++)
        {
            int remainingSlots = count - i;
            T selected;

            // Check if we are FORCED to pick A to meet the minimum
            if (currentACount < leastACount && remainingSlots <= (leastACount - currentACount) + Math.Max(0, leastBCount - currentBCount))
            {
                selected = objA;
            }
            // Check if we are FORCED to pick B to meet the minimum
            else if (currentBCount < leastBCount && remainingSlots <= (leastBCount - currentBCount))
            {
                selected = objB;
            }
            else
            {
                selected = DrawAOrB(probabilityOfA, objA, objB);
            }

            if (EqualityComparer<T>.Default.Equals(selected, objA)) currentACount++;
            else currentBCount++;

            result.Add(selected);
        }

        result = result.OrderBy(x => _rand.Next()).ToList(); // Shuffle the result to avoid predictable patterns
        return result;
    }

    /// <summary>
    /// Core weighted selection logic.
    /// </summary>
    public static List<T> RandomChoose<T>(List<T> candidates, List<double> weights, int count = 1)
    {
        if (candidates.Count != weights.Count)
            throw new ArgumentException("Candidates and weights count mismatch.");

        var results = new List<T>();
        double totalWeight = weights.Sum();

        for (int i = 0; i < count; i++)
        {
            double r = _rand.NextDouble() * totalWeight;
            double cumulative = 0;
            for (int j = 0; j < candidates.Count; j++)
            {
                cumulative += weights[j];
                if (r <= cumulative)
                {
                    results.Add(candidates[j]);
                    break;
                }
            }
        }
        return results;
    }
}