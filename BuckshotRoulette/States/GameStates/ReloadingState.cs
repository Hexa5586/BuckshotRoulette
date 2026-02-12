using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.States.GameStates;

/// <summary>
/// State responsible for filling the magazine with a randomized mix of real and blank bullets.
/// </summary>
public class ReloadingState : IState
{
    public int Handle(GlobalContext context)
    {
        // Uses RandomizeTools to generate a new bullet sequence based on game probabilities
        var newMagazine = RandomizeTools.FillWithAOrB(
            context.RealProbability,
            context.MagazineSize,
            context.LeastRealCount,
            context.LeastBlankCount,
            BulletType.Real,
            BulletType.Blank
        );

        context.InitializeMagazine(newMagazine);
        context.CurrentState = new OperatingState();
        return 0;
    }
}