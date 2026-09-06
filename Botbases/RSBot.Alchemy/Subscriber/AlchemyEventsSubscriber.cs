using System;
using System.Linq;
using RSBot.Core;
using RSBot.Core.Client.ReferenceObjects;
using RSBot.Core.Components;
using RSBot.Core.Event;
using RSBot.Core.Objects;

namespace RSBot.Alchemy.Subscriber;

internal class AlchemyEventsSubscriber
{
    public static void Subscribe()
    {
        EventManager.SubscribeEvent("OnAlchemyError", new Action<ushort, AlchemyType>(OnAlchemyError));
        EventManager.SubscribeEvent("OnAlchemyDestroyed", new Action<InventoryItem, AlchemyType>(OnAlchemyDestroyed));
        EventManager.SubscribeEvent("OnFuseRequest", new Action<AlchemyAction, AlchemyType>(OnFuseRequest));
    }

    private static void OnAlchemyDestroyed(InventoryItem oldItem, AlchemyType type)
    {
        if (!Bootstrap.IsActive)
            return;

        Globals.Botbase.EnhanceBundleConfig = null;
        Globals.Botbase.MagicBundleConfig = null;

        Globals.View.SelectedItem = null;
        Globals.View.AddLog(
            oldItem.Record.GetRealName(),
            Game.ReferenceManager.GetTranslation("UIIT_MSG_REINFORCERR_BREAKDOWN")
        );
        Bootstrap.StopWithReason("[Alchemy] The item has been destroyed, stopping now...");
    }

    private static void OnAlchemyError(ushort errorCode, AlchemyType type)
    {
        if (!Bootstrap.IsActive)
            return;

        // Non-fatal server responses — bot will retry on the next tick
        if (errorCode is 0x5423)
            return;

        // Astral already at max (equals immortal level). Disable and continue.
        if (errorCode is 0x5424)
        {
            Log.Warn("[Alchemy] Astral stone max reached, disabling astral for this session.");
            if (Globals.Botbase?.EnhanceBundleConfig != null)
                Globals.Botbase.EnhanceBundleConfig.UseAstralStones = false;
            return;
        }

        // Code 3: transient server rejection (timing / cooldown). Item is intact,
        // ingredients were not consumed. The Run() tick will re-check and retry.
        if (errorCode is 0x3)
        {
            Log.Warn($"[Alchemy] Fusion rejected by server (code: {errorCode:X}), will retry...");
            return;
        }

        Bootstrap.StopWithReason($"[Alchemy] Alchemy fusion error: {errorCode:X}");
    }

    /// <summary>
    ///     Will be triggered if any fuse request (either elixir or magic stone..) was sent to the server. Adds a log message.
    /// </summary>
    /// <param name="action">The alchemy action</param>
    /// <param name="type">The type of alchemy</param>
    private static void OnFuseRequest(AlchemyAction action, AlchemyType type)
    {
        if (AlchemyManager.ActiveAlchemyItems == null)
            return;

        var ingredient = AlchemyManager.ActiveAlchemyItems.ElementAtOrDefault(1);
        var item = AlchemyManager.ActiveAlchemyItems.ElementAtOrDefault(0);

        switch (type)
        {
            case AlchemyType.Elixir:
                Globals.View.AddLog(item?.Record.GetRealName(), $"Fusing elixir [{ingredient.Record.GetRealName()}]");
                break;

            case AlchemyType.MagicStone:
                Globals.View.AddLog(
                    item?.Record.GetRealName(),
                    $"Fusing magic stone [{ingredient.Record.GetRealName()}]"
                );
                break;

            case AlchemyType.AttributeStone:
                Globals.View.AddLog(
                    item?.Record.GetRealName(),
                    $"Fusing attribute stone [{ingredient.Record.GetRealName()}]"
                );
                break;

            default:
                Globals.View.AddLog(item?.Record.GetRealName(), $"Fusing [{ingredient.Record.GetRealName()}]");
                break;
        }
    }
}
