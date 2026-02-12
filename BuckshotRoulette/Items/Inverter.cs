using BuckshotRoulette.Simplified.Contexts;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

/// <summary>
/// Switches the type of the next bullet (Real to Blank, or vice versa).
/// </summary>
public class Inverter : IItem
{
    public string Name => "Inverter";

    public void Use(GlobalContext context, List<string> args)
    {
        // Argument count
        if (args.Count != 0) {
            throw new InvalidOperationException($"{Name} does not take extra arguments.");
        }

        context.SwitchMagazineTop();
        context.CorrectKnowledge(context.ActivePlayer);
        Debug.WriteLine($"{Name} used: The type of the next bullet has been inverted.");
    }
}