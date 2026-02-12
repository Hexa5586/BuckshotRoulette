using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Items;

namespace BuckshotRoulette.Simplified.States.GameStates;

/// <summary>
/// Sets up the initial game configuration, health, and starting player.
/// </summary>
public class InitializingState : IState
{
    public int Handle(GlobalContext context)
    {
        ImportConfigs(context);

        context.SetHealth(PlayerType.Player, context.PlayerMaxHealth);
        context.SetHealth(PlayerType.Dealer, context.DealerMaxHealth);

        context.IsMultipleDamaged = false;
        context.IsPassiveCuffed = false;
        context.TurnCount = 0;
        context.ActivePlayer = PlayerType.Player;

        context.CurrentState = new ItemsReloadingState(initializing: true);
        return 0;
    }

    private void ImportConfigs(GlobalContext context)
    {
        // Debug configuration
        context.PlayerName = "PLAYER";
        context.DealerName = "DEALER";
        context.PlayerMaxHealth = 5;
        context.DealerMaxHealth = 5;
        context.MagazineSize = 5;
        context.PlayerMaxItems = 6;
        context.DealerMaxItems = 6;
        context.PlayerItemsRefill = 4;
        context.DealerItemsRefill = 4;
        context.LeastRealCount = 1;
        context.LeastBlankCount = 1;
        context.RealBulletDamage = 1;
        context.MedicineDamage = 1;
        context.MedicineCure = 2;
        context.CigaretteCure = 1;
        context.HandsawMultiplier = 2;
        context.RealProbability = 0.5;
        context.MedicineValidProbability = 0.6;
        context.CuffUsingCd = 2;
        context.ItemProbabilityWeights = new Dictionary<ItemType, double>
        {
            { ItemType.MagnifyingGlass, 0.18 },
            { ItemType.Beer,            0.15 },
            { ItemType.Handsaw,         0.12 },
            { ItemType.Cigarette,       0.12 },
            { ItemType.Handcuffs,       0.10 },
            { ItemType.BurnerPhone,     0.10 },
            { ItemType.Adrenaline,      0.08 },
            { ItemType.Inverter,        0.08 },
            { ItemType.ExpiredMedicine, 0.07 }
        };
    }
}