using BuckshotRoulette.Simplified.Contexts;

namespace BuckshotRoulette.Simplified.Utilities;

public static class RenderingTools
{
    public static void ConsoleCompletelyClear()
    {
        Console.Write("\x1b[2J\x1b[3J\x1b[H");  // Clear console and move cursor to top-left
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

    public static string Colorize(string? text, ConsoleColor color)
    {
        if (text == null) return string.Empty;
        if (RenderContext.ANSI_COLOR_MAP.TryGetValue(color, out string? ansi))
        {
            return $"{ansi}{text}{RenderContext.ANSI_COLOR_RESET}";
        }
        return text;
    }
}
