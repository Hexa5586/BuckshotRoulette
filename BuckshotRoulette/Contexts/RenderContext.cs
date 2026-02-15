using BuckshotRoulette.Simplified.Renderers;
using BuckshotRoulette.Simplified.Utilities;
using System.Reflection;

namespace BuckshotRoulette.Simplified.Contexts;

public class RenderContext
{
    private readonly LocaleContext _locale;

    public RenderContext(LocaleContext locale)
    {
        _locale = locale;
    }
    
    public int UI_WIDTH { get; private set; } = 150; // UI width in characters
    public int UI_HEIGHT { get; private set; } = 35; // UI height in lines, should be an odd number
    public int HP_BAR_WIDTH { get; private set; } = 40; // Width of the HP bar in characters
    public int GAMING_NOTNULL_HEIGHT { get; private set; } = 25; // Number of lines occupied by non-blank content (dealer info, magazine info, player info)
    public int SPLASH_NOTNULL_HEIGHT { get; private set; } = 13; // Number of lines occupied by non-blank content (title, borders, instructions)
    public int CONFIG_ITEM_WIDTH { get; private set; } = 50; // Width allocated for item display in the config page
    public int LOGO_HEIGHT { get; private set; } = 8; // Number of lines the logo occupies
    public int LOGO_WIDTH { get; private set; } = 114; // Approximate width of the logo in characters
    public int COMMAND_AREA_RESERVE_HEIGHT { get; private set; } = 5; // Minimum width to properly display the logo and borders
    public int BORDER_WIDTH { get; private set; } = 2; // Width of the borders on each side
    
    public const char BORDER_FULL = '\u2588'; // █
    public const char BORDER_SHADE = '\u2592'; // ▒
    public const char BORDER_LIGHT = '\u2591'; // ░
    public static readonly Dictionary<ConsoleColor, string> ANSI_COLOR_MAP = new()
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

    public static readonly string LOGO = @"
__________               __           .__            __    __________             .__          __    __          
\______   \__ __   ____ |  | __  _____|  |__   _____/  |_  \______   \ ____  __ __|  |   _____/  |__/  |_  ____  
 |    |  _/  |  \_/ ___\|  |/ / /  ___/  |  \ /  _ \   __\  |       _//  _ \|  |  \  | _/ __ \   __\   __\/ __ \ 
 |    |   \  |  /\  \___|    <  \___ \|   Y  (  <_> )  |    |    |   (  <_> )  |  /  |_\  ___/|  |  |  | \  ___/ 
 |______  /____/  \___  >__|_ \/____  >___|  /\____/|__|    |____|_  /\____/|____/|____/\___  >__|  |__|  \___  >
        \/            \/     \/     \/     \/                      \/                       \/                \/ 
";

    public void AutoAdjustUISize()
    {
        UI_WIDTH = Console.WindowWidth;
        UI_HEIGHT = Console.WindowHeight; // Leave some space for command input
    }

    public string GetVersionString()
    {
        var versionAttribute = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        string displayVersion = "v" + versionAttribute?.InformationalVersion ?? $"<{_locale.UNKNOWN_VERSION_LABEL}>";
        return displayVersion;
    }

    // Print lines with borders and centered content; Contains auto-adjustment for UI width and height based on console size
    public void PrintBorderedLine(string content)
    {
        int innerWidth = UI_WIDTH - 2 * BORDER_WIDTH;  // Account for borders and shading
        string centeredContent = RenderingTools.CenterString(content, innerWidth);
        Console.WriteLine($"{BORDER_FULL}{BORDER_SHADE}{centeredContent}{BORDER_SHADE}{BORDER_FULL}");
    }

    public void PrintLine(string text)
    {
        Console.WriteLine(RenderingTools.CenterString(text, UI_WIDTH));
    }

    public void PrintBlankLines(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PrintLine("");
        }
    }

    public void PrintBorderedBlankLines(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PrintBorderedLine("");
        }
    }


    public void PrintLogo()
    {
        AutoAdjustUISize();

        string[] lines = LOGO.Split('\n');
        foreach (var line in lines)
        {
            string cleanLine = line;
            Console.WriteLine(new string(' ', (UI_WIDTH - LOGO_WIDTH) / 2) + cleanLine);
        }
    }

    public void PrintTitle(TitleType type)
    {
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor = ConsoleColor.Black;
        
        switch (type)
        {
            case TitleType.Splash:
                Console.WriteLine(_locale.SPLASH_TITLE 
                    + new string(' ', UI_WIDTH - RenderingTools.GetVisibleLength(_locale.SPLASH_TITLE)));
                break;
            case TitleType.Gaming:
                Console.WriteLine(_locale.GAMING_TITLE 
                    + new string(' ', UI_WIDTH - RenderingTools.GetVisibleLength(_locale.GAMING_TITLE)));
                break;
            case TitleType.Configs:
                Console.WriteLine(_locale.CONFIGS_TITLE 
                    + new string(' ', UI_WIDTH - RenderingTools.GetVisibleLength(_locale.CONFIGS_TITLE)));
                break;
            case TitleType.ConfigsModified:
                Console.WriteLine(_locale.CONFIGS_MODIFIED_TITLE 
                    + new string(' ', UI_WIDTH - RenderingTools.GetVisibleLength(_locale.CONFIGS_MODIFIED_TITLE)));
                break;
            default:
                break;
        }

        Console.ResetColor();
    }

    public void PrintFooter(FooterType type)
    {
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor = ConsoleColor.Black;

        switch (type)
        {
            case FooterType.Splash:
                Console.WriteLine(_locale.SPLASH_FOOTER 
                    + new string(' ', UI_WIDTH - RenderingTools.GetVisibleLength(_locale.SPLASH_FOOTER)));
                break;
            case FooterType.Gaming:
                Console.WriteLine(_locale.GAMING_FOOTER 
                    + new string(' ', UI_WIDTH - RenderingTools.GetVisibleLength(_locale.GAMING_FOOTER)));
                break;
            case FooterType.ConfigsReading:
                Console.WriteLine(_locale.CONFIGS_READING_FOOTER 
                    + new string(' ', UI_WIDTH - RenderingTools.GetVisibleLength(_locale.CONFIGS_READING_FOOTER)));
                break;
            case FooterType.ConfigsCommand:
                Console.WriteLine(_locale.CONFIGS_COMMAND_FOOTER 
                    + new string(' ', UI_WIDTH - RenderingTools.GetVisibleLength(_locale.CONFIGS_COMMAND_FOOTER)));
                break;
            default:
                break;
        }
        Console.ResetColor();
    }

    public void PrintInnerBorder()
    {
        PrintLine(BORDER_FULL + new string(BORDER_SHADE, UI_WIDTH - BORDER_WIDTH) + BORDER_FULL);
    }

    public void PrintErrorMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            PrintLine("ERROR: " + message);
            Console.ResetColor();
        }
    }

    public int GetHalvedBlankLineCount(PageType type, int outerSource = 0)
    {
        int nonNullHeight = type switch
        {
            PageType.Splash => SPLASH_NOTNULL_HEIGHT,
            PageType.Gaming => GAMING_NOTNULL_HEIGHT,
            PageType.Configs => outerSource, // For config page, the non-null height is determined by the number of config items
            _ => 0
        };
        return (UI_HEIGHT - nonNullHeight - LOGO_HEIGHT - COMMAND_AREA_RESERVE_HEIGHT) / 2;
    }

    
}
