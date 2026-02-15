using BuckshotRoulette.Simplified.Items;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Contexts;

public class GameContext : GameView
{
    private readonly GlobalContext _globalContext;

    public GameContext(GlobalContext globalContext)
    {
        _globalContext = globalContext;
    }

    private int _magazineSize;
    private int _leastRealCount;
    private int _leastBlankCount;
    private double _realProbability;
    private double _medicineValidProbability;
    private int _realBulletDamage;
    private int _medicineDamage;
    private int _medicineCure;
    private int _cigaretteCure;
    private int _handsawMultiplier;
    private int _cuffUsingCd;
    private Dictionary<ItemType, Double> _itemProbabilityWeights = new();

    public int MagazineSize { get => _magazineSize; set => _magazineSize = ValidationTools.EnsureNonNegative(value); }
    public int LeastRealCount { get => _leastRealCount; set => ValidateMagazineCounts(value, _leastBlankCount); }
    public int LeastBlankCount { get => _leastBlankCount; set => ValidateMagazineCounts(_leastRealCount, value); }
    public double RealProbability { get => _realProbability; set => _realProbability = ValidationTools.ValidateProbability(value); }
    public double MedicineValidProbability { get => _medicineValidProbability; set => _medicineValidProbability = ValidationTools.ValidateProbability(value); }
    public int RealBulletDamage { get => _realBulletDamage; set => _realBulletDamage = ValidationTools.EnsureNonNegative(value); }
    public int MedicineDamage { get => _medicineDamage; set => _medicineDamage = ValidationTools.EnsureNonNegative(value); }
    public int MedicineCure { get => _medicineCure; set => _medicineCure = ValidationTools.EnsureNonNegative(value); }
    public int CigaretteCure { get => _cigaretteCure; set => _cigaretteCure = ValidationTools.EnsureNonNegative(value); }
    public int HandsawMultiplier { get => _handsawMultiplier; set => _handsawMultiplier = ValidationTools.EnsureNonNegative(value); }
    public int CuffUsingCd { get => _cuffUsingCd; set => _cuffUsingCd = ValidationTools.EnsureNonNegative(value); }
    public Dictionary<ItemType, Double> ItemProbabilityWeights => _itemProbabilityWeights;
    IReadOnlyDictionary<ItemType, double> GameView.ItemProbabilityWeights => ItemProbabilityWeights;

    private List<BulletType> _magazine = new();
    private bool _isMultipleDamaged;
    private bool _isPassiveCuffed;
    private int _turnCount;

    public List<BulletType> Magazine { get => _magazine; }
    IReadOnlyList<BulletType> GameView.Magazine => _magazine;

    public bool IsMultipleDamaged { get => _isMultipleDamaged; set => _isMultipleDamaged = value; }
    public bool IsPassiveCuffed { get => _isPassiveCuffed; set => _isPassiveCuffed = value; }
    public int TurnCount { get => _turnCount; set => _turnCount = ValidationTools.EnsureNonNegative(value); }


    public EntityType ActiveEntity { get; set; }
    public EntityType PassiveEntity => ActiveEntity == EntityType.Player ? EntityType.Dealer : EntityType.Player;


    private void ValidateMagazineCounts(int real, int blank)
    {
        if (real < 0 || blank < 0) throw new ArgumentOutOfRangeException("Counts must be non-negative.");
        if (real + blank > _magazineSize)
            throw new ArgumentException("LeastRealCount + LeastBlankCount cannot exceed MagazineSize.");

        _leastRealCount = real;
        _leastBlankCount = blank;
    }

    public void SwitchActiveEntity()
    {
        ActiveEntity = (ActiveEntity == EntityType.Dealer) ? EntityType.Player : EntityType.Dealer;
    }

    public void InitializeMagazine(List<BulletType> newMagazine)
    {
        _magazine = newMagazine;
    }

    public void SwitchMagazineTop()
    {
        if (_magazine.Count > 0)
        {
            _magazine[0] = (_magazine[0] == BulletType.Real) ? BulletType.Blank : BulletType.Real;
        }
    }

    public BulletType PopMagazine()
    {
        if (_magazine.Count == 0) throw new InvalidOperationException("Magazine is empty");

        BulletType bullet = _magazine[0];
        _magazine.RemoveAt(0);
        return bullet;
    }

    public int GetRealBulletCount() => _magazine.Count(b => b == BulletType.Real);

    public void SetItemProbabilityWeights(Dictionary<ItemType, Double> weights)
    {
        _itemProbabilityWeights = weights;
    }

}
