using BuckshotRoulette.Simplified.Items;

namespace BuckshotRoulette.Simplified.Contexts;

public interface GameView
{
    int MagazineSize { get; }
    int LeastRealCount { get; }
    int LeastBlankCount { get; }
    double RealProbability { get; }
    double MedicineValidProbability { get; }
    int RealBulletDamage { get; }
    int MedicineDamage { get; }
    int MedicineCure { get; }
    int CigaretteCure { get; }
    int HandsawMultiplier { get; }
    int CuffUsingCd { get; }
    
    bool IsMultipleDamaged { get; }
    bool IsPassiveCuffed { get; }
    int TurnCount { get; }
    EntityType ActiveEntity { get; }
    EntityType PassiveEntity { get; }

    int GetRealBulletCount();
    IReadOnlyList<BulletType> Magazine { get; }
    IReadOnlyDictionary<ItemType, double> ItemProbabilityWeights { get; }
}