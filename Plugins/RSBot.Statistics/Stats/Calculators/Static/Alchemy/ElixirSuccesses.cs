using System;
using System.Collections.Generic;
using System.Linq;
using RSBot.Core.Client.ReferenceObjects;
using RSBot.Core.Event;
using RSBot.Core.Objects;
using RSBot.Statistics.Stats;
using RSBot.Statistics.Stats.Calculators;

namespace RSBot.Statistics.Stats.Calculators.Static;

internal class ElixirSuccesses : IStatisticCalculator
{
    // Store achieved opt level per success to allow live milestone filtering.
    private readonly List<byte> _achievedLevels = new();

    public string Name => "ElixirSuccesses";
    public string Label => "Enhance - Successes";
    public StatisticsGroup Group => StatisticsGroup.Alchemy;
    public string ValueFormat => "{0}";
    public UpdateType UpdateType => UpdateType.Static;

    // achievedLevel > milestone  ↔  sourceLevel (achieved - 1) >= milestone
    public object GetValue() => _achievedLevels.Count(l => l > AlchemyMilestone.Level);
    public void Reset() => _achievedLevels.Clear();

    public void Initialize() =>
        EventManager.SubscribeEvent("OnAlchemySuccess",
            new Action<InventoryItem, InventoryItem, AlchemyType>(OnSuccess));

    private void OnSuccess(InventoryItem oldItem, InventoryItem newItem, AlchemyType type)
    {
        if (type == AlchemyType.Elixir)
            _achievedLevels.Add(newItem.OptLevel);
    }
}
