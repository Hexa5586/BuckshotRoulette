using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.States.ConfigStates;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Renderers;

public static class ConfigsRenderer
{
    public static void RenderConfig(GlobalContext context, ConfigModeType type, bool modified)
    {
        int config_notnull_count = 10 + context.Configs.Groups[context.Configs.CurrentPage].ConfigNames.Count;
        int blanklines_count = context.Render.GetHalvedBlankLineCount(PageType.Configs, config_notnull_count);

        RenderingTools.ConsoleCompletelyClear();
    
        context.Render.AutoAdjustUISize();
        context.Render.PrintLogo();
    
        // Title bar
        if (modified)
        {
            context.Render.PrintTitle(TitleType.ConfigsModified);
        }
        else
        {
            context.Render.PrintTitle(TitleType.Configs);
        }

        // Top border
        context.Render.PrintInnerBorder();

        context.Render.PrintBorderedLine("");   // Empty line for spacing
        context.Render.PrintBorderedLine(
            RenderingTools.Colorize(context.Configs.Groups[context.Configs.CurrentPage].Title, ConsoleColor.Green));

        // Blank lines
        context.Render.PrintBorderedBlankLines(blanklines_count);

        context.Render.PrintBorderedBlankLines(2);  // Empty lines for spacing

        foreach (var configName in context.Configs.Groups[context.Configs.CurrentPage].ConfigNames)
        {
            string namePart = $"[{configName}]";
            string valuePart = context.Configs.GetConfigValue(configName).ToString() ?? "";
            string filling = new string(' ', Math.Max(1, context.Render.CONFIG_ITEM_WIDTH - namePart.Length - valuePart.Length));
            context.Render.PrintBorderedLine(namePart + filling + valuePart);
        }

        context.Render.PrintBorderedBlankLines(2);  // Empty lines for spacing

        // Blank lines
        context.Render.PrintBorderedBlankLines(blanklines_count - config_notnull_count % 2);    // Adjust blank lines if config count is odd

        context.Render.PrintBorderedLine(
            RenderingTools.Colorize($"Page {context.Configs.CurrentPage + 1} / {context.Configs.Groups.Count}", ConsoleColor.DarkGray));

        context.Render.PrintBorderedLine("");   // Empty line for spacing

        // Bottom border
        context.Render.PrintInnerBorder();

        // Footer
        if (type == ConfigModeType.Reading)
        {
            context.Render.PrintFooter(FooterType.ConfigsReading);
        }
        else if (type == ConfigModeType.Command)
        {
            context.Render.PrintFooter(FooterType.ConfigsCommand);
        }

        context.Render.PrintLine("");   // Empty line for spacing

        // Consume and print error messages
        string message = context.ConsumeErrorMessage();
        if (!string.IsNullOrWhiteSpace(message))
        {
            context.Render.PrintLine(RenderingTools.Colorize(message, ConsoleColor.Red));
        }
        
    }
}
