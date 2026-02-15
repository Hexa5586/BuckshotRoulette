using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Items;

namespace BuckshotRoulette.Simplified.States.GameStates;

/// <summary>
/// Sets up the initial game configuration, health, and starting player.
/// </summary>
public class InitializingState : State
{
    public int Handle(GlobalContext context)
    {
        ImportConfigs(context);

        context.Player.SetHealth(context.Player.MaxHealth);
        context.Dealer.SetHealth(context.Dealer.MaxHealth);
        context.Player.Items.Clear();
        context.Dealer.Items.Clear();
        context.Player.Knowledge.Clear();
        context.Dealer.Knowledge.Clear();

        context.Game.IsMultipleDamaged = false;
        context.Game.IsPassiveCuffed = false;
        context.Game.TurnCount = 0;
        context.Game.ActiveEntity = EntityType.Player;

        context.CurrentState = new ItemsReloadingState(initializing: true);
        return 0;
    }

    private void ImportConfigs(GlobalContext context)
    {
        context.Player.Name = context.Configs.PlayerName;
        context.Dealer.Name = context.Configs.DealerName;
        context.Player.MaxHealth = context.Configs.PlayerMaxHealth;
        context.Dealer.MaxHealth = context.Configs.DealerMaxHealth;

        context.Game.MagazineSize = context.Configs.MagazineSize;

        context.Player.MaxItems = context.Configs.PlayerMaxItems;
        context.Dealer.MaxItems = context.Configs.DealerMaxItems;
        context.Player.ItemsRefill = context.Configs.PlayerItemsRefill;
        context.Dealer.ItemsRefill = context.Configs.DealerItemsRefill;

        context.Game.LeastRealCount = context.Configs.LeastRealCount;
        context.Game.LeastBlankCount = context.Configs.LeastBlankCount;
        context.Game.RealBulletDamage = context.Configs.RealBulletDamage;
        context.Game.MedicineDamage = context.Configs.MedicineDamage;
        context.Game.MedicineCure = context.Configs.MedicineCure;
        context.Game.CigaretteCure = context.Configs.CigaretteCure;
        context.Game.HandsawMultiplier = context.Configs.HandsawMultiplier;
        context.Game.RealProbability = context.Configs.RealProbability;
        context.Game.MedicineValidProbability = context.Configs.MedicineValidProbability;
        context.Game.CuffUsingCd = context.Configs.CuffUsingCd;

        context.Game.SetItemProbabilityWeights(context.Configs.ItemWeights);
    }
}