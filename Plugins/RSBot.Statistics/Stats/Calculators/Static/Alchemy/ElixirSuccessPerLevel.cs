using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RSBot.Core.Client.ReferenceObjects;
using RSBot.Core.Event;
using RSBot.Core.Objects;
using RSBot.Statistics.Stats;
using RSBot.Statistics.Stats.Calculators;

namespace RSBot.Statistics.Stats.Calculators.Static;

internal class ElixirSuccessPerLevel : IStatisticCalculator
{
    // Full history keyed by achieved opt level (newItem.OptLevel).
    private readonly SortedDictionary<byte, int> _byLevel = new();

    public string Name => "ElixirSuccessPerLevel";
    public string Label => "Enhance - Success / Level";
    public StatisticsGroup Group => StatisticsGroup.Alchemy;
    public string ValueFormat => "{0}";
    public UpdateType UpdateType => UpdateType.Static;

    public object GetValue()
    {
        // achievedLevel > milestone  ↔  sourceLevel (achieved - 1) >= milestone
        var filtered = _byLevel.Where(kv => kv.Key > AlchemyMilestone.Level).ToList();
        if (filtered.Count == 0)
            return "—";

        var sb = new StringBuilder();
        foreach (var kv in filtered)
        {
            if (sb.Length > 0) sb.Append("  ");
            sb.Append($"+{kv.Key}:{kv.Value}");
        }
        return sb.ToString();
    }

    public void Reset() => _byLevel.Clear();

    public void Initialize() =>
        EventManager.SubscribeEvent("OnAlchemySuccess",
            new Action<InventoryItem, InventoryItem, AlchemyType>(OnSuccess));

    private void OnSuccess(InventoryItem oldItem, InventoryItem newItem, AlchemyType type)
    {
        if (type != AlchemyType.Elixir)
            return;

        var level = newItem.OptLevel;
        _byLevel.TryGetValue(level, out var current);
        _byLevel[level] = current + 1;
    }
}
