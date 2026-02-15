using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Items;

namespace BuckshotRoulette.Simplified.States.GameStates;

/// <summary>
/// State responsible for replenishing items for both the player and the dealer.
/// </summary>
public class ItemsReloadingState : State
{
    private bool _initializing;
    public ItemsReloadingState(bool initializing = false)
    {
        _initializing = initializing;
    }

    public int Handle(GlobalContext context)
    {
        // Re-initialize items using the ItemFactory
        context.Player.RefillItems(_initializing);
        context.Dealer.RefillItems(_initializing);

        context.CurrentState = new ReloadingState();
        return 0;
    }
}