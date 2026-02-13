using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Renderers;

public static class SplashRenderer
{
    public static void RenderSplashScreen(GlobalContext context)
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
        context.Render.PrintBorderedLine("Welcome to Buckshot Roulette! For better display, please ensure the window is MAXIMIZED.");
        context.Render.PrintBorderedLine(""); // Blank line for spacing
        context.Render.PrintBorderedLine("You can always directly input an ENTER to refresh the UI.");
        context.Render.PrintBorderedLine(""); // Blank line for spacing
        context.Render.PrintBorderedLine("[   START   ]   Start the game;     ");
        context.Render.PrintBorderedLine("[  CONFIGS  ]   Edit Configurations;");
        context.Render.PrintBorderedLine("[   CLOSE   ]   Exit the game.      ");
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
