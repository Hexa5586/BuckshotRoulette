using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Renderers;
using BuckshotRoulette.Simplified.States;

namespace BuckshotRoulette.Simplified.States.SplashStates;

public class OperatingState : State
{
    public int Handle(GlobalContext context)
    {
        context.Configs.CurrentPage = 0; // Ensure the next time user enters configs, it starts from the first page

        SplashRenderer.Render(context);

        Console.Write(context.Locale.SPLASH_PROMPT);
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
                context.CurrentState = new ConfigStates.OperatingState(ConfigStates.ConfigModeType.Reading);
                break;
            case "close":
                context.CurrentState = new EndingState();
                break;
            default:
                throw new InvalidOperationException($"Unknown command: {tokens[0]}");
        }
    }
}
