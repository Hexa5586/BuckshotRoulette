using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Renderers;

public static class SplashRenderer
{
    public static void RenderSplashScreen(GlobalContext context)
    {
        RenderingTools.ConsoleCompletelyClear();

        RenderContext.AutoAdjustUISize();

        RenderContext.PrintLogo();

        // Title bar
        RenderContext.PrintTitle(PageType.Splash);

        // Top border
        RenderContext.PrintInnerBorder();

        // Blank lines
        RenderingTools.PrintBorderedBlankLines(RenderContext.GetHalvedBlankLineCount(PageType.Splash));
        
        RenderingTools.PrintBorderedLine(""); // Blank line for spacing
        RenderingTools.PrintBorderedLine("Welcome to Buckshot Roulette! For better display, please ensure the window is MAXIMIZED.");
        RenderingTools.PrintBorderedLine(""); // Blank line for spacing
        RenderingTools.PrintBorderedLine("You can always directly input an ENTER to refresh the UI.");
        RenderingTools.PrintBorderedLine(""); // Blank line for spacing
        RenderingTools.PrintBorderedLine("[   START   ]   Start the game;     ");
        RenderingTools.PrintBorderedLine("[  CONFIGS  ]   Edit Configurations;");
        RenderingTools.PrintBorderedLine("[   CLOSE   ]   Exit the game.      ");
        RenderingTools.PrintBorderedLine(""); // Blank line for spacing

        // Blank lines
        RenderingTools.PrintBorderedBlankLines(RenderContext.GetHalvedBlankLineCount(PageType.Splash));

        // Bottom border
        RenderContext.PrintInnerBorder();

        // Footer
        RenderContext.PrintFooter();

        RenderingTools.PrintLine("");

        // Consume and print error message if exists
        RenderContext.PrintErrorMessage(context.ConsumeErrorMessage());
    }
}
