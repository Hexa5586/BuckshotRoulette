using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Renderers;
using BuckshotRoulette.Simplified.States;
using System.Net.Http.Headers;

namespace BuckshotRoulette.Simplified.States.ConfigStates;

public class OperatingState : IState
{
    private ConfigModeType _mode;

    public OperatingState(ConfigModeType mode)
    {
        _mode = mode;
    }

    public int Handle(GlobalContext context)
    {
        switch (_mode)
        {
            case ConfigModeType.Command:
                HandleCommandMode(context);
                break;
            case ConfigModeType.Reading:
                HandleReadingMode(context);
                break;
        }

        return 0;
    }

    private void HandleCommandMode(GlobalContext context)
    {
        ConfigsRenderer.RenderConfig(context, _mode, context.IsConfigModified);

        Console.Write("[CONFIGS] COMMANDS > ");
        string? input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            Interpret(input, context);
        }
    }

    private void HandleReadingMode(GlobalContext context)
    {
        ConfigsRenderer.RenderConfig(context, _mode, context.IsConfigModified);

        var keyInfo = Console.ReadKey(true);

        switch (keyInfo.Key)
        {
            case ConsoleKey.LeftArrow:
                context.CurrentState = new PageTurningState(-1);
                break;
            case ConsoleKey.RightArrow:
                context.CurrentState = new PageTurningState(1);
                break;
            case ConsoleKey.S:
                context.CurrentState = new SavingState();
                break;
            case ConsoleKey.Enter:
                context.CurrentState = new OperatingState(ConfigModeType.Command);
                break;
            case ConsoleKey.Escape:
                context.CurrentState = new EndingState();
                break;
        }
    }

    private void Interpret(string command, GlobalContext context)
    {
        var tokens = command.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        if (tokens.Count == 0) return;

        switch (tokens[0])
        {
            case "quit":
                context.CurrentState = new EndingState();
                break;
            case "save":
                context.CurrentState = new SavingState();
                break;
            case "reset":
                context.CurrentState = new ResettingState();
                break;
            case "prev":
                context.CurrentState = new PageTurningState(-1);
                break;
            case "next":
                context.CurrentState = new PageTurningState(1);
                break;
            case "set":
                context.CurrentState = new SettingState(tokens.Skip(1).ToList());
                break;
            default:
                throw new InvalidOperationException($"Unknown command: {command}");
        }
    }
}
