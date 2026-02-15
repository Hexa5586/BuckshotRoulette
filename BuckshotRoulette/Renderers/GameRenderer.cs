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
        context.Render.PrintTitle(TitleType.Gaming);
        context.Render.PrintInnerBorder();

        RenderEntity(PlayerType.Dealer, context);

        context.Render.PrintBorderedBlankLines(context.Render.GetHalvedBlankLineCount(PageType.Gaming));
        RenderMagazineInfo(context);
        context.Render.PrintBorderedBlankLines(context.Render.GetHalvedBlankLineCount(PageType.Gaming) - context.Render.GAMING_NOTNULL_HEIGHT % 2);

        RenderEntity(PlayerType.Player, context);

        context.Render.PrintInnerBorder();
        context.Render.PrintFooter(FooterType.Gaming);
        context.Render.PrintLine("");

        string message = context.ConsumeErrorMessage();
        if (!string.IsNullOrWhiteSpace(message))
        {
            context.Render.PrintLine(RenderingTools.Colorize(message, ConsoleColor.Red));
        }
    }

    private static void RenderEntity(PlayerType type, GlobalContext context)
    {
        context.Render.AutoAdjustUISize();
        context.Render.PrintBorderedLine("");

        RenderHP(type, context);
        context.Render.PrintBorderedLine("");

        RenderStatus(type, context);
        context.Render.PrintBorderedLine("");

        RenderItems(type, context);
        context.Render.PrintBorderedLine("");

        RenderKnowledges(type, context);
        context.Render.PrintBorderedLine("");
    }

    private static void RenderHP(PlayerType type, GlobalContext context)
    {
        int hp = context.GetHealth(type);
        int maxHp = context.GetMaxHealth(type);
        string label = context.GetName(type);

        ConsoleColor hpColor = (hp * 100 / maxHp <= 25) ? ConsoleColor.Red : Console.ForegroundColor;
        int filledLength = hp * context.Render.HP_BAR_WIDTH / maxHp;
        string hpBar = new string(RenderContext.BORDER_FULL, filledLength) +
                       new string(RenderContext.BORDER_LIGHT, context.Render.HP_BAR_WIDTH - filledLength);

        context.Render.PrintBorderedLine($"{label}   {RenderingTools.Colorize($"{hpBar}  {hp}/{maxHp}", hpColor)}");
    }

    private static void RenderStatus(PlayerType type, GlobalContext context)
    {
        string cuffed = (context.ActivePlayer != type && context.IsPassiveCuffed)
            ? RenderingTools.Colorize("< CUFFED >", ConsoleColor.Red) : "";
        string cooldown = (context.GetCuffCdLeft(type) > 0)
            ? RenderingTools.Colorize($"< HANDCUFFS COOLING ({context.GetCuffCdLeft(type)}) >", ConsoleColor.Yellow) : "";
        string empty = (string.IsNullOrEmpty(cuffed) && string.IsNullOrEmpty(cooldown)) ? "<EMPTY>" : "";

        context.Render.PrintBorderedLine($"STATUS   {empty}{cuffed}  {cooldown}");
    }

    private static void RenderItems(PlayerType type, GlobalContext context)
    {
        var items = context.GetItems(type);
        string content = items.Any()
            ? string.Join("  ", items.Select((item, i) => $"[{i + 1}]{item.Name}"))
            : "<EMPTY>";
        context.Render.PrintBorderedLine("ITEMS   " + content);
    }

    private static void RenderKnowledges(PlayerType type, GlobalContext context)
    {
        string knowledgeStr;
        if (context.ActivePlayer == type)
        {
            var list = context.GetKnowledge(type);
            knowledgeStr = (list != null && list.Any())
                ? $"[ {string.Join("  ", list.Select(k => k.GetAscii()))} ]"
                : "NONE";
        }
        else
        {
            string noise = string.Join("  ", Enumerable.Repeat("?", context.GetMagazine().Count));
            knowledgeStr = $"[ {noise} ]";
        }
        context.Render.PrintBorderedLine("KNOWLEDGES   " + knowledgeStr);
    }

    private static void RenderMagazineInfo(GlobalContext context)
    {
        context.Render.AutoAdjustUISize();
        context.Render.PrintBorderedLine(new string(RenderContext.BORDER_LIGHT, context.Render.UI_WIDTH - 2 * context.Render.BORDER_WIDTH));
        context.Render.PrintBorderedLine("");

        ConsoleColor color = context.IsMultipleDamaged ? ConsoleColor.Green : Console.ForegroundColor;
        string dmgInfo = context.IsMultipleDamaged ? $"DAMAGE x{context.HandsawMultiplier}" : "DAMAGE x1";
        string ammoInfo = $"MAGAZINE  [ REAL  {context.GetRemainingRealCount()}  |  {context.GetMagazine().Count}  TOTAL ]   {RenderingTools.Colorize(dmgInfo, color)}";

        context.Render.PrintBorderedLine(ammoInfo);
        context.Render.PrintBorderedLine("");
        context.Render.PrintBorderedLine(new string(RenderContext.BORDER_LIGHT, context.Render.UI_WIDTH - 2 * context.Render.BORDER_WIDTH));
    }
}