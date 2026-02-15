using BuckshotRoulette.Simplified.Contexts;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

/// <summary>
/// Skips the opponent's next turn by applying the 'cuffed' status.
/// </summary>
public class Handcuffs : Item
{
    public Handcuffs(string name) : base()
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

        // Check Cooling Down
        if (context.GetActiveEntity().CuffCdLeft > 0)
        {
            throw new InvalidOperationException($"{Name} are cooling down for {context.GetActiveEntity().CuffCdLeft} more turn(s).");
        }

        // Execute
        context.Game.IsPassiveCuffed = true;
        context.GetActiveEntity().CuffCdLeft = context.Game.CuffUsingCd;
        Debug.WriteLine($"{Name} used: Cuffed {context.GetPassiveEntity().Name}. Handcuffs will cool down for 2 turns.");
    }
}