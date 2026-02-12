using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

/// <summary>
/// A high-risk item that either heals significantly or causes damage based on probability.
/// </summary>
public class Medicine : IItem
{
    public string Name => "Medicine";

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
            throw new InvalidOperationException("Cannot use Medicine with max HP.");
        }

        // Execute
        // Probabilistically determine if the effect is a cure or damage
        int healthChange = RandomizeTools.DrawAOrB(
            context.MedicineValidProbability,
            context.MedicineCure,
            -context.MedicineDamage
        );

        int preHealth = context.GetHealth(context.ActivePlayer);
        context.AdjustHealth(context.ActivePlayer, healthChange);

        string effect = healthChange > 0 ? "Healed" : "Hurt";
        Debug.WriteLine($"{Name} used ({effect}): Health {preHealth} -> {context.GetHealth(context.ActivePlayer)}");
    }
}