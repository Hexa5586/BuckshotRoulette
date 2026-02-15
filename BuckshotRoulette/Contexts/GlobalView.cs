using BuckshotRoulette.Simplified.States;

namespace BuckshotRoulette.Simplified.Contexts;

public interface GlobalView
{
    LocaleContext Locale { get;  }
    GameView Game { get; }
    EntityView Player { get; }
    EntityView Dealer { get; }
    RenderContext Render { get; }
    ConfigsContext Configs { get; }
    bool IsConfigModified { get; }
    string ErrorMessage { get; }
    State CurrentState { get; }

    EntityView GetEntity(EntityType type);
    EntityView GetActiveEntity();
    EntityView GetPassiveEntity();
    string ConsumeErrorMessage();
}
