using BuckshotRoulette.Simplified.Contexts;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

/// <summary>
/// Increases the damage of the next bullet by a multiplier.
/// </summary>
public class Handsaw : IItem
{
    public string Name => "Handsaw";

    public void Use(GlobalContext context, List<string> args)
    {
        // Argument count
        if (args.Count != 0)
        {
            throw new InvalidOperationException($"{Name} does not take extra arguments.");
        }

        // Execute
        context.IsMultipleDamaged = true;
        Debug.WriteLine($"{Name} used: The next bullet will deal multiplied damage.");
    }
}