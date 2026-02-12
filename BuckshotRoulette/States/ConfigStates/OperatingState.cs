using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Renderers;
using BuckshotRoulette.Simplified.States;

namespace BuckshotRoulette.Simplified.States.ConfigStates;

public class OperatingState : IState
{
    public int Handle(GlobalContext context)
    {
        ConfigRenderer.RenderConfig(context);

        Console.WriteLine("");
        Console.Write("[CONFIG] COMMAND > ");
        Console.ReadLine();

        context.CurrentState = new SplashStates.OperatingState();
        return 0;
    }
}
