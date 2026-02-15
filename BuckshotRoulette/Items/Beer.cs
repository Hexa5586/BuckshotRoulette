using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.States;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Items;

public class Beer : Item
{
    public Beer(string name) : base()
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
   
        var bullet = context.Game.PopMagazine();
        context.Player.PopKnowledge();
        context.Dealer.PopKnowledge();
        Debug.WriteLine($"{Name} used: Popped out a {bullet} bullet.");

        if (context.Game.Magazine.Count == 0)
        {
            Debug.WriteLine($"The magazine is empty! Refilling magazine and items...");
            context.CurrentState = new States.GameStates.ItemsReloadingState();
            context.Game.TurnCount = 0;
        }
    }
}