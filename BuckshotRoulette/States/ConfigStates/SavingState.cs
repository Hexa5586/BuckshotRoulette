using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.States.ConfigStates;

public class SavingState : IState
{
    public int Handle(GlobalContext context)
    {
        context.CurrentState = new OperatingState(ConfigModeType.Reading);

        context.Configs.WriteConfigs();
        context.IsConfigModified = false;
        return 0;
    }
}