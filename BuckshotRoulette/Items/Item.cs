using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.Items;

public interface Item
{
    string Name { get; }
    void Use(GlobalContext context, List<string> args);
}