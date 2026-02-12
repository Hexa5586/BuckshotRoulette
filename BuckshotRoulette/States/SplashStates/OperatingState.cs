using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Renderers;
using BuckshotRoulette.Simplified.States;

namespace BuckshotRoulette.Simplified.States.SplashStates;

public class OperatingState : IState
{
    public int Handle(GlobalContext context)
    {
        SplashRenderer.RenderSplashScreen(context);

        Console.WriteLine("");
        Console.Write("[SPLASH] GOTO > ");
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            Interpret(input, context);
        }

        return 0;
    }

    private void Interpret(string command, GlobalContext context)
    {
        // Standardize input by trimming and reducing whitespace
        var tokens = command.ToLower().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        if (tokens.Count == 0) return;
        switch (tokens[0])
        {
            case "start":
                context.CurrentState = new GameStates.InitializingState();
                break;
            case "configs":
                context.CurrentState = new ConfigStates.OperatingState();
                break;
            case "close":
                context.CurrentState = new EndingState();
                break;
            default:
                throw new InvalidOperationException($"Unknown command: {tokens[0]}");
        }
    }
}
