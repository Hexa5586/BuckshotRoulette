using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Items;

/// <summary>
/// Responsible for instantiating items based on type and generating random inventories.
/// </summary>
public static class ItemFactory
{
    /// <summary>
    /// Creates a specific IItem instance based on the provided ItemType.
    /// </summary>
    public static IItem CreateItem(ItemType type) => type switch
    {
        ItemType.Adrenaline => new Adrenaline(),
        ItemType.Beer => new Beer(),
        ItemType.BurnerPhone => new Phone(),
        ItemType.Cigarette => new Cigarette(),
        ItemType.ExpiredMedicine => new Medicine(),
        ItemType.Handcuffs => new Handcuffs(),
        ItemType.Handsaw => new Handsaw(),
        ItemType.Inverter => new Inverter(),
        ItemType.MagnifyingGlass => new Magnifier(),
        _ => throw new ArgumentException($"Invalid item type: {type}")
    };

    /// <summary>
    /// Generates a list of random items using weighted probabilities.
    /// </summary>
    /// <param name="count">Number of items to generate.</param>
    /// <param name="itemWeights">A dictionary mapping ItemType to its appearance probability.</param>
    public static List<IItem> CreateItemList(int count, Dictionary<ItemType, Double> itemWeights, bool forbidAdrenaline = false)
    {
        // Optionally filter out Adrenaline if it's forbidden
        var filteredWeights = new Dictionary<ItemType, Double>(itemWeights);
        if (forbidAdrenaline)
        {
            filteredWeights.Remove(ItemType.Adrenaline);
        }

        // Separate the Map into Candidates and Weights for RandomChoose
        var candidates = filteredWeights.Keys.Cast<object>().ToList();
        var weights = filteredWeights.Values.ToList();

        // Use the utility to pick the types based on the image probabilities
        var selectedTypes = RandomizeTools.RandomChoose(candidates, weights, count);

        // Map the selected types back to specific item instances
        return selectedTypes
            .Cast<ItemType>()
            .Select(CreateItem)
            .ToList();
    }
}