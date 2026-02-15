using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Renderers;
using BuckshotRoulette.Simplified.States;
using BuckshotRoulette.Simplified.Utilities;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BuckshotRoulette.Simplified.States.GameStates;

/// <summary>
/// The primary interactive state where the game displays information and waits for player input.
/// </summary>
public class OperatingState : IState
{
    public int Handle(GlobalContext context)
    {
        // Debugger outputs
#if DEBUG
        DebugOutput(context);
#endif

        // Render game board
        GameRenderer.RenderGaming(context);

        // Prompt player for input
        Console.Write($"[TURN {context.TurnCount}] {context.GetName(context.ActivePlayer)} > ");
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            Interpret(input, context);
        }

        return 0;
    }

    /// <summary>
    /// Parses input commands to transition the game to shooting or item usage states.
    /// </summary>
    public void Interpret(string command, GlobalContext context)
    {
        // Standardize input by trimming and reducing whitespace
        var tokens = command.ToLower().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        if (tokens.Count == 0) return;

        // Command routing using the new switch expression or standard switch
        switch (tokens[0])
        {
            case "shoot":
                context.CurrentState = new ShootingState(tokens.Skip(1).ToList());
                break;
            case "item":
                context.CurrentState = new ItemsEnablingState(tokens.Skip(1).ToList());
                break;
            case "quit":
                context.CurrentState = new SplashStates.OperatingState();
                break;
            default:
                throw new InvalidOperationException($"Unknown command: {tokens[0]}");
        }
    }

    [Conditional("DEBUG")]
    private void DebugOutput(GlobalContext context)
    {
        Debug.WriteLine($"\nTurn {context.TurnCount}: {context.GetName(context.ActivePlayer)}'s turn");
        Debug.WriteLine($"Player Health: {context.GetHealth(PlayerType.Player)}/{context.PlayerMaxHealth} "
            + $"Dealer Health: {context.GetHealth(PlayerType.Dealer)}/{context.DealerMaxHealth}");
        Debug.Write("Magazine: ");
        Debug.Write(string.Join(" ", context.GetMagazine().Select(bullet => bullet.GetAscii())));
        Debug.Write("Player Items: ");
        Debug.WriteLine(string.Join(" ", context.GetItems(PlayerType.Player).Select(item => item.Name)));
        Debug.Write("Dealer Items: ");
        Debug.WriteLine(string.Join(" ", context.GetItems(PlayerType.Dealer).Select(item => item.Name)));

        Debug.Write($"{context.PlayerName}'s Knowledge: ");
        Debug.WriteLine(string.Join(" ", context.GetKnowledge(PlayerType.Player).Select(knowledge => knowledge.GetAscii())));
        
        Debug.Write($"{context.DealerName}'s Knowledge: ");
        Debug.WriteLine(string.Join(" ", context.GetKnowledge(PlayerType.Dealer).Select(knowledge => knowledge.GetAscii())));
    }
}