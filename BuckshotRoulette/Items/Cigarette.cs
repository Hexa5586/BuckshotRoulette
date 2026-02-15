using BuckshotRoulette.Simplified.Contexts;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

public class Cigarette : Item
{
    public Cigarette(string name) : base()
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
            throw new InvalidOperationException("Cannot use Cigarette with max HP.");
        }

        // Execute
        context.GetActiveEntity().AdjustHealth(context.Game.CigaretteCure);
        Debug.WriteLine($"{Name} used: Health {currentHealth} -> {context.GetActiveEntity().Health}");
    }
}