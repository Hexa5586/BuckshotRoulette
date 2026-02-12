using BuckshotRoulette.Simplified.Renderers;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Contexts;

public static class RenderContext
{
    public static int UI_WIDTH { get; private set; } = 150; // UI width in characters
    public static int UI_HEIGHT { get; private set; } = 35; // UI height in lines, should be an odd number
    public static int HP_BAR_WIDTH { get; private set; } = 40; // Width of the HP bar in characters
    public static int GAMING_NOTNULL_HEIGHT { get; private set; } = 27; // Number of lines occupied by non-blank content (dealer info, magazine info, player info)
    public static int SPLASH_NOTNULL_HEIGHT { get; private set; } = 12; // Number of lines occupied by non-blank content (title, borders, instructions)
    public static int CONFIG_NOTNULL_HEIGHT { get; private set; } = 7; // Number of lines occupied by non-blank content (title, borders, instructions, config options)
    public static int LOGO_HEIGHT { get; private set; } = 8; // Number of lines the logo occupies
    public static int LOGO_WIDTH { get; private set; } = 114; // Approximate width of the logo in characters
    public static int COMMAND_AREA_RESERVE_HEIGHT { get; private set; } = 5; // Minimum width to properly display the logo and borders
    public static int BORDER_WIDTH { get; private set; } = 2; // Width of the borders on each side
    public static string GAMING_TITLE { get; private set; } = "  Buckshot Roulette Simplified Edition - Gaming";
    public static string SPLASH_TITLE { get; private set; } = "  Buckshot Roulette Simplified Edition";
    public static string CONFIG_TITLE { get; private set; } = "  Buckshot Roulette Simplified Edition - Configuration";
    public static string FOOTER { get; private set; } = "   By Hoshi-Inori";

    public const char BORDER_FULL = '\u2588'; // █
    public const char BORDER_SHADE = '\u2592'; // ▒
    public const char BORDER_LIGHT = '\u2591'; // ░
    public readonly static Dictionary<ConsoleColor, string> ANSI_COLOR_MAP = new()
    {
        { ConsoleColor.Black,        "\u001b[30m" },
        { ConsoleColor.DarkRed,      "\u001b[31m" },
        { ConsoleColor.DarkGreen,    "\u001b[32m" },
        { ConsoleColor.DarkYellow,   "\u001b[33m" },
        { ConsoleColor.DarkBlue,     "\u001b[34m" },
        { ConsoleColor.DarkMagenta,  "\u001b[35m" },
        { ConsoleColor.DarkCyan,     "\u001b[36m" },
        { ConsoleColor.Gray,         "\u001b[37m" },

        { ConsoleColor.DarkGray,     "\u001b[90m" },
        { ConsoleColor.Red,          "\u001b[91m" },
        { ConsoleColor.Green,        "\u001b[92m" },
        { ConsoleColor.Yellow,       "\u001b[93m" },
        { ConsoleColor.Blue,         "\u001b[94m" },
        { ConsoleColor.Magenta,      "\u001b[95m" },
        { ConsoleColor.Cyan,         "\u001b[96m" },
        { ConsoleColor.White,        "\u001b[97m" }
    };
    public const string ANSI_COLOR_RESET = "\u001b[0m";

    public static string LOGO { get; private set; } = @"
__________               __           .__            __    __________             .__          __    __          
\______   \__ __   ____ |  | __  _____|  |__   _____/  |_  \______   \ ____  __ __|  |   _____/  |__/  |_  ____  
 |    |  _/  |  \_/ ___\|  |/ / /  ___/  |  \ /  _ \   __\  |       _//  _ \|  |  \  | _/ __ \   __\   __\/ __ \ 
 |    |   \  |  /\  \___|    <  \___ \|   Y  (  <_> )  |    |    |   (  <_> )  |  /  |_\  ___/|  |  |  | \  ___/ 
 |______  /____/  \___  >__|_ \/____  >___|  /\____/|__|    |____|_  /\____/|____/|____/\___  >__|  |__|  \___  >
        \/            \/     \/     \/     \/                      \/                       \/                \/ 
";

    public static void AutoAdjustUISize()
    {
        UI_WIDTH = Console.WindowWidth;
        UI_HEIGHT = Console.WindowHeight; // Leave some space for command input
    }

    public static void PrintLogo()
    {
        AutoAdjustUISize();

        string[] lines = LOGO.Split('\n');
        foreach (var line in lines)
        {
            string cleanLine = line;
            Console.WriteLine(new string(' ', (UI_WIDTH - LOGO_WIDTH) / 2) + cleanLine);
        }
    }

    public static void PrintTitle(PageType type)
    {
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor = ConsoleColor.Black;
        
        switch (type)
        {
            case PageType.Splash:
                Console.WriteLine(SPLASH_TITLE + new string(' ', UI_WIDTH - SPLASH_TITLE.Length));
                break;
            case PageType.Gaming:
                Console.WriteLine(GAMING_TITLE + new string(' ', UI_WIDTH - GAMING_TITLE.Length));
                break;
            case PageType.Config:
                Console.WriteLine(CONFIG_TITLE + new string(' ', UI_WIDTH - CONFIG_TITLE.Length));
                break;
            default:
                break;
        }

        Console.ResetColor();
    }

    public static void PrintFooter()
    {
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(FOOTER + new string(' ', UI_WIDTH - FOOTER.Length));
        Console.ResetColor();
    }

    public static void PrintInnerBorder()
    {
        RenderingTools.PrintLine(BORDER_FULL + new string(BORDER_SHADE, UI_WIDTH - BORDER_WIDTH) + BORDER_FULL);
    }

    public static void PrintErrorMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            RenderingTools.PrintLine("ERROR: " + message);
            Console.ResetColor();
        }
    }

    public static int GetHalvedBlankLineCount(PageType type)
    {
        int nonNullHeight = type switch
        {
            PageType.Splash => SPLASH_NOTNULL_HEIGHT,
            PageType.Gaming => GAMING_NOTNULL_HEIGHT,
            PageType.Config => CONFIG_NOTNULL_HEIGHT,
            _ => 0
        };
        return (UI_HEIGHT - nonNullHeight - LOGO_HEIGHT - COMMAND_AREA_RESERVE_HEIGHT) / 2;
    }

    
}
