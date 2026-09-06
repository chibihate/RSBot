using System;
using System.Collections.Generic;
using System.Linq;
using RSBot.Core.Client.ReferenceObjects;
using RSBot.Core.Event;
using RSBot.Core.Objects;
using RSBot.Statistics.Stats;
using RSBot.Statistics.Stats.Calculators;

namespace RSBot.Statistics.Stats.Calculators.Static;

internal class ElixirFailures : IStatisticCalculator
{
    // Store source opt level (oldItem) per failure to allow live milestone filtering.
    private readonly List<byte> _sourceLevels = new();

    public string Name => "ElixirFailures";
    public string Label => "Enhance - Failures";
    public StatisticsGroup Group => StatisticsGroup.Alchemy;
    public string ValueFormat => "{0}";
    public UpdateType UpdateType => UpdateType.Static;

    public object GetValue() => _sourceLevels.Count(l => l >= AlchemyMilestone.Level);
    public void Reset() => _sourceLevels.Clear();

    public void Initialize() =>
        EventManager.SubscribeEvent("OnAlchemyFailed",
            new Action<InventoryItem, InventoryItem, AlchemyType>(OnFailed));

    private void OnFailed(InventoryItem oldItem, InventoryItem newItem, AlchemyType type)
    {
        if (type == AlchemyType.Elixir && oldItem != null)
            _sourceLevels.Add(oldItem.OptLevel);
    }
}
