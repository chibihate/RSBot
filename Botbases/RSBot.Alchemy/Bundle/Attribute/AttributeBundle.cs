using System;
using System.Linq;
using RSBot.Core;
using RSBot.Core.Client.ReferenceObjects;
using RSBot.Core.Components;
using RSBot.Core.Event;
using RSBot.Core.Extensions;
using RSBot.Core.Objects;

namespace RSBot.Alchemy.Bundle.Attribute;

internal class AttributeBundle : IAlchemyBundle
{
    #region Members

    private bool _shouldRun;
    private DateTime _nextRunAfter = DateTime.MinValue;

    #endregion Members

    #region Constructor

    public AttributeBundle()
    {
        SubscribeEvents();
    }

    #endregion Constructor

    #region Methods

    public void SubscribeEvents()
    {
        EventManager.SubscribeEvent(
            "OnAlchemySuccess",
            new Action<InventoryItem, InventoryItem, AlchemyType>(OnStoneAlchemySuccess)
        );
        EventManager.SubscribeEvent(
            "OnAlchemyFailed",
            new Action<InventoryItem, InventoryItem, AlchemyType>(OnStoneAlchemyFailed)
        );
        EventManager.SubscribeEvent("OnAlchemyError", new Action<ushort, AlchemyType>(OnStoneAlchemyError));
        EventManager.SubscribeEvent("OnFuseRequest", new Action<AlchemyAction, AlchemyType>(OnFuseRequest));
    }

    /// <summary>
    ///     Runs the attribute granter using the specified configuration.
    /// </summary>
    /// <param name="engineConfig">The configuration.</param>
    public void Run<T>(T engineConfig)
    {
        if (engineConfig is not AttributeBundleConfig config)
            return;

        if (!_shouldRun)
            return;

        if (DateTime.UtcNow < _nextRunAfter)
            return;

        if (!config.Attributes.Any())
        {
            Bootstrap.StopWithReason("[Alchemy] No attribute stone fusion configured!");
            return;
        }

        foreach (var attribute in config.Attributes)
        {
            if (_shouldRun == false)
                break;

            var slot = ItemAttributesInfo.GetAttributeSlotForItem(attribute.Group, config.Item.Record);
            var currentValue = config.Item.Attributes.GetPercentage(slot);

            if (currentValue >= attribute.MaxValue && attribute.MaxValue > 0)
                continue;

            var currentStone = Game.Player.Inventory.GetItem(s => s.ItemId == attribute.Stone.ItemId && s.Amount > 0);
            if (currentStone == null)
            {
                Bootstrap.StopWithReason($"[Alchemy] Attribute stone [{attribute.Stone.Record.GetRealName()}] is out of stock!");
                return;
            }

            if (!AlchemyManager.TryFuseAttributeStone(config.Item, currentStone))
                continue;

            _shouldRun = false;
        }

        if (_shouldRun)
            Bootstrap.StopWithSuccess("[Alchemy] Attribute stone fusing finished!");
    }

    /// <summary>
    ///     Stops this instance.
    /// </summary>
    public void Stop()
    {
        _shouldRun = false;
        _nextRunAfter = DateTime.MinValue;
    }

    /// <summary>
    ///     Starts this instance.
    /// </summary>
    public void Start()
    {
        _shouldRun = true;
    }

    private void ReadyAfterBuffer()
    {
        _nextRunAfter = DateTime.UtcNow.AddMilliseconds(500);
        _shouldRun = true;
    }

    #endregion Methods

    #region Events

    /// <summary>
    ///     Will be triggered if any fuse request was sent to the server
    /// </summary>
    private void OnFuseRequest(AlchemyAction action, AlchemyType type)
    {
        if (type != AlchemyType.AttributeStone || !Bootstrap.IsActive)
            return;

        _shouldRun = false;
    }

    /// <summary>
    ///     Will be triggered if a stone alchemy operation was successful
    /// </summary>
    private void OnStoneAlchemySuccess(InventoryItem oldItem, InventoryItem newItem, AlchemyType type)
    {
        if (type != AlchemyType.AttributeStone || !Bootstrap.IsActive)
            return;

        if (Globals.Botbase.AttributeBundleConfig != null)
            Globals.Botbase.AttributeBundleConfig.Item = newItem;

        ReadyAfterBuffer();

        var changedAttributeSlots = oldItem.Attributes.CompareSlots(newItem.Attributes);

        foreach (var slot in changedAttributeSlots)
        {
            var attributeGroup = ItemAttributesInfo.GetAttributeGroupBySlot(newItem.Record, slot);
            var attributeValue = newItem.Attributes.GetPercentage(slot);

            Globals.View.AddLog(
                newItem.Record.GetRealName(),
                $"The attribute [{attributeGroup.GetTranslation()}] changed to [+{attributeValue}%]"
            );
        }
    }

    /// <summary>
    ///     Will be triggered if a stone alchemy action failed
    /// </summary>
    private void OnStoneAlchemyFailed(InventoryItem oldItem, InventoryItem newItem, AlchemyType type)
    {
        if (type != AlchemyType.AttributeStone || !Bootstrap.IsActive)
            return;

        if (Globals.Botbase.AttributeBundleConfig != null)
            Globals.Botbase.AttributeBundleConfig.Item = newItem;

        ReadyAfterBuffer();

        Globals.View.AddLog(
            newItem.Record.GetRealName(),
            Game.ReferenceManager.GetTranslation("UIIT_MSG_REINFORCERR_FAIL")
        );
    }

    /// <summary>
    ///     Will be triggered if a stone alchemy error occurs
    /// </summary>
    private void OnStoneAlchemyError(ushort errorCode, AlchemyType type)
    {
        if (type != AlchemyType.AttributeStone || !Bootstrap.IsActive)
            return;

        ReadyAfterBuffer();

        Globals.View.AddLog(
            Globals.Botbase.AttributeBundleConfig?.Item?.Record?.GetRealName(),
            Game.ReferenceManager.GetTranslation("UIIT_MSG_REINFORCERR_FAIL")
        );
    }

    #endregion Events
}
