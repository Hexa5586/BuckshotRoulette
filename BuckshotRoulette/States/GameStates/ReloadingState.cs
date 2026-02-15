using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.States.GameStates;

/// <summary>
/// State responsible for filling the magazine with a randomized mix of real and blank bullets.
/// </summary>
public class ReloadingState : State
{
    public int Handle(GlobalContext context)
    {
        // Uses RandomizeTools to generate a new bullet sequence based on game probabilities
        var newMagazine = RandomizeTools.FillWithAOrB(
            context.Game.RealProbability,
            context.Game.MagazineSize,
            context.Game.LeastRealCount,
            context.Game.LeastBlankCount,
            BulletType.Real,
            BulletType.Blank
        );

        context.Game.InitializeMagazine(newMagazine);
        context.Player.InitializeKnowledge();
        context.Dealer.InitializeKnowledge();
        context.CurrentState = new OperatingState();
        return 0;
    }
}