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
        context.PlayerName = context.Configs.PlayerName;
        context.DealerName = context.Configs.DealerName;
        context.PlayerMaxHealth = context.Configs.PlayerMaxHealth;
        context.DealerMaxHealth = context.Configs.DealerMaxHealth;
        context.MagazineSize = context.Configs.MagazineSize;
        context.PlayerMaxItems = context.Configs.PlayerMaxItems;
        context.DealerMaxItems = context.Configs.DealerMaxItems;
        context.PlayerItemsRefill = context.Configs.PlayerItemsRefill;
        context.DealerItemsRefill = context.Configs.DealerItemsRefill;
        context.LeastRealCount = context.Configs.LeastRealCount;
        context.LeastBlankCount = context.Configs.LeastBlankCount;
        context.RealBulletDamage = context.Configs.RealBulletDamage;
        context.MedicineDamage = context.Configs.MedicineDamage;
        context.MedicineCure = context.Configs.MedicineCure;
        context.CigaretteCure = context.Configs.CigaretteCure;
        context.HandsawMultiplier = context.Configs.HandsawMultiplier;
        context.RealProbability = context.Configs.RealProbability;
        context.MedicineValidProbability = context.Configs.MedicineValidProbability;
        context.CuffUsingCd = context.Configs.CuffUsingCd;

        context.ItemProbabilityWeights = context.Configs.ItemWeights;
    }
}