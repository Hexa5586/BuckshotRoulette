using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Items;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.States.GameStates;

public class ItemsEnablingState(List<string> args) : State
{
    private readonly List<string> _args = args;

    public int Handle(GlobalContext context)
    {
        context.CurrentState = new OperatingState();

        
        if (_args.Count == 0) return 0;

        if (!int.TryParse(_args[0], out int itemIndex))
            throw new FormatException($"Invalid item index '{_args[0]}'.");

        var inventory = context.GetActiveEntity().Items;
        if (itemIndex < 0 || itemIndex >= inventory.Count)
            throw new ArgumentException($"Item index {itemIndex} is out of bounds.");

            
        Item item = inventory[itemIndex];
            
        var extraArgs = _args.Skip(1).ToList();

        item.Use(context, extraArgs);
        context.GetActiveEntity().DrawItem(itemIndex);

        return 0;
    }
}