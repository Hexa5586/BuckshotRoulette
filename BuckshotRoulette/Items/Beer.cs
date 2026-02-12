using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.States;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

public class Beer : IItem
{
    public string Name => "Beer";

    public void Use(GlobalContext context, List<string> args)
    {
        // Argument count
        if (args.Count != 0)
        {
            throw new InvalidOperationException($"{Name} does not take extra arguments.");
        }
   
        var bullet = context.PopMagazine();
        Debug.WriteLine($"{Name} used: Popped out a {bullet} bullet.");

        if (context.GetMagazine().Count == 0)
        {
            Debug.WriteLine($"The magazine is empty! Refilling magazine and items...");
            context.CurrentState = new States.GameStates.ItemsReloadingState();
            context.TurnCount = 0;
        }
    }
}