using BuckshotRoulette.Simplified.Items;

namespace BuckshotRoulette.Simplified.Contexts;

public interface EntityView
{
    string Name { get; }
    int MaxHealth { get; }
    int MaxItems { get; }
    int ItemsRefill { get; }
    int Health { get; }
    int ItemsCount { get; }
    int CuffCdLeft { get; }

    IReadOnlyList<Item> Items { get; }
    IReadOnlyList<BulletType> Knowledge { get; }

    bool IsHealthCritical();
    Dictionary<ConsoleColor, string> GetStatus();
}