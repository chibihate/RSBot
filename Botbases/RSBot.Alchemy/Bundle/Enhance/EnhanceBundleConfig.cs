using System.Collections.Generic;
using RSBot.Core.Objects;

namespace RSBot.Alchemy.Bundle.Enhance;

internal class EnhanceBundleConfig
{
    #region Properties

    /// <summary>
    ///     Gets or sets the maximum opt level for the item
    /// </summary>
    public byte MaxOptLevel { get; set; }

    /// <summary>
    ///     Gets or sets the inventory item used to do the alchemy on
    /// </summary>
    public InventoryItem Item { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating if immortal stones should be used
    /// </summary>
    public bool UseImmortalStones { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating if astral stones should be used
    /// </summary>
    public bool UseAstralStones { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating if steady stones should be used
    /// </summary>
    public bool UseSteadyStones { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating if lucky stones should be used
    /// </summary>
    public bool UseLuckyStones { get; set; }

    /// <summary>
    ///     Gets or sets the selected enhancer elixir
    /// </summary>
    public IEnumerable<InventoryItem> Elixirs { get; set; }

    /// <summary>
    ///     Gets or sets the selected lucky powder. Null means use any matching powder from inventory.
    /// </summary>
    public IEnumerable<InventoryItem> LuckyPowders { get; set; }

    /// <summary>
    ///     Gets or sets the minimum opt level at which lucky stones start being used (default 5)
    /// </summary>
    public byte LuckyStoneFromLevel { get; set; } = 5;

    /// <summary>
    ///     Gets or sets the list of per-level alchemy rules
    /// </summary>
    public List<AlchemyRule> Rules { get; set; } = new();

    #endregion Properties
}
