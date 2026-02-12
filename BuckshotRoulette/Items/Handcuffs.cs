using BuckshotRoulette.Simplified.Contexts;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

/// <summary>
/// Skips the opponent's next turn by applying the 'cuffed' status.
/// </summary>
public class Handcuffs : IItem
{
    public string Name => "Handcuffs";

    public void Use(GlobalContext context, List<string> args)
    {
        // Argument count
        if (args.Count != 0)
        {
            throw new InvalidOperationException($"{Name} does not take extra arguments.");
        }

        // Check Cooling Down
        if (context.GetCuffCdLeft(context.ActivePlayer) > 0)
        {
            throw new InvalidOperationException($"{Name} are cooling down for {context.GetCuffCdLeft(context.ActivePlayer)} more turn(s).");
        }

        // Execute
        context.IsPassiveCuffed = true;
        context.AdjustCuffCdLeft(context.ActivePlayer, context.CuffUsingCd);
        Debug.WriteLine($"{Name} used: Cuffed {context.GetName(context.PassivePlayer)}. Handcuffs will cool down for 2 turns.");
    }
}