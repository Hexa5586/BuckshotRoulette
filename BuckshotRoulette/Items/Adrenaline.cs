using BuckshotRoulette.Simplified.Contexts;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

public class Adrenaline : Item
{
    public Adrenaline(string name) : base()
    {
        Name = name;
    }

    public string Name { get; }

    public void Use(GlobalContext context, List<string> args)
    {
        // Argument count
        if (args.Count < 1)
            throw new InvalidOperationException($"{Name} requires a target item index from the opponent.");

        // Check index
        if (!int.TryParse(args[0], out int targetIdx))
            throw new InvalidOperationException($"Invalid item index for {Name}.");

        var opponentItems = context.GetPassiveEntity().Items;
        if (targetIdx < 0 || targetIdx >= opponentItems.Count)
            throw new InvalidOperationException($"Item index {targetIdx} out of bounds for {Name}.");

        // Cannot steal another Adrenaline
        if (opponentItems[targetIdx] is Adrenaline)
            throw new InvalidOperationException("You cannot use Adrenaline to steal another Adrenaline.");

        // Execute
        Item stolenItem = context.GetPassiveEntity().DrawItem(targetIdx);
        context.GetActiveEntity().Items.Add(stolenItem);

        Debug.WriteLine($"{Name} used: Stole [{stolenItem.Name}] from opponent.");
    }
}