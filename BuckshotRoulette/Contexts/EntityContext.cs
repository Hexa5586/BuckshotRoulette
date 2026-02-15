using BuckshotRoulette.Simplified.Items;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Contexts;

public class EntityContext : EntityView
{
    private readonly GlobalContext _globalContext;
    private readonly EntityType _entityType;
    
    public EntityContext(GlobalContext globalContext, EntityType entityType)
    {
        _globalContext = globalContext;
        _entityType = entityType;
    }
    
    private int _maxHealth;
    private int _maxItems;
    private int _itemsRefill;
    private string _name = string.Empty;
    
    public string Name { get => _name; set => _name = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = ValidationTools.EnsureNonNegative(value); }
    public int MaxItems { get => _maxItems; set => _maxItems = ValidationTools.EnsureNonNegative(value); }
    public int ItemsRefill { get => _itemsRefill; set => _itemsRefill = ValidationTools.EnsureNonNegative(value); }


    private int _health;
    private List<Item> _items = new();
    private List<BulletType> _knowledge = new();
    private int _cuffCdLeft;

    public int Health { get => _health; }
    public List<Item> Items { get => _items; }
    public List<BulletType> Knowledge { get => _knowledge; }
    public int ItemsCount { get => _items.Count; }
    public int CuffCdLeft { get => _cuffCdLeft; set => _cuffCdLeft = value; }

    IReadOnlyList<Item> EntityView.Items => Items;
    IReadOnlyList<BulletType> EntityView.Knowledge => Knowledge;


    public void SetHealth(int health)
    {
        int clampedValue = Math.Clamp(health, 0, _maxHealth);
        _health = clampedValue;
    }

    public bool IsHealthCritical()
    {
        return _health * 1.0 / _maxHealth <= 0.25;
    }

    public void AdjustHealth(int delta) => SetHealth(_health + delta);

    public void InitializeKnowledge()
    {
        _knowledge = Enumerable.Repeat(BulletType.Unknown, _globalContext.GetGameView().MagazineSize).ToList();
    }

    public void SwitchKnowledgeTop()
    {
        if (_knowledge.Count > 0)
        {
            _knowledge[0] = (_knowledge[0] == BulletType.Real) ? BulletType.Blank : BulletType.Real;
        }
    }

    public BulletType PopKnowledge()
    {
        if (_knowledge.Count == 0) throw new InvalidOperationException("Magazine is empty");

        BulletType bullet = _knowledge[0];
        _knowledge.RemoveAt(0);
        return bullet;
    }

    public void UpdateKnowledge(int index, BulletType knowledge)
    {
        if (index >= 0 && index < _knowledge.Count)
        {
            _knowledge[index] = knowledge;
        }
    }

    public Item DrawItem(int index)
    {
        if (index < 0 || index >= _items.Count) throw new IndexOutOfRangeException("No such item");

        Item item = _items[index];
        _items.RemoveAt(index);
        return item;
    }

    public void RefillItems(bool initializing = false)
    {
        var gameView = _globalContext.GetGameView();
        int refillCount;
        if (initializing)
        {
            refillCount = MaxItems;
        }
        else
        {
            refillCount = Math.Min(MaxItems - _items.Count, ItemsRefill);
        }

        _items.AddRange(_globalContext.Factory.CreateItemList(refillCount, 
            gameView.ItemProbabilityWeights, _entityType == EntityType.Player && initializing));
    }

    public Dictionary<ConsoleColor, string> GetStatus()
    {
        var gameView = _globalContext.GetGameView();
        var status = new Dictionary<ConsoleColor, string>();
        bool isNoStatus = true;

        if (gameView.ActiveEntity != _entityType && gameView.IsPassiveCuffed)
        {
            status.Add(ConsoleColor.Red, "< CUFFED >");
            isNoStatus = false;
        }

        if (CuffCdLeft > 0)
        {
            status.Add(ConsoleColor.Yellow, $"< CUFFS COOLDOWN ({CuffCdLeft}) >");
            isNoStatus = false;
        }

        if (isNoStatus) status.Add(Console.ForegroundColor, "<EMPTY>");

        return status;
    }

}
