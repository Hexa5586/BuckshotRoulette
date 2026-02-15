using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Renderers;

public static class SplashRenderer
{
    public static void Render(GlobalView context)
    {
        RenderingTools.ConsoleCompletelyClear();

        context.Render.AutoAdjustUISize();

        context.Render.PrintLogo();

        // Title bar
        context.Render.PrintTitle(TitleType.Splash);

        // Top border
        context.Render.PrintInnerBorder();

        // Blank lines
        context.Render.PrintBorderedBlankLines(context.Render.GetHalvedBlankLineCount(PageType.Splash));
        
        context.Render.PrintBorderedLine(""); // Blank line for spacing
        context.Render.PrintBorderedLine(context.Locale.SPLASH_LINE_CUE_1);
        context.Render.PrintBorderedLine(""); // Blank line for spacing
        context.Render.PrintBorderedLine(context.Locale.SPLASH_LINE_CUE_2);
        context.Render.PrintBorderedLine(""); // Blank line for spacing
        context.Render.PrintBorderedLine(context.Locale.SPLASH_OPTION_1);
        context.Render.PrintBorderedLine(context.Locale.SPLASH_OPTION_2);
        context.Render.PrintBorderedLine(context.Locale.SPLASH_OPTION_3);
        context.Render.PrintBorderedLine(""); // Blank line for spacing
        context.Render.PrintBorderedLine(context.Render.GetVersionString());
        context.Render.PrintBorderedLine(""); // Blank line for spacing

        // Blank lines
        context.Render.PrintBorderedBlankLines(context.Render.GetHalvedBlankLineCount(PageType.Splash) - context.Render.SPLASH_NOTNULL_HEIGHT % 2);

        // Bottom border
        context.Render.PrintInnerBorder();

        // Footer
        context.Render.PrintFooter(FooterType.Splash);

        context.Render.PrintLine("");

        // Consume and print error messages
        string message = context.ConsumeErrorMessage();
        if (!string.IsNullOrWhiteSpace(message))
        {
            context.Render.PrintLine(RenderingTools.Colorize(message, ConsoleColor.Red));
        }
    }
}
