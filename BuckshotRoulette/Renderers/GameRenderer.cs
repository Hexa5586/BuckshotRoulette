using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Renderers;

public static class GameRenderer
{
    public static void RenderGaming(GlobalContext context)
    {
        RenderingTools.ConsoleCompletelyClear();

        RenderContext.AutoAdjustUISize();
        RenderContext.PrintLogo();

        // Title bar
        RenderContext.PrintTitle(PageType.Gaming);

        // Top border
        RenderContext.PrintInnerBorder();

        // Dealer Info
        RenderEntity(PlayerType.Dealer, context);

        // Blank
        RenderingTools.PrintBorderedBlankLines(RenderContext.GetHalvedBlankLineCount(PageType.Gaming));

        // Separators and Magazine Info
        RenderMagazineInfo(context);

        // Blank
        RenderingTools.PrintBorderedBlankLines(RenderContext.GetHalvedBlankLineCount(PageType.Gaming));

        // Player Info
        RenderEntity(PlayerType.Player, context);

        // Bottom border
        RenderContext.PrintInnerBorder();

        // Footer
        RenderContext.PrintFooter();

        // Bottom padding for commanding area
        RenderingTools.PrintLine("");

        // Consume and output error message if exists
        RenderContext.PrintErrorMessage(context.ConsumeErrorMessage());
        
    }

    private static void RenderEntity(PlayerType type, GlobalContext context)
    {
        RenderContext.AutoAdjustUISize();

        RenderingTools.PrintBorderedLine(""); // Blank line for vertical alignment

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

        int hpBarFilledLength = hp * RenderContext.HP_BAR_WIDTH / maxHp;
        string hpBar = new string(RenderContext.BORDER_FULL, hpBarFilledLength) + new string(RenderContext.BORDER_LIGHT, RenderContext.HP_BAR_WIDTH - hpBarFilledLength);
        string label = context.GetName(type);
        
        RenderingTools.PrintBorderedLine($"{label}   {RenderingTools.Colorize($"{hpBar}  {hp}/{maxHp}", hpColor)}");

        RenderingTools.PrintBorderedLine(""); // Blank line for spacing

        // Status effects
        string cuffedLabel = (context.ActivePlayer != type && context.IsPassiveCuffed)
            ? RenderingTools.Colorize("< CUFFED >", ConsoleColor.Red) : "";
        string cuffCdLabel = (context.GetCuffCdLeft(type) > 0)
            ? RenderingTools.Colorize($"< HANDCUFFS COOLING ({context.GetCuffCdLeft(type)}) >", ConsoleColor.Yellow) : "";
        string noStatusLabel = (string.IsNullOrEmpty(cuffedLabel) && string.IsNullOrEmpty(cuffCdLabel)) ? "<EMPTY>" : "";
        RenderingTools.PrintBorderedLine($"STATUS   {noStatusLabel}{cuffedLabel}  {cuffCdLabel}");

        RenderingTools.PrintBorderedLine(""); // Blank line for spacing

        // Items
        using var sw = new StringWriter();
        OutputTools.OutputListThroughMapping(items, i => i.Name, "  ", "", 
            containIndex: true, containBrackets: false, writer: sw);
        RenderingTools.PrintBorderedLine("ITEMS " + (items.Any() ? sw.ToString() : "<EMPTY>"));

        RenderingTools.PrintBorderedLine(""); // Blank line for spacing

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
        RenderingTools.PrintBorderedLine(knowledgeContent);

        RenderingTools.PrintBorderedLine(""); // Blank line for vertical alignment
    }

    private static void RenderMagazineInfo(GlobalContext context)
    {
        RenderContext.AutoAdjustUISize();

        RenderingTools.PrintBorderedLine(new string(RenderContext.BORDER_LIGHT, RenderContext.UI_WIDTH - 2 * RenderContext.BORDER_WIDTH));
        RenderingTools.PrintBorderedLine("");

        ConsoleColor multiplierColor = Console.ForegroundColor;
        if (context.IsMultipleDamaged)
        {
            multiplierColor = ConsoleColor.Green;
        }

        string multipliedInfo = (context.IsMultipleDamaged ? $"DAMAGE x{context.HandsawMultiplier}" : "DAMAGE x1");
        string ammoInfo = $"MAGAZINE  [ REAL  {context.GetRemainingRealCount()}  |  {context.GetMagazine().Count}  TOTAL ]" +
            $"   {RenderingTools.Colorize(multipliedInfo, multiplierColor)}";
        RenderingTools.PrintBorderedLine(ammoInfo);

        RenderingTools.PrintBorderedLine("");
        RenderingTools.PrintBorderedLine(new string(RenderContext.BORDER_LIGHT, RenderContext.UI_WIDTH - 2 * RenderContext.BORDER_WIDTH));
    }

}