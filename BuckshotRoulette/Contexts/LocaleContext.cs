using Newtonsoft.Json;
using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Contexts;

public class LocaleContext
{
    public string GAMING_TITLE { get; set; } = "  Buckshot Roulette Simplified - In-Game";

    public string SPLASH_TITLE { get; set; } = "  Buckshot Roulette Simplified Edition";

    public string CONFIGS_TITLE { get; set; } = "  Buckshot Roulette Simplified - Settings";

    public string CONFIGS_MODIFIED_TITLE { get; set; } = "  Buckshot Roulette Simplified - Settings *";

    public string GAMING_FOOTER { get; set; } = "  [USAGE] SHOOT 0 (Self) / 1 (Opponent); ITEM <index> [args...]; QUIT.";

    public string SPLASH_FOOTER { get; set; } = "  Created by Hoshi-Inori";

    public string CONFIGS_READING_FOOTER { get; set; } = "  [USAGE] ←/→: Navigate; S: Save; Enter: Command Mode; Esc: Back to Menu.";

    public string CONFIGS_COMMAND_FOOTER { get; set; } = "  [USAGE] SET <key> <value>; PREV; NEXT; SAVE; RESET; QUIT.";

    public string SPLASH_LINE_CUE_1 { get; set; } = "Welcome! For the best experience, please MAXIMIZE your terminal window.";

    public string SPLASH_LINE_CUE_2 { get; set; } = "Tip: Press ENTER at any time to refresh the interface.";

    public string SPLASH_OPTION_1 { get; set; } = "[   START   ]   Begin the game        ";

    public string SPLASH_OPTION_2 { get; set; } = "[  CONFIGS  ]   Adjust settings       ";

    public string SPLASH_OPTION_3 { get; set; } = "[   EXIT    ]   Quit the program      ";

    public string UNKNOWN_VERSION_LABEL { get; set; } = "UNKNOWN VERSION";

    public string STATUS_LABEL { get; set; } = "STATUS";

    public string ITEMS_LABEL { get; set; } = "ITEMS";

    public string KNOWLEDGE_LABEL { get; set; } = "INTEL";

    public string EMPTY_LABEL { get; set; } = "EMPTY";

    public string MAGAZINE_LABEL { get; set; } = "CYLINDER";

    public string MAGAZINE_REAL_LABEL { get; set; } = "LIVE";

    public string MAGAZINE_TOTAL_LABEL { get; set; } = "TOTAL";

    public string MAGAZINE_DAMAGE_LABEL { get; set; } = "DMG";

    public string SPLASH_PROMPT { get; set; } = "[MENU] ACTION > ";

    public string GAMING_PROMPT { get; set; } = "[TURN {0}] {1} > ";

    public string CONFIGS_COMMAND_PROMPT { get; set; } = "[SETTINGS] COMMAND > ";

    public string ITEM_ADRENALINE_NAME { get; set; } = "Adrenaline";

    public string ITEM_BEER_NAME { get; set; } = "Beer";

    public string ITEM_CIGARETTE_NAME { get; set; } = "Cigarette";

    public string ITEM_HANDCUFFS_NAME { get; set; } = "Handcuffs";

    public string ITEM_HANDSAW_NAME { get; set; } = "Handsaw";

    public string ITEM_INVERTER_NAME { get; set; } = "Inverter";

    public string ITEM_MAGNIFIER_NAME { get; set; } = "Magnifier";

    public string ITEM_MEDICINE_NAME { get; set; } = "Medicine";

    public string ITEM_PHONE_NAME { get; set; } = "Phone";

    private string GetLocaleFilepath(string language)
    {
        return $"./locales/BuckshotRoulette.locale.{language}.json";
    }

    public void LoadLocale(string language)
    {
        string fileName = GetLocaleFilepath(language);
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        if (!File.Exists(fullPath))
        {
            Debug.WriteLine($"[Warning] Locale file not found: {fullPath}. Using default values.");
            return;
        }

        try
        {
            string jsonContent = File.ReadAllText(fullPath);
            JsonConvert.PopulateObject(jsonContent, this);
        }
        catch (Exception ex)
        {
            throw new JsonReaderException($"[Error] Failed to load locale: {ex.Message}");
        }
    }
}