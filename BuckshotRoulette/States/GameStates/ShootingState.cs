using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Renderers;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.States.GameStates;

/// <summary>
/// Handles the resolution of a shot, including damage calculation and turn logic.
/// </summary>
/// <param name="args">Arguments where args[0] indicates the target (0 for opponent, 1 for self).</param>
public class ShootingState(List<string> args) : IState
{
    private readonly List<string> _args = args;

    public int Handle(GlobalContext context)
    {
        context.CurrentState = new OperatingState(); // Switching to Operating State first to prevent deadlock

        // Remove the top bullet from the magazine
        BulletType bullet = context.PopMagazine();

        // Calculate final damage based on bullet type and active modifiers (like Handsaw)
        int multiplier = context.IsMultipleDamaged ? context.HandsawMultiplier : 1;
        int originalDamage = bullet == BulletType.Real ? context.RealBulletDamage : 0;
        int damage = originalDamage * multiplier;

        // Identify the target (0/None = Passive/Opponent, 1 = Active/Self)
        int targetNum = 0;
        bool hasTarget = int.TryParse(_args.FirstOrDefault("0"), out targetNum);
        PlayerType target = targetNum == 0 ? context.PassivePlayer : context.ActivePlayer;

        int previousHealth = context.GetHealth(target);
        context.AdjustHealth(target, -damage);
        int currentHealth = context.GetHealth(target);

        // Feedback for the console
        Debug.WriteLine($"BOOM! {context.GetName(context.ActivePlayer)} shot a {bullet} bullet at {context.GetName(target)}!");
        Debug.WriteLine($"{context.GetName(target)}: {previousHealth} -> {currentHealth}");

        // Switch turns unless the passive player is cuffed, or the active player shot a blank at themselves
        if (!(context.IsPassiveCuffed || targetNum == 1 && bullet == BulletType.Blank))
        {
            context.SwitchActivePlayer();
        }

        // Reset temporary combat flags
        context.IsMultipleDamaged = false;
        context.IsPassiveCuffed = false;

        // Cutting down remaining Handcuff cooling time
        if (context.GetCuffCdLeft(PlayerType.Player) > 0)
        {
            context.AdjustCuffCdLeft(PlayerType.Player, -1);
        }
        if (context.GetCuffCdLeft(PlayerType.Dealer) > 0)
        {
            context.AdjustCuffCdLeft(PlayerType.Dealer, -1);
        }

        // Increment turn count and check for end conditions
        context.TurnCount++;
        if (context.GetHealth(PlayerType.Player) <= 0)
        {
            GameRenderer.RenderGaming(context);
            Console.WriteLine($"{context.GetName(PlayerType.Dealer)} won! Next game...");
            Console.ReadLine();
            context.CurrentState = new InitializingState();
        }
        else if (context.GetHealth(PlayerType.Dealer) <= 0)
        {
            GameRenderer.RenderGaming(context);
            Console.WriteLine($"{context.GetName(PlayerType.Player)} won! Next game...");
            Console.ReadLine();
            context.CurrentState = new InitializingState();
        }
        else if (context.GetMagazine().Count <= 0)
        {
            Debug.WriteLine($"The magazine is empty! Refilling magazine and items...");
            context.CurrentState = new ItemsReloadingState();
            context.TurnCount = 0;
        }
        else
        {
            context.CurrentState = new OperatingState();
        }
        
        return 0;
    }
}