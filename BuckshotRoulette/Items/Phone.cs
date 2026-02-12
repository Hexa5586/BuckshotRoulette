using BuckshotRoulette.Simplified.Contexts;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

public class Phone : IItem
{
    public string Name => "Phone";

    public void Use(GlobalContext context, List<string> args)
    {
        // Argument count
        if (args.Count < 1)
        {
            throw new InvalidOperationException($"{Name} requires a bullet index.");
        }

        // Check index
        if (!int.TryParse(args[0], out int bulletIdx))
        {
            throw new InvalidOperationException($"Invalid bullet index for {Name}.");
        }

        var magazine = context.GetMagazine();
        if (bulletIdx < 0 || bulletIdx >= magazine.Count)
        {
            throw new InvalidOperationException($"Bullet index out of bounds for {Name}.");
        }

        // Cannot see current bullet
        if (bulletIdx == 0)
        {
            throw new InvalidOperationException($"The {Name} cannot see the current bullet (index 0).");
        }

        // Execute
        var bullet = magazine[bulletIdx];
        context.UpdateKnowledge(context.ActivePlayer, bulletIdx, bullet);
        Debug.WriteLine($"{Name} used: Bullet at index {bulletIdx} is a {bullet}.");
    }
}