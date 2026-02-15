using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Items;

/// <summary>
/// Responsible for instantiating items based on type and generating random inventories.
/// </summary>
public class ItemFactory
{
    private readonly LocaleContext _locale;
    
    public ItemFactory(LocaleContext locale)
    {
        _locale = locale;
    }
    
    /// <summary>
    /// Creates a specific IItem instance based on the provided ItemType.
    /// </summary>
    public Item CreateItem(ItemType type) => type switch
    {
        ItemType.Adrenaline => new Adrenaline(_locale.ITEM_ADRENALINE_NAME),
        ItemType.Beer => new Beer(_locale.ITEM_BEER_NAME),
        ItemType.Cigarette => new Cigarette(_locale.ITEM_CIGARETTE_NAME),
        ItemType.Handcuffs => new Handcuffs(_locale.ITEM_HANDCUFFS_NAME),
        ItemType.Handsaw => new Handsaw(_locale.ITEM_HANDSAW_NAME),
        ItemType.Inverter => new Inverter(_locale.ITEM_INVERTER_NAME),
        ItemType.Magnifier => new Magnifier(_locale.ITEM_MAGNIFIER_NAME),
        ItemType.Medicine => new Medicine(_locale.ITEM_MEDICINE_NAME),
        ItemType.Phone => new Phone(_locale.ITEM_PHONE_NAME),
        _ => throw new ArgumentException($"Invalid item type: {type}")
    };

    /// <summary>
    /// Generates a list of random items using weighted probabilities.
    /// </summary>
    /// <param name="count">Number of items to generate.</param>
    /// <param name="itemWeights">A dictionary mapping ItemType to its appearance probability.</param>
    public List<Item> CreateItemList(int count, IReadOnlyDictionary<ItemType, Double> itemWeights, bool forbidAdrenaline = false)
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