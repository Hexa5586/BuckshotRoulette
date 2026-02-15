using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Renderers;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.States.GameStates;

/// <summary>
/// Handles the resolution of a shot, including damage calculation and turn logic.
/// </summary>
/// <param name="args">Arguments where args[0] indicates the target (0 for opponent, 1 for self).</param>
public class ShootingState(List<string> args) : State
{
    private readonly List<string> _args = args;

    public int Handle(GlobalContext context)
    {
        context.CurrentState = new OperatingState(); // Switching to Operating State first to prevent deadlock

        // Remove the top bullet from the magazine
        BulletType bullet = context.Game.PopMagazine();
        context.Player.PopKnowledge();
        context.Dealer.PopKnowledge();

        // Calculate final damage based on bullet type and active modifiers (like Handsaw)
        int multiplier = context.Game.IsMultipleDamaged ? context.Game.HandsawMultiplier : 1;
        int originalDamage = bullet == BulletType.Real ? context.Game.RealBulletDamage : 0;
        int damage = originalDamage * multiplier;

        // Identify the target (0/None = Passive/Opponent, 1 = Active/Self)
        int targetNum = 0;
        bool hasTarget = int.TryParse(_args.FirstOrDefault("0"), out targetNum);
        EntityContext target = targetNum == 0 ? context.GetPassiveEntity() : context.GetActiveEntity();

        int previousHealth = target.Health;
        target.AdjustHealth(-damage);
        int currentHealth = target.Health;

        // Feedback for the console
        Debug.WriteLine($"BOOM! {context.GetActiveEntity().Name} shot a {bullet} bullet at {target.Name}!");
        Debug.WriteLine($"{target.Name}: {previousHealth} -> {currentHealth}");


        bool isSelfBlank = (targetNum == 1 && bullet == BulletType.Blank);

        bool doUpdateTurn = !isSelfBlank;

        bool doReleaseCuff = context.Game.IsPassiveCuffed && !isSelfBlank;

        bool doSwitch = !isSelfBlank && !context.Game.IsPassiveCuffed;

        bool doCutCd = (targetNum == 0) || (targetNum == 1 && bullet == BulletType.Real);

        if (doSwitch)
        {
            context.Game.SwitchActiveEntity();
        }

        if (doCutCd)
        {
            if (context.Player.CuffCdLeft > 0)
            {
                context.Player.CuffCdLeft -= 1;
            }
            if (context.Dealer.CuffCdLeft > 0)
            {
                context.Dealer.CuffCdLeft -= 1;
            }
        }

        if (doReleaseCuff)
        {
            context.Game.IsPassiveCuffed = false;
        }
        
        if (doUpdateTurn)
        {
            context.Game.TurnCount += 1;
        }
        
        // Reset temporary combat flags
        context.Game.IsMultipleDamaged = false;

        if (context.Player.Health <= 0)
        {
            GameRenderer.Render(context);
            Console.WriteLine($"{context.Dealer.Name} won! Next game...");
            Console.ReadLine();
            context.CurrentState = new InitializingState();
            return 0;
        }

        if (context.Dealer.Health <= 0)
        {
            GameRenderer.Render(context);
            Console.WriteLine($"{context.Player.Name} won! Next game...");
            Console.ReadLine();
            context.CurrentState = new InitializingState();
            return 0;
        }

        if (context.Game.Magazine.Count <= 0)
        {
            Debug.WriteLine($"The magazine is empty! Refilling magazine and items...");
            context.CurrentState = new ItemsReloadingState(false);
            context.Game.TurnCount = 0;
            return 0;
        }
        
        context.CurrentState = new OperatingState();
        return 0;
    }
}