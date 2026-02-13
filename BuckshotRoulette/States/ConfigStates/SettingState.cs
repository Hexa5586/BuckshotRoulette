using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.States.ConfigStates;

public class SettingState : IState
{
    public readonly List<string> _args;

    public SettingState(List<string> args)
    {
        _args = args;
    }

    public int Handle(GlobalContext context)
    {
        
        if (_args.Count != 2)
        {
            throw new ArgumentException("Invalid number of arguments. Usage: set <setting> <value>");
        }

        string configName = _args[0];
        string configValue = _args[1];

        context.CurrentState = new OperatingState(ConfigModeType.Command);
        context.Configs.SetConfigValue(configName, configValue);
        context.IsConfigModified = true;
        context.CurrentState = new OperatingState(ConfigModeType.Reading);

        return 0;
    }

}
