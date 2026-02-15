using BuckshotRoulette.Simplified.Items;
using BuckshotRoulette.Simplified.States;

namespace BuckshotRoulette.Simplified.Contexts;

/// <summary>
/// Central state container that holds all game data, player stats, 
/// and handles transition logic with strict validation constraints.
/// </summary>
public class GlobalContext : GlobalView
{
    /// <summary>
    /// Initializes the game with the starting state.
    /// </summary>
    public GlobalContext()
    {
        Locale = new LocaleContext();
        Render = new RenderContext(Locale);
        Configs = new ConfigsContext();
        Game = new GameContext(this);
        Player = new EntityContext(this, EntityType.Player);
        Dealer = new EntityContext(this, EntityType.Dealer);
        Factory = new ItemFactory(Locale);

        CurrentState = new States.GameStates.InitializingState();
    }

    /// <summary>
    /// Execute according to the current state.
    /// </summary>
    /// <returns></returns>
    public int Execute() => CurrentState.Handle(this);

    public LocaleContext Locale { get; private set; }
    public RenderContext Render { get; private set; }
    public ConfigsContext Configs { get; private set; }
    public GameContext Game { get; private set; }
    public EntityContext Player { get; private set; }
    public EntityContext Dealer { get; private set; }

    public ItemFactory Factory { get; private set; }

    LocaleContext GlobalView.Locale => Locale;
    GameView GlobalView.Game => Game;
    EntityView GlobalView.Player => Player;
    EntityView GlobalView.Dealer => Dealer;

    public bool IsConfigModified { get; set; } = false;
    
    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => _errorMessage = value ?? string.Empty;
    }

    public string ConsumeErrorMessage()
    {
        var msg = _errorMessage;
        _errorMessage = "";
        return msg;
    }
    
    public State CurrentState { get; set; }

    public EntityContext GetEntity(EntityType type) => type == EntityType.Player ? Player : Dealer;
    EntityView GlobalView.GetEntity(EntityType type) => GetEntity(type);

    public EntityContext GetActiveEntity() => GetEntity(Game.ActiveEntity);
    EntityView GlobalView.GetActiveEntity() => GetActiveEntity();

    public EntityContext GetPassiveEntity() => GetEntity(Game.PassiveEntity);
    EntityView GlobalView.GetPassiveEntity() => GetPassiveEntity();

    public GameView GetGameView() => Game;

    public EntityView GetEntityView(EntityType type) => type == EntityType.Player ? Player : Dealer;
}