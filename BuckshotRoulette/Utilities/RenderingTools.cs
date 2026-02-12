using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.Utilities;

public static class RenderingTools
{
    public static void ConsoleCompletelyClear()
    {
        Console.Write("\x1b[2J\x1b[3J\x1b[H");  // Clear console and move cursor to top-left
    }

    // Print lines with borders and centered content; Contains auto-adjustment for UI width and height based on console size
    public static void PrintBorderedLine(string content)
    {
        int innerWidth = RenderContext.UI_WIDTH - 2 * RenderContext.BORDER_WIDTH;  // Account for borders and shading
        string centeredContent = CenterString(content, innerWidth);
        Console.WriteLine($"{RenderContext.BORDER_FULL}{RenderContext.BORDER_SHADE}{centeredContent}{RenderContext.BORDER_SHADE}{RenderContext.BORDER_FULL}");
    }

    public static void PrintLine(string text)
    {
        Console.WriteLine(CenterString(text, RenderContext.UI_WIDTH));
    }

    public static void PrintBlankLines(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PrintLine("");
        }
    }

    public static void PrintBorderedBlankLines(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PrintBorderedLine("");
        }
    }

    public static string CenterString(string s, int width)
    {
        // Get the visible length
        int visibleLength = GetVisibleLength(s);

        if (visibleLength >= width)
        {
            return s;
        }

        int left = (width - visibleLength) / 2;
        int right = width - visibleLength - left;

        return new string(' ', left) + s + new string(' ', right);
    }

    /// <summary>
    /// Gets the visible length of a string by removing ANSI escape codes, which are used for coloring and do not take up space
    /// in the console. This is important for proper alignment when centering strings that contain color codes.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int GetVisibleLength(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0;
        return System.Text.RegularExpressions.Regex.Replace(s, @"\u001b\[[0-9;]*m", "").Length;
    }

    public static string Colorize(string text, ConsoleColor color)
    {
        if (RenderContext.ANSI_COLOR_MAP.TryGetValue(color, out string? ansi))
        {
            return $"{ansi}{text}{RenderContext.ANSI_COLOR_RESET}";
        }
        return text;
    }
}
