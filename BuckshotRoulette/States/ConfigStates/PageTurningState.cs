using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.States.ConfigStates;

public class PageTurningState : State
{
    private int _step = 0;

    public PageTurningState(int step)
    {
        _step = step;
    }

    public int Handle(GlobalContext context)
    {
        context.CurrentState = new OperatingState(ConfigModeType.Reading);

        int nowPage = (context.Configs.CurrentPage + _step) % context.Configs.Groups.Count;
        while (nowPage < 0) nowPage += context.Configs.Groups.Count;

        context.Configs.CurrentPage = nowPage;
        return 0;
    }
    
}