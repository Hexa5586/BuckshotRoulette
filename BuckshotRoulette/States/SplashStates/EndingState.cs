using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.States.SplashStates;

/// <summary>
/// Represents the end of the game session.
/// </summary>
public class EndingState : IState
{
    public int Handle(GlobalContext context)
    {
        // Return 1 to signal the main loop to terminate
        return 1;
    }
}