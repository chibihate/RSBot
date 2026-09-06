using System.Collections.Generic;
using RSBot.Core.Objects;

namespace RSBot.Alchemy.Bundle.Enhance;

internal class AlchemyRule
{
    /// <summary>
    ///     The target opt level this rule applies to (the level being attempted to reach)
    /// </summary>
    public byte OptLevel { get; set; }

    /// <summary>
    ///     Override elixir for this level. Null means use the default elixir.
    /// </summary>
    public IEnumerable<InventoryItem> Elixirs { get; set; }

    /// <summary>
    ///     Override lucky powder for this level. Null means use the default lucky powder.
    /// </summary>
    public IEnumerable<InventoryItem> LuckyPowders { get; set; }

    /// <summary>
    ///     Maximum lucky times (value of MATTR_LUCK option on the item).
    ///     If the item's current lucky times >= this value, skip applying lucky stones.
    ///     Null means use the default behavior (always apply if enabled).
    /// </summary>
    public byte? MaxLuckyTimes { get; set; }
}
