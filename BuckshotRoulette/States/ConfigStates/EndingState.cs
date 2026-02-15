using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.States.ConfigStates;

public class EndingState : State
{
    public int Handle(GlobalContext context)
    {
        context.CurrentState = new SplashStates.OperatingState();

        context.Configs.ReadConfigs();
        return 0;
    }
}