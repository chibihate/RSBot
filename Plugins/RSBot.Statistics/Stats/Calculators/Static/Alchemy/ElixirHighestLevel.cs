using System;
using RSBot.Core.Client.ReferenceObjects;
using RSBot.Core.Event;
using RSBot.Core.Objects;
using RSBot.Statistics.Stats;
using RSBot.Statistics.Stats.Calculators;

namespace RSBot.Statistics.Stats.Calculators.Static;

internal class ElixirHighestLevel : IStatisticCalculator
{
    private byte _highestLevel;

    public string Name => "ElixirHighestLevel";
    public string Label => "Enhance - Highest Level";
    public StatisticsGroup Group => StatisticsGroup.Alchemy;
    public string ValueFormat => "+{0}";
    public UpdateType UpdateType => UpdateType.Static;

    public object GetValue() => _highestLevel;
    public void Reset() => _highestLevel = 0;

    public void Initialize() =>
        EventManager.SubscribeEvent("OnAlchemySuccess",
            new Action<InventoryItem, InventoryItem, AlchemyType>(OnSuccess));

    private void OnSuccess(InventoryItem oldItem, InventoryItem newItem, AlchemyType type)
    {
        if (type == AlchemyType.Elixir && newItem.OptLevel > _highestLevel)
            _highestLevel = newItem.OptLevel;
    }
}
