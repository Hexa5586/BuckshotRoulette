using BuckshotRoulette.Simplified.Contexts;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

/// <summary>
/// Allows the player to see the type of the next bullet in the magazine.
/// </summary>
public class Magnifier : Item
{
    public Magnifier(string name) : base()
    {
        Name = name;
    }

    public string Name { get; }

    public void Use(GlobalContext context, List<string> args)
    {
        // Argument count
        if (args.Count != 0)
        {
            throw new InvalidOperationException($"{Name} does not take extra arguments.");
        }

        // Execute
        var bullet = context.Game.Magazine.First();
        context.GetActiveEntity().UpdateKnowledge(0, bullet);
        Debug.WriteLine($"{Name} used: Next bullet is a {bullet.GetName()} bullet.");
    }
}