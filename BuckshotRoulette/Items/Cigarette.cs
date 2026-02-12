using BuckshotRoulette.Simplified.Contexts;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

public class Cigarette : IItem
{
    public string Name => "Cigarette";

    public void Use(GlobalContext context, List<string> args)
    {
        
        // Argument count
        if (args.Count != 0)
        {
            throw new InvalidOperationException($"{Name} does not take extra arguments.");
        }

        // Cannot use if health is already at max
        int currentHealth = context.GetHealth(context.ActivePlayer);
        int maxHealth = context.ActivePlayer == PlayerType.Player
            ? context.PlayerMaxHealth
            : context.DealerMaxHealth;

        if (currentHealth >= maxHealth)
        {
            throw new InvalidOperationException("Cannot use Cigarette with max HP.");
        }

        // Execute
        context.AdjustHealth(context.ActivePlayer, context.CigaretteCure);
        Debug.WriteLine($"{Name} used: Health {currentHealth} -> {context.GetHealth(context.ActivePlayer)}");
    }
}