using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.States;

/// <summary>
/// Defines the contract for all game states in the Buckshot Roulette state machine.
/// </summary>
public interface IState
{
    /// <summary>
    /// Processes the logic for the current state.
    /// </summary>
    /// <param name="context">The global game context.</param>
    /// <returns>An integer status code (0 to continue, 1 to exit).</returns>
    int Handle(GlobalContext context);
}