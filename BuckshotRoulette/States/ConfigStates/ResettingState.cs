using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.States.ConfigStates;

public class ResettingState : State
{
    public int Handle(GlobalContext context)
    {
        context.CurrentState = new OperatingState(ConfigModeType.Reading);

        context.Configs.ResetToDefault();
        context.IsConfigModified = true;

        return 0;
    }
}