using System;
using System.Collections.Generic;
using System.Linq;
using RSBot.Alchemy.Bot;
using RSBot.Alchemy.Extension;
using RSBot.Alchemy.Helper;
using RSBot.Core;
using RSBot.Core.Client.ReferenceObjects;
using RSBot.Core.Components;
using RSBot.Core.Event;
using RSBot.Core.Objects;

namespace RSBot.Alchemy.Bundle.Enhance;

internal class EnhanceBundle : IAlchemyBundle
{
    private EnhanceBundleConfig _config;

    private bool _isStoneFusing;

    private AlchemyRule _activeRule;

    #region Constructor

    /// <summary>
    ///     Subscribes events
    /// </summary>
    public EnhanceBundle()
    {
        SubscribeEvents();

        _shouldRun = true;
    }

    #endregion Constructor

    #region Members

    private bool _shouldRun;

    private IEnumerable<InventoryItem> _luckyPowders;

    private DateTime _nextRunAfter = DateTime.MinValue;

    #endregion Members

    #region Methods

    public void Stop()
    {
        _shouldRun = false;
        _config = null;
        _nextRunAfter = DateTime.MinValue;

        AlchemyManager.CancelPending();
    }

    /// <summary>
    ///     Starts this manager
    /// </summary>
    public void Start()
    {
        _shouldRun = true;
    }

    /// <summary>
    ///     Subscribes all required events
    /// </summary>
    private void SubscribeEvents()
    {
        EventManager.SubscribeEvent(
            "OnAlchemyDestroyed",
            new Action<InventoryItem, AlchemyType>(OnElixirAlchemyDestroyed)
        );
        EventManager.SubscribeEvent(
            "OnAlchemySuccess",
            new Action<InventoryItem, InventoryItem, AlchemyType>(OnElixirAlchemySuccess)
        );
        EventManager.SubscribeEvent("OnAlchemy", OnElixirAlchemy);
        EventManager.SubscribeEvent(
            "OnAlchemyFailed",
            new Action<InventoryItem, InventoryItem, AlchemyType>(OnElixirAlchemyFailed)
        );
        EventManager.SubscribeEvent("OnFuseRequest", new Action<AlchemyAction, AlchemyType>(OnFuseRequest));
        EventManager.SubscribeEvent(
            "OnAlchemyError",
            new Action<ushort, AlchemyType>(OnElixirAlchemyError)
        );
    }

    /// <summary>
    ///     Runs a new tick of this manager
    /// </summary>
    /// <param name="engineConfig"></param>
    public void Run<T>(T engineConfig)
    {
        if (engineConfig is not EnhanceBundleConfig config)
            return;

        if (config.Item == null)
        {
            Bootstrap.StopWithReason("[Alchemy] No item configured");
            return;
        }

        //Item still there and available?
        var item = Game.Player.Inventory.GetItemAt(config.Item.Slot);
        if (item == null || item.Amount == 0)
        {
            Bootstrap.StopWithReason("[Alchemy] Item to enhance is unavailable");
            return;
        }

        //Config incomplete?
        if (!_shouldRun || Globals.Botbase.AlchemyEngine != AlchemyEngine.Enhance)
            return;

        // 500ms buffer between consecutive sends
        if (DateTime.UtcNow < _nextRunAfter)
            return;

        _config = config;
        _luckyPowders = AlchemyItemHelper.GetLuckyPowders(config.Item);

        // Resolve active rule for the next opt level
        var targetLevel = (byte)(config.Item.OptLevel + 1);
        _activeRule = config.Rules?.Count > 0
            ? config.Rules.FirstOrDefault(r => r.OptLevel == targetLevel)
            : null;

        // Use rule's elixirs if available, otherwise fall back to default config
        var effectiveElixirs = _activeRule?.Elixirs ?? config.Elixirs;

        if (effectiveElixirs == null || !effectiveElixirs.Any() || effectiveElixirs.Sum(i => i.Amount) == 0)
        {
            Bootstrap.StopWithReason("[Alchemy] No enhancement elixir selected");
            return;
        }

        //Stop if lucky powder is empty (always enforced)
        {
            // Check only the configured powder if one is selected; otherwise check any matching powder
            var effectivePowdersForCheck = _activeRule?.LuckyPowders ?? config.LuckyPowders;
            bool powderAvailable;
            if (effectivePowdersForCheck != null && effectivePowdersForCheck.Any())
            {
                var p = Game.Player.Inventory.GetItem(effectivePowdersForCheck.First().ItemId);
                powderAvailable = p != null && p.Amount > 0;
            }
            else
            {
                powderAvailable = _luckyPowders.Any();
            }

            if (!powderAvailable)
            {
                Bootstrap.StopWithReason("[Alchemy] No lucky powder left, stopping alchemy now!");
                return;
            }
        }

        //Max opt level reached?
        if (config.Item.OptLevel >= config.MaxOptLevel)
        {
            Globals.View.AddLog(
                config.Item.Record.GetRealName(),
                $"The item's option level is {config.Item.OptLevel}/{config.MaxOptLevel}"
            );
            Bootstrap.StopWithSuccess($"[Alchemy] Item reached the target +{config.MaxOptLevel}");
            return;
        }

        //Use steady stone?
        if (_config.UseSteadyStones && _config.Item.OptLevel >= 5)
        {
            if (!AlchemyItemHelper.HasMagicOption(config.Item, RefMagicOpt.MaterialSteady))
            {
                var steadyStone = AlchemyItemHelper.GetSteadyStone(config.Item);
                if (steadyStone == null || steadyStone.Amount == 0)
                {
                    Bootstrap.StopWithReason("[Alchemy] Steady stone required but none available, stopping!");
                    return;
                }

                if (!AlchemyManager.TryFuseMagicStone(_config.Item, steadyStone))
                    return;

                _shouldRun = false;
                _isStoneFusing = true;
                return;
            }
        }

        //Use lucky stone?
        var hasLuckyRule = _activeRule?.MaxLuckyTimes.HasValue == true;
        if ((_config.UseLuckyStones && _config.Item.OptLevel >= _config.LuckyStoneFromLevel)
            || (hasLuckyRule && _config.Item.OptLevel >= 5))
        {
            // How many Lucky times to reach before enhancement:
            // - Rule with MaxLuckyTimes set: keep applying until currentLucky reaches the target
            // - Default (no rule): apply once (target = 1), same as the old !HasMagicOption behaviour
            var luckyTarget = hasLuckyRule ? (uint)_activeRule.MaxLuckyTimes.Value : 1u;
            var currentLucky = AlchemyItemHelper.GetMagicOptionValue(config.Item, RefMagicOpt.MaterialLuck);

            if (currentLucky < luckyTarget)
            {
                var luckyStone = AlchemyItemHelper.GetLuckyStone(config.Item);
                if (luckyStone == null || luckyStone.Amount == 0)
                {
                    Bootstrap.StopWithReason(
                        $"[Alchemy] Lucky stone required at +{targetLevel} (Lucky {currentLucky}/{luckyTarget}) but none available, stopping!"
                    );
                    return;
                }

                if (!AlchemyManager.TryFuseMagicStone(_config.Item, luckyStone))
                    return;

                _shouldRun = false;
                _isStoneFusing = true;
                return;
            }
        }

        //Use immortal stone?
        if (_config.UseImmortalStones && _config.Item.OptLevel >= 5)
        {
            if (!AlchemyItemHelper.HasMagicOption(config.Item, RefMagicOpt.MaterialImmortal))
            {
                var immortalStone = AlchemyItemHelper.GetImmortalStone(config.Item);
                if (immortalStone == null || immortalStone.Amount == 0)
                {
                    Bootstrap.StopWithReason("[Alchemy] Immortal stone required but none available, stopping!");
                    return;
                }

                if (!AlchemyManager.TryFuseMagicStone(_config.Item, immortalStone))
                    return;

                _shouldRun = false;
                _isStoneFusing = true;
                return;
            }
        }

        //Use astral stone?
        if (_config.UseAstralStones && _config.Item.OptLevel >= 5)
        {
            if (!AlchemyItemHelper.HasMagicOption(config.Item, RefMagicOpt.MaterialAstral))
            {
                var astralStone = AlchemyItemHelper.GetAstralStone(config.Item);
                if (astralStone == null || astralStone.Amount == 0)
                {
                    Bootstrap.StopWithReason("[Alchemy] Astral stone required but none available, stopping!");
                    return;
                }

                //Is immortal high enough?
                var magicOption = Game.ReferenceManager.GetMagicOption(
                    RefMagicOpt.MaterialImmortal,
                    (byte)config.Item.Record.Degree
                );

                //Can not fuse if immortal is not available (or not high enough)
                var magicOptionInfo = config.Item.MagicOptions?.FirstOrDefault(m => m.Id == magicOption.Id);
                if (magicOptionInfo == null)
                {
                    Log.Notify(
                        $"[Alchemy] Could not fuse {astralStone.Record.GetRealName()} because the immortality option is not high enough"
                    );

                    _config.UseAstralStones = false;
                    return;
                }

                if (!AlchemyManager.TryFuseMagicStone(_config.Item, astralStone))
                    return;

                _shouldRun = false;
                _isStoneFusing = true;
                return;
            }
        }

        var nextPlusValue = config.Item.OptLevel + 1;

        Log.Notify($"[Alchemy] Attempting +{nextPlusValue}...");

        SendFusePacket();

        _shouldRun = false;
    }

    /// <summary>
    ///     Sends the fuse packet to the server
    /// </summary>
    private void SendFusePacket()
    {
        var effectiveElixirs = _activeRule?.Elixirs ?? _config?.Elixirs;
        if (_config == null || !_shouldRun || effectiveElixirs == null || !effectiveElixirs.Any())
            return;

        // Resolve powder: rule override > default config powder > any matching inventory powder
        InventoryItem powder;
        var effectivePowders = _activeRule?.LuckyPowders ?? _config.LuckyPowders;
        if (effectivePowders != null && effectivePowders.Any())
        {
            powder = Game.Player.Inventory.GetItem(effectivePowders.First().ItemId);
            if (powder == null || powder.Amount == 0)
            {
                Bootstrap.StopWithReason("[Alchemy] Selected lucky powder no longer available in inventory, stopping!");
                return;
            }
        }
        else
        {
            powder = _luckyPowders.FirstOrDefault();
        }

        var elixir = Game.Player.Inventory.GetItem(effectiveElixirs.First().ItemId);
        if (elixir == null || elixir.Amount == 0)
        {
            Bootstrap.StopWithReason("[Alchemy] Elixir no longer available in inventory, stopping!");
            return;
        }

        AlchemyManager.TryFuseElixir(_config.Item, elixir, powder);
    }

    private void ReadyAfterBuffer()
    {
        _nextRunAfter = DateTime.UtcNow.AddMilliseconds(500);
        _shouldRun = true;
    }

    #endregion Methods

    #region Events

    /// <summary>
    ///     Will be triggered if any elixir alchemy operation was completed
    /// </summary>
    private void OnElixirAlchemy()
    {
        ReadyAfterBuffer();
    }

    /// <summary>
    ///     Will be triggered if any elixir alchemy operation was successful
    /// </summary>
    /// <param name="newItem"></param>
    private void OnElixirAlchemySuccess(InventoryItem oldItem, InventoryItem newItem, AlchemyType type)
    {
        if (Globals.Botbase.AlchemyEngine != AlchemyEngine.Enhance)
            return;

        //After fusing a magic stone (steady, astral & co.) tell the bot to continue to fuse elixirs!
        if (Bootstrap.IsActive && _isStoneFusing)
        {
            ReadyAfterBuffer();
            _isStoneFusing = false;
            if (_config != null)
            {
                newItem.Slot = _config.Item.Slot;
                _config.Item = newItem;
            }
        }

        if (type != AlchemyType.Elixir && type != AlchemyType.EnhancerElixir)
            return;

        var message = Game
            .ReferenceManager.GetTranslation("UIIT_MSG_REINFORCERR_SUCCESS")
            .JoymaxFormat(newItem.OptLevel);

        Log.Notify(message);
        Globals.View.AddLog(newItem.Record.GetRealName(), message);

        if (_config != null)
        {
            newItem.Slot = _config.Item.Slot;
            _config.Item = newItem;
        }

        ReadyAfterBuffer();
    }

    /// <summary>
    ///     Will be triggered if the selected item was destroyed. Logs a message and stops the bot
    /// </summary>
    /// <param name="oldItem">The the item that has been destroyed</param>
    /// <param name="type">The type of alchemy that was triggered</param>
    private void OnElixirAlchemyDestroyed(InventoryItem oldItem, AlchemyType type)
    {
        _shouldRun = false;
    }

    /// <summary>
    ///     Will be triggered if any elixir alchemy operation has failed. Logs a message and resets the current item
    /// </summary>
    /// <param name="newItem">The new item after the action has failed</param>
    /// <param name="type">The type of alchemy that was triggered</param>
    private void OnElixirAlchemyFailed(InventoryItem oldItem, InventoryItem newItem, AlchemyType type)
    {
        if (type != AlchemyType.Elixir && type != AlchemyType.EnhancerElixir)
            return;

        ReadyAfterBuffer();
        var message = Game.ReferenceManager.GetTranslation("UIIT_MSG_REINFORCERR_FAIL");
        Log.Warn(message);
        Globals.View.AddLog(newItem.Record.GetRealName(), message);

        if (oldItem == null)
            return;

        message = string.Empty;
        if (newItem.Durability < oldItem.Durability)
            message = Game
                .ReferenceManager.GetTranslation("UIIT_MSG_REINFORCERR_FAILDOWN_DURABILITY")
                .JoymaxFormat(newItem.Durability);

        if (oldItem.OptLevel > 0 && newItem.OptLevel == 0)
            message = Game.ReferenceManager.GetTranslation("UIIT_MSG_REINFORCERR_FAIL_RESULT_OPTLV_ZERO");

        if (oldItem.OptLevel > 0 && oldItem.OptLevel < newItem.OptLevel)
            message = Game
                .ReferenceManager.GetTranslation("UIIT_MSG_REINFORCERR_FAIL_RESULT_OPTLV_DOWN")
                .JoymaxFormat(newItem.OptLevel, _config.Item.OptLevel - newItem.OptLevel);

        //Additional message
        if (message != string.Empty)
        {
            Log.Debug(message);
            Globals.View.AddLog(newItem.Record.GetRealName(), message);
        }

        if (_config != null)
        {
            newItem.Slot = _config.Item.Slot;
            _config.Item = newItem;
        }
    }

    /// <summary>
    ///     Called when [fuse request].
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="type">The type.</param>
    private void OnFuseRequest(AlchemyAction action, AlchemyType type)
    {
        _shouldRun = false;
    }

    /// <summary>
    ///     Resets run state so the bot can retry or stop cleanly on the next tick.
    ///     OnAlchemySuccess/Failed are not fired for result=2 responses, so this
    ///     is the only way to unblock the Run() loop after a server rejection.
    /// </summary>
    private void OnElixirAlchemyError(ushort errorCode, AlchemyType type)
    {
        ReadyAfterBuffer();
        _isStoneFusing = false;
    }


    #endregion Events
}
