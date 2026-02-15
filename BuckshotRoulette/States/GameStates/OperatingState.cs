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
public class OperatingState : State
{
    public int Handle(GlobalContext context)
    {
        // Debugger outputs
#if DEBUG
        DebugOutput(context);
#endif

        // Render game board
        GameRenderer.Render(context);

        // Prompt player for input
        Console.Write(string.Format(context.Locale.GAMING_PROMPT, context.Game.TurnCount, context.GetActiveEntity().Name));
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
        Debug.WriteLine($"\nTurn {context.Game.TurnCount}: {context.GetActiveEntity().Name}'s turn");
        Debug.WriteLine($"Player Health: {context.Player.Health}/{context.Player.MaxHealth} "
            + $"Dealer Health: {context.Dealer.Health}/{context.Dealer.MaxHealth}");
        Debug.Write("Magazine: ");
        Debug.Write(string.Join(" ", context.Game.Magazine.Select(bullet => bullet.GetChar())));
        Debug.Write("Player Items: ");
        Debug.WriteLine(string.Join(" ", context.Player.Items.Select(item => item.Name)));
        Debug.Write("Dealer Items: ");
        Debug.WriteLine(string.Join(" ", context.Dealer.Items.Select(item => item.Name)));

        Debug.Write($"{context.Player.Name}'s Knowledge: ");
        Debug.WriteLine(string.Join(" ", context.Player.Knowledge.Select(knowledge => knowledge.GetChar())));
        
        Debug.Write($"{context.Dealer.Name}'s Knowledge: ");
        Debug.WriteLine(string.Join(" ", context.Player.Knowledge.Select(knowledge => knowledge.GetChar())));
    }
}