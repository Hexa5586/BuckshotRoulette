using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

/// <summary>
/// A high-risk item that either heals significantly or causes damage based on probability.
/// </summary>
public class Medicine : Item
{
    public Medicine(string name) : base()
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

        // Cannot use if health is already at max
        int currentHealth = context.GetActiveEntity().Health;
        int maxHealth = context.GetActiveEntity().MaxHealth;

        if (currentHealth >= maxHealth)
        {
            throw new InvalidOperationException("Cannot use Medicine with max HP.");
        }

        // Execute
        // Probabilistically determine if the effect is a cure or damage
        int healthChange = RandomizeTools.DrawAOrB(
            context.Game.MedicineValidProbability,
            context.Game.MedicineCure,
            -context.Game.MedicineDamage
        );

        int preHealth = context.GetActiveEntity().Health;
        context.GetActiveEntity().AdjustHealth(healthChange);
        int nowHealth = context.GetActiveEntity().Health;

        string effect = healthChange > 0 ? "Healed" : "Hurt";
        Debug.WriteLine($"{Name} used: ({effect})! Health {preHealth} -> {nowHealth}");
    }
}