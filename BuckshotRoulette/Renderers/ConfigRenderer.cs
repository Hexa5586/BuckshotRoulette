using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Renderers;

public static class ConfigRenderer
{
    public static void RenderConfig(GlobalContext context)
    {
        RenderingTools.ConsoleCompletelyClear();
    
        RenderContext.AutoAdjustUISize();
        RenderContext.PrintLogo();
    
        // Title bar
        RenderContext.PrintTitle(PageType.Config);
    
        // Top border
        RenderContext.PrintInnerBorder();
    
        // Blank lines
        RenderingTools.PrintBorderedBlankLines(RenderContext.GetHalvedBlankLineCount(PageType.Config));
    
        RenderingTools.PrintBorderedLine(""); // Blank line for spacing
        RenderingTools.PrintBorderedLine("Configuration page is under construction. Please check back later!");
        RenderingTools.PrintBorderedLine(""); // Blank line for spacing
    
        // Blank lines
        RenderingTools.PrintBorderedBlankLines(RenderContext.GetHalvedBlankLineCount(PageType.Config));
    
        // Bottom border
        RenderContext.PrintInnerBorder();
    
        // Footer
        RenderContext.PrintFooter();
    }
}
