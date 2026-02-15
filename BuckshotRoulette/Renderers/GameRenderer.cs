using BuckshotRoulette.Simplified.Contexts;
using BuckshotRoulette.Simplified.Utilities;

namespace BuckshotRoulette.Simplified.Renderers;

public static class GameRenderer
{
    public static void Render(GlobalView view)
    {
        RenderingTools.ConsoleCompletelyClear();
        view.Render.AutoAdjustUISize();
        view.Render.PrintLogo();
        view.Render.PrintTitle(TitleType.Gaming);
        view.Render.PrintInnerBorder();

        RenderEntity(EntityType.Dealer, view);

        view.Render.PrintBorderedBlankLines(view.Render.GetHalvedBlankLineCount(PageType.Gaming));
        RenderMagazineInfo(view);
        view.Render.PrintBorderedBlankLines(view.Render.GetHalvedBlankLineCount(PageType.Gaming) - view.Render.GAMING_NOTNULL_HEIGHT % 2);

        RenderEntity(EntityType.Player, view);

        view.Render.PrintInnerBorder();
        view.Render.PrintFooter(FooterType.Gaming);
        view.Render.PrintLine("");

        string message = view.ConsumeErrorMessage();
        if (!string.IsNullOrWhiteSpace(message))
        {
            view.Render.PrintLine(RenderingTools.Colorize(message, ConsoleColor.Red));
        }
    }

    private static void RenderEntity(EntityType type, GlobalView context)
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

    private static void RenderHP(EntityType type, GlobalView context)
    {
        int hp = context.GetEntity(type).Health;
        int maxHp = context.GetEntity(type).MaxHealth;
        string label = context.GetEntity(type).Name;

        ConsoleColor hpColor = (hp * 100 / maxHp <= 25) ? ConsoleColor.Red : Console.ForegroundColor;
        int filledLength = hp * context.Render.HP_BAR_WIDTH / maxHp;
        string hpBar = new string(RenderContext.BORDER_FULL, filledLength) +
                       new string(RenderContext.BORDER_LIGHT, context.Render.HP_BAR_WIDTH - filledLength);

        context.Render.PrintBorderedLine($"{label}   {RenderingTools.Colorize($"{hpBar}  {hp}/{maxHp}", hpColor)}");
    }

    private static void RenderStatus(EntityType type, GlobalView context)
    {
        var statusDict = context.GetEntity(type).GetStatus();

        context.Render.PrintBorderedLine($"{context.Locale.STATUS_LABEL}   {string.Join("  ", statusDict.Select(item => 
            RenderingTools.Colorize(item.Value, item.Key)))}");
    }

    private static void RenderItems(EntityType type, GlobalView context)
    {
        var items = context.GetEntity(type).Items;
        string content = items.Any()
            ? string.Join("  ", items.Select((item, i) => $"[{i}]{item.Name}"))
            : $"{context.Locale.ITEMS_LABEL}";
        context.Render.PrintBorderedLine($"{context.Locale.ITEMS_LABEL}   " + content);
    }

    private static void RenderKnowledges(EntityType type, GlobalView context)
    {
        string knowledgeStr;
        if (context.Game.ActiveEntity == type)
        {
            var list = context.GetEntity(type).Knowledge;
            knowledgeStr = (list != null && list.Any())
                ? $"[ {string.Join("  ", list.Select(k => k.GetChar()))} ]"
                : $"{context.Locale.EMPTY_LABEL}";
        }
        else
        {
            string noise = string.Join("  ", new string(RenderContext.BORDER_LIGHT, context.Game.Magazine.Count * 3 + 2));
            knowledgeStr = $"{noise}";
        }
        context.Render.PrintBorderedLine($"{context.Locale.KNOWLEDGE_LABEL}   " + knowledgeStr);
    }

    private static void RenderMagazineInfo(GlobalView context)
    {
        context.Render.AutoAdjustUISize();
        context.Render.PrintBorderedLine(new string(RenderContext.BORDER_LIGHT, context.Render.UI_WIDTH - 2 * context.Render.BORDER_WIDTH));
        context.Render.PrintBorderedLine("");

        ConsoleColor color = context.Game.IsMultipleDamaged
            ? ConsoleColor.Green : Console.ForegroundColor;
        string dmgInfo = context.Game.IsMultipleDamaged
            ? $"{context.Locale.MAGAZINE_DAMAGE_LABEL} x{context.Game.HandsawMultiplier}"
            : $"{context.Locale.MAGAZINE_DAMAGE_LABEL} x1";
        string ammoInfo = $"{context.Locale.MAGAZINE_LABEL}" +
            $"  [ {context.Locale.MAGAZINE_REAL_LABEL}  {context.Game.GetRealBulletCount()}  |  {context.Game.Magazine.Count}" +
            $"  {context.Locale.MAGAZINE_TOTAL_LABEL} ]" +
            $"  {RenderingTools.Colorize(dmgInfo, color)}";

        context.Render.PrintBorderedLine(ammoInfo);
        context.Render.PrintBorderedLine("");
        context.Render.PrintBorderedLine(new string(RenderContext.BORDER_LIGHT, context.Render.UI_WIDTH - 2 * context.Render.BORDER_WIDTH));
    }
}