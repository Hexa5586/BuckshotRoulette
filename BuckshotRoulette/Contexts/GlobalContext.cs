using BuckshotRoulette.Simplified.Items;
using BuckshotRoulette.Simplified.States;

namespace BuckshotRoulette.Simplified.Contexts;

/// <summary>
/// Central state container that holds all game data, player stats, 
/// and handles transition logic with strict validation constraints.
/// </summary>
public class GlobalContext
{
    // --- Sub Contexts and Status ---
    public RenderContext Render { get; private set; } = new RenderContext();
    public ConfigsContext Configs { get; private set; } = new ConfigsContext();
    public bool IsConfigModified { get; set; } = false;

    // --- Error Message ---
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


    // --- Backing Fields for Validation ---
    private int _magazineSize;
    private int _leastRealCount;
    private int _leastBlankCount;
    private double _realProbability;
    private double _medicineValidProbability;
    private int _playerMaxHealth;
    private int _dealerMaxHealth;
    private int _playerMaxItems;
    private int _dealerMaxItems;
    private int _playerItemsRefill;
    private int _dealerItemsRefill;
    private int _realBulletDamage;
    private int _medicineDamage;
    private int _medicineCure;
    private int _cigaretteCure;
    private int _handsawMultiplier;
    private int _cuffUsingCd;
    private Dictionary<ItemType, Double> _itemProbabilityWeights = new();

    // --- State Management ---
    public IState CurrentState { get; set; }

    // --- Player Data ---
    public string PlayerName { get; set; } = string.Empty;
    public string DealerName { get; set; } = "Dealer";

    private int _playerHealth;
    private int _dealerHealth;

    // --- Inventories and Magazine ---
    private List<IItem> _playerItems = new();
    private List<IItem> _dealerItems = new();
    private List<BulletType> _magazine = new();

    private List<BulletType> _playerKnowledge = new();
    private List<BulletType> _dealerKnowledge = new();

    // --- Game Flags & Active Player ---
    public bool IsMultipleDamaged { get; set; } = false;
    public bool IsPassiveCuffed { get; set; } = false;
    public int TurnCount { get; set; } = 0;
    public int PlayerCuffCdLeft { get; set; } = 0;
    public int DealerCuffCdLeft { get; set; } = 0;
    public PlayerType ActivePlayer { get; set; }

    // --- Constrained Properties ---

    public int MagazineSize { get => _magazineSize; set => _magazineSize = EnsureNonNegative(value); }
    public int LeastRealCount{ get => _leastRealCount; set => ValidateMagazineCounts(value, _leastBlankCount); }
    public int LeastBlankCount{ get => _leastBlankCount; set => ValidateMagazineCounts(_leastRealCount, value); }
    public double RealProbability{ get => _realProbability; set => _realProbability = ValidateProbability(value); }
    public double MedicineValidProbability{ get => _medicineValidProbability; set => _medicineValidProbability = ValidateProbability(value); }
    public int PlayerMaxHealth { get => _playerMaxHealth; set => _playerMaxHealth = EnsureNonNegative(value); }
    public int DealerMaxHealth { get => _dealerMaxHealth; set => _dealerMaxHealth = EnsureNonNegative(value); }
    public int PlayerMaxItems { get => _playerMaxItems; set => _playerMaxItems = EnsureNonNegative(value); }
    public int DealerMaxItems { get => _dealerMaxItems; set => _dealerMaxItems = EnsureNonNegative(value); }
    public int PlayerItemsRefill { get => _playerItemsRefill; set => _playerItemsRefill = EnsureNonNegative(value); }
    public int DealerItemsRefill { get => _dealerItemsRefill; set => _dealerItemsRefill = EnsureNonNegative(value); }
    public int RealBulletDamage { get => _realBulletDamage; set => _realBulletDamage = EnsureNonNegative(value); }
    public int MedicineDamage { get => _medicineDamage; set => _medicineDamage = EnsureNonNegative(value); }
    public int MedicineCure { get => _medicineCure; set => _medicineCure = EnsureNonNegative(value); }
    public int CigaretteCure { get => _cigaretteCure; set => _cigaretteCure = EnsureNonNegative(value); }
    public int HandsawMultiplier { get => _handsawMultiplier; set => _handsawMultiplier = EnsureNonNegative(value); }
    public int CuffUsingCd { get => _cuffUsingCd; set => _cuffUsingCd = EnsureNonNegative(value); }
    public Dictionary<ItemType, Double> ItemProbabilityWeights 
    { 
        get => new Dictionary<ItemType, double>(_itemProbabilityWeights);
        set => _itemProbabilityWeights = new Dictionary<ItemType, double>(value);
    }

    /// <summary>
    /// Initializes the game with the starting state.
    /// </summary>
    public GlobalContext()
    {
        CurrentState = new States.GameStates.InitializingState();
    }

    // --- Validation Helpers ---

    private int EnsureNonNegative(int value) =>
        value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), "Value must be non-negative.");

    private double ValidateProbability(double value) =>
        (value >= 0 && value <= 1) ? value : throw new ArgumentOutOfRangeException(nameof(value), "Probability must be between 0 and 1.");

    private void ValidateMagazineCounts(int real, int blank)
    {
        if (real < 0 || blank < 0) throw new ArgumentOutOfRangeException("Counts must be non-negative.");
        if (real + blank > _magazineSize)
            throw new ArgumentException("LeastRealCount + LeastBlankCount cannot exceed MagazineSize.");

        _leastRealCount = real;
        _leastBlankCount = blank;
    }

    // --- Health Logic ---

    public int GetHealth(PlayerType player) => player == PlayerType.Player ? _playerHealth : _dealerHealth;

    public void SetHealth(PlayerType player, int health)
    {
        int max = (player == PlayerType.Player) ? PlayerMaxHealth : DealerMaxHealth;
        int clampedValue = Math.Clamp(health, 0, max);

        if (player == PlayerType.Player) _playerHealth = clampedValue;
        else _dealerHealth = clampedValue;
    }

    public void AdjustHealth(PlayerType player, int delta) => SetHealth(player, GetHealth(player) + delta);

    public int GetMaxHealth(PlayerType player) => player == PlayerType.Player ? PlayerMaxHealth : DealerMaxHealth;

    // --- Turn & Player Logic ---

    public PlayerType PassivePlayer => ActivePlayer == PlayerType.Player ? PlayerType.Dealer : PlayerType.Player;

    public void SwitchActivePlayer() => ActivePlayer = (ActivePlayer == PlayerType.Dealer) ? PlayerType.Player : PlayerType.Dealer;

    public string GetName(PlayerType player) => player == PlayerType.Player ? PlayerName : DealerName;

    // --- Magazine Logic ---

    public void InitializeMagazine(List<BulletType> newMagazine)
    {
        _magazine = newMagazine;
        _playerKnowledge = Enumerable.Repeat(BulletType.Unknown, MagazineSize).ToList();
        _dealerKnowledge = Enumerable.Repeat(BulletType.Unknown, MagazineSize).ToList();
    }

    public BulletType PopMagazine()
    {
        if (_magazine.Count == 0) throw new InvalidOperationException("Magazine is empty");

        BulletType bullet = _magazine[0];
        _magazine.RemoveAt(0);
        _playerKnowledge.RemoveAt(0);
        _dealerKnowledge.RemoveAt(0);
        return bullet;
    }

    public void SwitchMagazineTop()
    {
        if (_magazine.Count > 0)
        {
            _magazine[0] = (_magazine[0] == BulletType.Real) ? BulletType.Blank : BulletType.Real;
        }
    }

    public List<BulletType> GetMagazine() => new(_magazine);

    public List<BulletType> GetKnowledge(PlayerType player) =>
        new((player == PlayerType.Player) ? _playerKnowledge : _dealerKnowledge);

    public void UpdateKnowledge(PlayerType player, int index, BulletType knowledge)
    {
        var list = (player == PlayerType.Player) ? _playerKnowledge : _dealerKnowledge;
        if (index >= 0 && index < list.Count)
        {
            list[index] = knowledge;
        }
    }

    public void CorrectKnowledge(PlayerType player)
    {
        var knowledgeList = (player == PlayerType.Player) ? _playerKnowledge : _dealerKnowledge;
        for (int i = 0; i < knowledgeList.Count; i++)
        {
            if (knowledgeList[i] != BulletType.Unknown && knowledgeList[i] != _magazine[i])
            {
                knowledgeList[i] = _magazine[i];
            }
        }
    }

    public int GetRemainingRealCount() => _magazine.Count(b => b == BulletType.Real);

    // --- Item Logic ---

    public void RefillItems(PlayerType player, bool initializing = false)
    {
        var inventory = (player == PlayerType.Player) ? _playerItems : _dealerItems;
        int count = inventory.Count;
        int max = (player == PlayerType.Player) ? PlayerMaxItems : DealerMaxItems;
        int refillCount = (player == PlayerType.Player) ? PlayerItemsRefill : DealerItemsRefill;

        if (initializing)
        {
            inventory.Clear();
            // Player shouldn't get adrenaline at the start
            var items = ItemFactory.CreateItemList(max, _itemProbabilityWeights, player == PlayerType.Player);
            inventory.AddRange(items);
        }
        else
        {
            var items = ItemFactory.CreateItemList(Math.Min(max - count, refillCount), _itemProbabilityWeights);
            inventory.AddRange(items);
        }
        
    }

    public void ExtendItems(PlayerType player, List<IItem> newItems)
    {
        var inventory = (player == PlayerType.Player) ? _playerItems : _dealerItems;
        int count = inventory.Count;
        int max = (player == PlayerType.Player) ? PlayerMaxItems : DealerMaxItems;

        var items = new List<IItem>(newItems);
        if (count + newItems.Count > max)
            items = items.Take(max - count).ToList();
        inventory.AddRange(newItems);
    }

    public IItem DrawItem(PlayerType player, int index)
    {
        var inventory = (player == PlayerType.Player) ? _playerItems : _dealerItems;
        if (index < 0 || index >= inventory.Count) throw new IndexOutOfRangeException("No such item");

        IItem item = inventory[index];
        inventory.RemoveAt(index);
        return item;
    }

    public List<IItem> GetItems(PlayerType player) =>
        new((player == PlayerType.Player) ? _playerItems : _dealerItems);

    public int GetCuffCdLeft(PlayerType player) => (player == PlayerType.Player) ? PlayerCuffCdLeft : DealerCuffCdLeft;

    public int AdjustCuffCdLeft(PlayerType player, int delta)
    {
        if (player == PlayerType.Player)
        {
            PlayerCuffCdLeft = Math.Max(0, PlayerCuffCdLeft + delta);
            return PlayerCuffCdLeft;
        }
        else
        {
            DealerCuffCdLeft = Math.Max(0, DealerCuffCdLeft + delta);
            return DealerCuffCdLeft;
        }
    }

    // --- Execution ---

    public int Execute() => CurrentState.Handle(this);
}