using BuckshotRoulette.Simplified.Items;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace BuckshotRoulette.Simplified.Contexts;

public class ConfigsContext
{
    private const string CONFIG_FILE_PATH = "BuckshotRoulette.configs.json";

    public static readonly IReadOnlyDictionary<string, object> DefaultConfigs = new Dictionary<string, object>
    {
        [nameof(Language)] = "en",
        [nameof(PlayerName)] = "PLAYER",
        [nameof(DealerName)] = "DEALER",
        [nameof(PlayerMaxHealth)] = 5,
        [nameof(DealerMaxHealth)] = 5,
        [nameof(MagazineSize)] = 5,
        [nameof(PlayerMaxItems)] = 6,
        [nameof(DealerMaxItems)] = 6,
        [nameof(PlayerItemsRefill)] = 4,
        [nameof(DealerItemsRefill)] = 4,
        [nameof(LeastRealCount)] = 1,
        [nameof(LeastBlankCount)] = 1,
        [nameof(RealProbability)] = 0.5,
        [nameof(RealBulletDamage)] = 1,
        [nameof(MedicineDamage)] = 1,
        [nameof(MedicineCure)] = 2,
        [nameof(MedicineValidProbability)] = 0.6,
        [nameof(CigaretteCure)] = 1,
        [nameof(HandsawMultiplier)] = 2,
        [nameof(CuffUsingCd)] = 2,
        [nameof(ItemWeights)] = new Dictionary<ItemType, double>
        {
            { ItemType.Magnifier,           0.16 },
            { ItemType.Beer,                0.12 },
            { ItemType.Handsaw,             0.08 },
            { ItemType.Cigarette,           0.16 },
            { ItemType.Handcuffs,           0.08 },
            { ItemType.Phone,               0.10 },
            { ItemType.Adrenaline,          0.08 },
            { ItemType.Inverter,            0.12 },
            { ItemType.Medicine,            0.10 }
        }
    };

    // Cache the PropertyInfo for each config property to optimize reflection access when resetting to default values
    private static readonly Dictionary<string, System.Reflection.PropertyInfo> _propCache =
    typeof(ConfigsContext)
        .GetProperties()
        .ToDictionary(p => p.Name);

    #region Config Properties
    public string Language { get; set; } = "";
    public string PlayerName { get; set; } = "";
    public string DealerName { get; set; } = "";
    public int PlayerMaxHealth { get; set; }
    public int DealerMaxHealth { get; set; }
    public int MagazineSize { get; set; }
    public int PlayerMaxItems { get; set; }
    public int DealerMaxItems { get; set; }
    public int PlayerItemsRefill { get; set; }
    public int DealerItemsRefill { get; set; }
    public int LeastRealCount { get; set; }
    public int LeastBlankCount { get; set; }
    public double RealProbability { get; set; }
    public int RealBulletDamage { get; set; }
    public int MedicineDamage { get; set; }
    public int MedicineCure { get; set; }
    public double MedicineValidProbability { get; set; }
    public int CigaretteCure { get; set; }
    public int HandsawMultiplier { get; set; }
    public int CuffUsingCd { get; set; }
    public Dictionary<ItemType, double> ItemWeights { get; set; } = new();
    #endregion

    [JsonIgnore] public int CurrentPage { get; set; } = 0;

    public void Initialize()
    {
        ResetToDefault();

        if (File.Exists(CONFIG_FILE_PATH))
        {
            ReadConfigs();
        }
        else
        {
            WriteConfigs();
        }
    }

    /// <summary>
    /// Reset all configs to default referenced from the DefaultConfigs dictionary.
    /// </summary>
    public void ResetToDefault()
    {
        Language = (string)DefaultConfigs[nameof(Language)];
        PlayerName = (string)DefaultConfigs[nameof(PlayerName)];
        DealerName = (string)DefaultConfigs[nameof(DealerName)];
        PlayerMaxHealth = (int)DefaultConfigs[nameof(PlayerMaxHealth)];
        DealerMaxHealth = (int)DefaultConfigs[nameof(DealerMaxHealth)];
        MagazineSize = (int)DefaultConfigs[nameof(MagazineSize)];
        PlayerMaxItems = (int)DefaultConfigs[nameof(PlayerMaxItems)];
        DealerMaxItems = (int)DefaultConfigs[nameof(DealerMaxItems)];
        PlayerItemsRefill = (int)DefaultConfigs[nameof(PlayerItemsRefill)];
        DealerItemsRefill = (int)DefaultConfigs[nameof(DealerItemsRefill)];
        LeastRealCount = (int)DefaultConfigs[nameof(LeastRealCount)];
        LeastBlankCount = (int)DefaultConfigs[nameof(LeastBlankCount)];
        RealProbability = (double)DefaultConfigs[nameof(RealProbability)];
        RealBulletDamage = (int)DefaultConfigs[nameof(RealBulletDamage)];
        MedicineDamage = (int)DefaultConfigs[nameof(MedicineDamage)];
        MedicineCure = (int)DefaultConfigs[nameof(MedicineCure)];
        MedicineValidProbability = (double)DefaultConfigs[nameof(MedicineValidProbability)];
        CigaretteCure = (int)DefaultConfigs[nameof(CigaretteCure)];
        HandsawMultiplier = (int)DefaultConfigs[nameof(HandsawMultiplier)];
        CuffUsingCd = (int)DefaultConfigs[nameof(CuffUsingCd)];

        // Deep copy the dictionary
        var defaultWeights = (Dictionary<ItemType, double>)DefaultConfigs[nameof(ItemWeights)];
        ItemWeights = new Dictionary<ItemType, double>(defaultWeights);
    }

    /// <summary>
    /// 
    /// </summary>
    [JsonIgnore]
    public readonly List<ConfigsGroup> Groups = new()
    {
        new() {
            Title = "BASIC SETTINGS",
            ConfigNames = new() {
                nameof(Language),
                nameof(PlayerName),
                nameof(DealerName),
            }
        },
        new() {
            Title = "OBJECT SETTINGS",
            ConfigNames = new() {
                nameof(MagazineSize),
                nameof(LeastRealCount),
                nameof(LeastBlankCount),
                nameof(RealProbability),
                nameof(RealBulletDamage),
                nameof(MedicineDamage),
                nameof(MedicineCure),
                nameof(MedicineValidProbability),
                nameof(CigaretteCure),
                nameof(HandsawMultiplier),
                nameof(CuffUsingCd),
            }
        },
        new()
        {
            Title = "ENTITIES SETTINGS",
            ConfigNames = new() {
                nameof(PlayerMaxHealth),
                nameof(DealerMaxHealth),
                nameof(PlayerMaxItems),
                nameof(DealerMaxItems),
                nameof(PlayerItemsRefill),
                nameof(DealerItemsRefill),
            }
        },
        new()
        {
            Title = "ITEM PROBABILITY SETTINGS",
            ConfigNames = new() {
                $"{nameof(ItemWeights)}.{nameof(ItemType.Adrenaline)}",
                $"{nameof(ItemWeights)}.{nameof(ItemType.Beer)}",
                $"{nameof(ItemWeights)}.{nameof(ItemType.Cigarette)}",
                $"{nameof(ItemWeights)}.{nameof(ItemType.Handcuffs)}",
                $"{nameof(ItemWeights)}.{nameof(ItemType.Handsaw)}",
                $"{nameof(ItemWeights)}.{nameof(ItemType.Inverter)}",
                $"{nameof(ItemWeights)}.{nameof(ItemType.Magnifier)}",
                $"{nameof(ItemWeights)}.{nameof(ItemType.Medicine)}",
                $"{nameof(ItemWeights)}.{nameof(ItemType.Phone)}",
            }
        }
    };

    public void ReadConfigs()
    {
        try
        {
            string json = File.ReadAllText(CONFIG_FILE_PATH);
            JsonConvert.PopulateObject(json, this);
        }
        catch (Exception ex)
        {
            throw new JsonReaderException($"Error reading configs: {ex.Message}");
        }
    }

    public void WriteConfigs()
    {
        try
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(CONFIG_FILE_PATH, json);
        }
        catch (Exception ex)
        {
            throw new JsonWriterException($"Error writing configs: {ex.Message}");
        }
    }

    public object GetConfigValue(string key)
    {
        // Nested paths for ItemWeights
        if (key.Contains('.'))
        {
            var parts = key.Split('.');
            if (parts[0] == nameof(ItemWeights) && Enum.TryParse(parts[1], out ItemType type))
            {
                return ItemWeights.TryGetValue(type, out var weight) ? weight : 0.0;
            }
        }

        if (_propCache.TryGetValue(key, out var prop))
        {
            return prop.GetValue(this) ?? 0;
        }
        else
        {
            throw new ArgumentException($"Config key '{key}' not found.");
        }
    }

    public void SetConfigValue(string key, string strValue)
    {
        object parsedValue;
        
        // Nested paths for ItemWeights
        if (key.Contains('.'))
        {
            var parts = key.Split('.');

            // Try to find the property of ItemWeights
            if (parts[0] == nameof(ItemWeights) && Enum.TryParse(parts[1], out ItemType type))
            {
                // Try parsing to double
                bool canParse = double.TryParse(strValue, out var weight);
                if (canParse)
                {
                    parsedValue = weight;
                }
                else
                {
                    throw new ArgumentException($"Invalid type of value for config '{key}': {strValue}");
                }

                // Validating the double value
                ValidateConfigValue(key, parsedValue);
                ItemWeights[type] = (double) parsedValue;

            }
            else
            {
                throw new ArgumentException($"Config key '{key}' not found.");
            }
        }
        // Try to find other properties
        else if (_propCache.TryGetValue(key, out var prop))
        {
            try
            {
                parsedValue = Convert.ChangeType(strValue, prop.PropertyType);
            }
            catch (FormatException)
            {
                throw new ArgumentException($"Invalid type of value for config '{key}': {strValue}");
            }

            ValidateConfigValue(key, parsedValue);
            prop.SetValue(this, parsedValue);
        }
        else
        {
            throw new ArgumentException($"Config key '{key}' not found.");
        }
    }

    private void ValidateConfigValue(string key, object value)
    {
        switch (key)
        {
            case nameof(PlayerName):
            case nameof(DealerName):
                {
                    string strValue = value as string ?? "";
                    if (string.IsNullOrWhiteSpace(strValue))
                    {
                        throw new ArgumentException($"Config '{key}' cannot be empty.");
                    }
                    if (strValue.Length > 20)
                    {
                        throw new ArgumentException($"Config '{key}' cannot exceed 20 characters.");
                    }
                    break;
                }
            
            case nameof(PlayerMaxHealth):
            case nameof(DealerMaxHealth):
            case nameof(MagazineSize):
            case nameof(RealBulletDamage):
            case nameof(HandsawMultiplier):
                {
                    int intValue = value is int iv ? iv : 0;
                    if (intValue < 1)
                    {
                        throw new ArgumentException($"Config '{key}' must be at least 1.");
                    }
                    break;
                }

            case nameof(PlayerMaxItems):
            case nameof(DealerMaxItems):
            case nameof(PlayerItemsRefill):
            case nameof(DealerItemsRefill):
            case nameof(MedicineDamage):
            case nameof(MedicineCure):
            case nameof(CigaretteCure):
            case nameof(CuffUsingCd):
                {
                    int intValue = value is int iv ? iv : 0;
                    if (intValue < 0)
                    {
                        throw new ArgumentException($"Config '{key}' cannot be negative.");
                    }
                    break;
                }

            case nameof(LeastRealCount):
                {
                    int intValue = value is int iv ? iv : 0;
                    if (intValue < 0)
                    {
                        throw new ArgumentException($"Config '{key}' cannot be negative.");
                    }
                    if (LeastBlankCount + intValue > MagazineSize)
                    {
                        throw new ArgumentException($"The sum of '{nameof(LeastRealCount)}' and '{nameof(LeastBlankCount)}' " +
                            $"cannot exceed '{nameof(MagazineSize)}'.");
                    }
                    break;
                }

            case nameof(LeastBlankCount):
                {
                    int intValue = value is int iv ? iv : 0;
                    if (intValue < 0)
                    {
                        throw new ArgumentException($"Config '{key}' cannot be negative.");
                    }
                    if (intValue + LeastRealCount > MagazineSize)
                    {
                        throw new ArgumentException($"The sum of '{nameof(LeastRealCount)}' and '{nameof(LeastBlankCount)}' " +
                            $"cannot exceed '{nameof(MagazineSize)}'.");
                    }
                    break;
                }

            case nameof(MedicineValidProbability):
            case nameof(RealProbability):
                {
                    double doubleValue = value is double dv ? dv : 0.0;
                    if (doubleValue < 0.0 || doubleValue > 1.0)
                    {
                        throw new ArgumentException($"Config '{key}' must be between 0.0 and 1.0.");
                    }
                    break;
                }

            case $"{nameof(ItemWeights)}.{nameof(ItemType.Adrenaline)}":
            case $"{nameof(ItemWeights)}.{nameof(ItemType.Beer)}":
            case $"{nameof(ItemWeights)}.{nameof(ItemType.Cigarette)}":
            case $"{nameof(ItemWeights)}.{nameof(ItemType.Handcuffs)}":
            case $"{nameof(ItemWeights)}.{nameof(ItemType.Handsaw)}":
            case $"{nameof(ItemWeights)}.{nameof(ItemType.Inverter)}":
            case $"{nameof(ItemWeights)}.{nameof(ItemType.Magnifier)}":
            case $"{nameof(ItemWeights)}.{nameof(ItemType.Medicine)}":
            case $"{nameof(ItemWeights)}.{nameof(ItemType.Phone)}":
                {
                    double doubleValue = value is double dv ? dv : 0.0;
                    if (doubleValue < 0.0)
                    {
                        throw new ArgumentException($"Config '{key}' cannot be negative.");
                    }
                    break;
                }

            default:
                Debug.WriteLine($"[WARNING] Validation for config '{key}' is not implemented.");
                break;
        }
    }
}