using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Renderers;

public static class GameRenderer
{
    public static void RenderGaming(GlobalContext context)
    {
        RenderingTools.ConsoleCompletelyClear();

        context.Render.AutoAdjustUISize();
        context.Render.PrintLogo();

        // Title bar
        context.Render.PrintTitle(TitleType.Gaming);

        // Top border
        context.Render.PrintInnerBorder();

        // Dealer Info
        RenderEntity(PlayerType.Dealer, context);

        // Blank
        context.Render.PrintBorderedBlankLines(context.Render.GetHalvedBlankLineCount(PageType.Gaming));

        // Separators and Magazine Info
        RenderMagazineInfo(context);

        // Blank
        context.Render.PrintBorderedBlankLines(context.Render.GetHalvedBlankLineCount(PageType.Gaming) - context.Render.GAMING_NOTNULL_HEIGHT % 2);

        // Player Info
        RenderEntity(PlayerType.Player, context);

        // Bottom border
        context.Render.PrintInnerBorder();

        // Footer
        context.Render.PrintFooter(FooterType.Gaming);

        // Bottom padding for commanding area
        context.Render.PrintLine("");

        // Consume and print error messages
        string message = context.ConsumeErrorMessage();
        if (!string.IsNullOrWhiteSpace(message))
        {
            context.Render.PrintLine(RenderingTools.Colorize(message, ConsoleColor.Red));
        }

    }

    private static void RenderEntity(PlayerType type, GlobalContext context)
    {
        context.Render.AutoAdjustUISize();

        context.Render.PrintBorderedLine(""); // Blank line for vertical alignment

        // Informations from the context
        int hp = context.GetHealth(type);
        int maxHp = context.GetMaxHealth(type);
        var items = context.GetItems(type);

        // Center align name and HP bar
        ConsoleColor hpColor = Console.ForegroundColor;
        if (hp * 100 / maxHp <= 25)
        {
            hpColor = ConsoleColor.Red;
        }

        int hpBarFilledLength = hp * context.Render.HP_BAR_WIDTH / maxHp;
        string hpBar = new string(RenderContext.BORDER_FULL, hpBarFilledLength) + new string(RenderContext.BORDER_LIGHT, context.Render.HP_BAR_WIDTH - hpBarFilledLength);
        string label = context.GetName(type);
        
        context.Render.PrintBorderedLine($"{label}   {RenderingTools.Colorize($"{hpBar}  {hp}/{maxHp}", hpColor)}");

        context.Render.PrintBorderedLine(""); // Blank line for spacing

        // Status effects
        string cuffedLabel = (context.ActivePlayer != type && context.IsPassiveCuffed)
            ? RenderingTools.Colorize("< CUFFED >", ConsoleColor.Red) : "";
        string cuffCdLabel = (context.GetCuffCdLeft(type) > 0)
            ? RenderingTools.Colorize($"< HANDCUFFS COOLING ({context.GetCuffCdLeft(type)}) >", ConsoleColor.Yellow) : "";
        string noStatusLabel = (string.IsNullOrEmpty(cuffedLabel) && string.IsNullOrEmpty(cuffCdLabel)) ? "<EMPTY>" : "";
        context.Render.PrintBorderedLine($"STATUS   {noStatusLabel}{cuffedLabel}  {cuffCdLabel}");

        context.Render.PrintBorderedLine(""); // Blank line for spacing

        // Items
        using var sw = new StringWriter();
        OutputTools.OutputListThroughMapping(items, i => i.Name, "  ", "", 
            containIndex: true, containBrackets: false, writer: sw);
        context.Render.PrintBorderedLine("ITEMS " + (items.Any() ? sw.ToString() : "<EMPTY>"));

        context.Render.PrintBorderedLine(""); // Blank line for spacing

        // Bullet knowledges
        string knowledgeContent;
        if (context.ActivePlayer == type)
        {
            using var knowledgeSw = new StringWriter();
            var knowledgeList = context.GetKnowledge(type);
            OutputTools.OutputListThroughMapping(knowledgeList, k => k.GetAscii(), " ", "", 
                containIndex: false, containBrackets: true, writer: knowledgeSw);

            string result = knowledgeSw.ToString();
            knowledgeContent = "KNOWLEDGE   " + (string.IsNullOrWhiteSpace(result) ? "NONE" : result);
        }
        else
        {
            string noise = new string(new string(RenderContext.BORDER_LIGHT, 2 * context.GetMagazine().Count + 3));
            knowledgeContent = $"KNOWLEDGE   {noise}";
        }
        context.Render.PrintBorderedLine(knowledgeContent);

        context.Render.PrintBorderedLine(""); // Blank line for vertical alignment
    }

    private static void RenderMagazineInfo(GlobalContext context)
    {
        context.Render.AutoAdjustUISize();

        context.Render.PrintBorderedLine(new string(RenderContext.BORDER_LIGHT, context.Render.UI_WIDTH - 2 * context.Render.BORDER_WIDTH));
        context.Render.PrintBorderedLine("");

        ConsoleColor multiplierColor = Console.ForegroundColor;
        if (context.IsMultipleDamaged)
        {
            multiplierColor = ConsoleColor.Green;
        }

        string multipliedInfo = (context.IsMultipleDamaged ? $"DAMAGE x{context.HandsawMultiplier}" : "DAMAGE x1");
        string ammoInfo = $"MAGAZINE  [ REAL  {context.GetRemainingRealCount()}  |  {context.GetMagazine().Count}  TOTAL ]" +
            $"   {RenderingTools.Colorize(multipliedInfo, multiplierColor)}";
        context.Render.PrintBorderedLine(ammoInfo);

        context.Render.PrintBorderedLine("");
        context.Render.PrintBorderedLine(new string(RenderContext.BORDER_LIGHT, context.Render.UI_WIDTH - 2 * context.Render.BORDER_WIDTH));
    }

}