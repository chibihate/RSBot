using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using RSBot.Alchemy.Bot;
using RSBot.Alchemy.Bundle.Enhance;
using RSBot.Alchemy.Helper;
using RSBot.Core;
using RSBot.Core.Objects;
using SDUI.Controls;

namespace RSBot.Alchemy.Views.Settings;

[ToolboxItem(false)]
public partial class EnhanceSettingsView : DoubleBufferedControl
{
    #region Member

    private InventoryItem _selectedItem;
    private readonly List<AlchemyRule> _rules = new();
    private bool _isPopulating;

    #endregion Member

    #region Constructor

    public EnhanceSettingsView()
    {
        CheckForIllegalCrossThreadCalls = false;
        InitializeComponent();
        SetStyle(
            ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer,
            true
        );
    }

    #endregion Constructor

    internal class ElixirComboboxItem
    {
        public ElixirComboboxItem(IEnumerable<InventoryItem> items)
        {
            Items = items;
        }

        public IEnumerable<InventoryItem> Items { get; set; }

        public override string ToString()
        {
            if (!Items.Any())
                return string.Empty;

            return $"{Items.Sum(i => i.Amount)}x {Items.First().Record.GetRealName()}";
        }
    }

    internal class LuckyPowderComboboxItem
    {
        public LuckyPowderComboboxItem(IEnumerable<InventoryItem> items)
        {
            Items = items;
        }

        public IEnumerable<InventoryItem> Items { get; set; }

        public override string ToString()
        {
            if (!Items.Any())
                return string.Empty;

            return $"{Items.Sum(i => i.Amount)}x {Items.First().Record.GetRealName()}";
        }
    }

    #region Methods

    private void PopulateView()
    {
        if (InvokeRequired)
        {
            BeginInvoke(PopulateView);
            return;
        }

        if (Globals.View == null || Globals.View.SelectedItem == null)
        {
            Enabled = false;
            return;
        }

        _isPopulating = true;
        try
        {
        _selectedItem = Globals.View.SelectedItem;
        lblCurrentOptLevel.Text = _selectedItem == null ? "+0" : $"+{Globals.View.SelectedItem.OptLevel}";

        var type = AlchemyItemHelper.ElixirType.Unspecified;
        var accessorryTypeId3 = new byte[] { 5, 12 };
        var armorTypeId3 = new byte[] { 1, 2, 3, 9, 10, 11 };

        if (_selectedItem.Record.TypeID3 == 4 && _selectedItem.Record.TypeID2 == 1)
            type = AlchemyItemHelper.ElixirType.Shield;

        if (_selectedItem.Record.TypeID3 == 6 && _selectedItem.Record.TypeID2 == 1)
            type = AlchemyItemHelper.ElixirType.Weapon;

        if (accessorryTypeId3.Contains(_selectedItem.Record.TypeID3) && _selectedItem.Record.TypeID2 == 1)
            type = AlchemyItemHelper.ElixirType.Accessory;

        if (armorTypeId3.Contains(_selectedItem.Record.TypeID3) && _selectedItem.Record.TypeID2 == 1)
            type = AlchemyItemHelper.ElixirType.Protector;

        // Populate default elixir combo
        var matchingElixirs = AlchemyItemHelper.GetElixirItems(_selectedItem.Record.Degree, type);
        comboElixir.Items.Clear();

        var index = 0;
        foreach (var items in matchingElixirs.GroupBy(i => i.ItemId))
        {
            comboElixir.Items.Add(new ElixirComboboxItem(items));

            if (items.Key == Globals.Botbase.EnhanceBundleConfig?.Elixirs?.FirstOrDefault()?.ItemId)
                comboElixir.SelectedIndex = index;

            index++;
        }

        if (comboElixir.Items.Count > 0 && comboElixir.SelectedItem == null)
            comboElixir.SelectedIndex = 0;

        // Populate default lucky powder combo
        var luckyPowders = AlchemyItemHelper.GetLuckyPowders(_selectedItem);
        comboLuckyPowder.Items.Clear();

        int powderIndex = 0;
        int selectedPowderIndex = 0;
        foreach (var items in luckyPowders.GroupBy(i => i.ItemId))
        {
            comboLuckyPowder.Items.Add(new LuckyPowderComboboxItem(items));

            if (items.Key == Globals.Botbase.EnhanceBundleConfig?.LuckyPowders?.FirstOrDefault()?.ItemId)
                selectedPowderIndex = powderIndex;

            powderIndex++;
        }

        if (comboLuckyPowder.Items.Count > 0)
            comboLuckyPowder.SelectedIndex = selectedPowderIndex;

        // Stones
        var luckyStones = AlchemyItemHelper.GetLuckyStone(_selectedItem);
        checkUseLuckyStones.Enabled = luckyStones != null && luckyStones.Amount > 0;
        if (luckyStones == null)
            checkUseLuckyStones.Checked = false;
        lblLuckyCount.Text = luckyStones == null ? "x0" : $"x{luckyStones.Amount}";

        var astralStones = AlchemyItemHelper.GetAstralStone(_selectedItem);
        if (astralStones == null)
            checkUseAstralStones.Checked = false;
        checkUseAstralStones.Enabled = astralStones != null && astralStones.Amount > 0;
        lblAstralCount.Text = astralStones == null ? "x0" : $"x{astralStones.Amount}";

        var immortalStones = AlchemyItemHelper.GetImmortalStone(_selectedItem);
        if (immortalStones == null)
            checkUseImmortalStones.Checked = false;
        checkUseImmortalStones.Enabled = immortalStones != null && immortalStones.Amount > 0;
        lblImmortalCount.Text = immortalStones == null ? "x0" : $"x{immortalStones.Amount}";

        var steadyStones = AlchemyItemHelper.GetSteadyStone(_selectedItem);
        checkUseSteadyStones.Enabled = steadyStones != null && steadyStones.Amount > 0;
        lblSteadyStonesCount.Text = steadyStones == null ? "x0" : $"x{steadyStones.Amount}";

        PopulateRulesEditorCombos();

        }
        catch (Exception ex)
        {
            Log.Debug($"[Alchemy] Error populating enhance settings view: {ex.Message}");
        }
        finally
        {
            _isPopulating = false;
            Enabled = true;
            UpdateConfig();
        }
    }

    private void PopulateRulesEditorCombos()
    {
        if (_selectedItem == null)
            return;

        // Populate rule elixir combo
        comboRuleElixir.Items.Clear();
        comboRuleElixir.Items.Add("(Default)");

        var type = AlchemyItemHelper.ElixirType.Unspecified;
        var accessorryTypeId3 = new byte[] { 5, 12 };
        var armorTypeId3 = new byte[] { 1, 2, 3, 9, 10, 11 };

        if (_selectedItem.Record.TypeID3 == 4 && _selectedItem.Record.TypeID2 == 1)
            type = AlchemyItemHelper.ElixirType.Shield;
        if (_selectedItem.Record.TypeID3 == 6 && _selectedItem.Record.TypeID2 == 1)
            type = AlchemyItemHelper.ElixirType.Weapon;
        if (accessorryTypeId3.Contains(_selectedItem.Record.TypeID3) && _selectedItem.Record.TypeID2 == 1)
            type = AlchemyItemHelper.ElixirType.Accessory;
        if (armorTypeId3.Contains(_selectedItem.Record.TypeID3) && _selectedItem.Record.TypeID2 == 1)
            type = AlchemyItemHelper.ElixirType.Protector;

        foreach (var items in AlchemyItemHelper.GetElixirItems(_selectedItem.Record.Degree, type).GroupBy(i => i.ItemId))
            comboRuleElixir.Items.Add(new ElixirComboboxItem(items));

        comboRuleElixir.SelectedIndex = 0;

        // Populate rule powder combo
        comboRulePowder.Items.Clear();
        comboRulePowder.Items.Add("(Default)");

        foreach (var items in AlchemyItemHelper.GetLuckyPowders(_selectedItem).GroupBy(i => i.ItemId))
            comboRulePowder.Items.Add(new LuckyPowderComboboxItem(items));

        comboRulePowder.SelectedIndex = 0;
    }

    private void RefreshRulesList()
    {
        listRules.Items.Clear();

        foreach (var rule in _rules.OrderBy(r => r.OptLevel))
        {
            var elixirName = rule.Elixirs?.FirstOrDefault()?.Record?.GetRealName() ?? "(Default)";
            var powderName = rule.LuckyPowders?.FirstOrDefault()?.Record?.GetRealName() ?? "(Default)";
            var luckyStr = rule.MaxLuckyTimes.HasValue ? rule.MaxLuckyTimes.Value.ToString() : "(Default)";

            var item = new ListViewItem($"+{rule.OptLevel}");
            item.SubItems.Add(elixirName);
            item.SubItems.Add(powderName);
            item.SubItems.Add(luckyStr);
            item.Tag = rule;

            listRules.Items.Add(item);
        }
    }

    private void UpdateConfig()
    {
        if (Globals.Botbase == null || Globals.Botbase.AlchemyEngine != AlchemyEngine.Enhance)
            return;

        Globals.Botbase.EnhanceBundleConfig = new EnhanceBundleConfig
        {
            Item = Globals.View.SelectedItem,
            UseAstralStones = checkUseAstralStones.Checked,
            UseLuckyStones = checkUseLuckyStones.Checked,
            LuckyStoneFromLevel = (byte)numLuckyFromLevel.Value,
            UseImmortalStones = checkUseImmortalStones.Checked,
            UseSteadyStones = checkUseSteadyStones.Checked,
            Elixirs = (comboElixir.SelectedItem as ElixirComboboxItem)?.Items,
            LuckyPowders = (comboLuckyPowder.SelectedItem as LuckyPowderComboboxItem)?.Items,
            MaxOptLevel = (byte)numMaxEnhancement.Value,
            Rules = new List<AlchemyRule>(_rules),
        };
    }

    #endregion Methods

    #region Events

    internal void View_EngineChanged(InventoryItem item, AlchemyEngine alchemyEngine)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => View_EngineChanged(item, alchemyEngine));
            return;
        }
        PopulateView();
    }

    private void linkRefreshItemList_Click(object sender, EventArgs e)
    {
        PopulateView();
    }

    internal void View_ItemChanged(InventoryItem item)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => View_ItemChanged(item));
            return;
        }
        PopulateView();
    }

    private void config_CheckedChange(object sender, EventArgs e)
    {
        if (_isPopulating)
            return;
        UpdateConfig();
    }

    private void btnAddRule_Click(object sender, EventArgs e)
    {
        var targetLevel = (byte)numRuleLevel.Value;

        var elixirs = comboRuleElixir.SelectedItem is ElixirComboboxItem exi ? exi.Items : null;
        var powders = comboRulePowder.SelectedItem is LuckyPowderComboboxItem pdi ? pdi.Items : null;
        var maxLucky = numRuleLucky.Value > 0 ? (byte?)numRuleLucky.Value : null;

        // Replace existing rule for the same level
        _rules.RemoveAll(r => r.OptLevel == targetLevel);
        _rules.Add(new AlchemyRule
        {
            OptLevel = targetLevel,
            Elixirs = elixirs,
            LuckyPowders = powders,
            MaxLuckyTimes = maxLucky,
        });

        RefreshRulesList();
        UpdateConfig();
    }

    private void btnRemoveRule_Click(object sender, EventArgs e)
    {
        if (listRules.SelectedItems.Count == 0)
            return;

        var rule = listRules.SelectedItems[0].Tag as AlchemyRule;
        if (rule != null)
        {
            _rules.Remove(rule);
            RefreshRulesList();
            UpdateConfig();
        }
    }

    private void btnRemoveAllRules_Click(object sender, EventArgs e)
    {
        _rules.Clear();
        RefreshRulesList();
        UpdateConfig();
    }

    private void listRules_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listRules.SelectedItems.Count == 0)
            return;

        var rule = listRules.SelectedItems[0].Tag as AlchemyRule;
        if (rule == null)
            return;

        numRuleLevel.Value = rule.OptLevel;
        numRuleLucky.Value = rule.MaxLuckyTimes ?? 0;

        // Select matching elixir in editor combo
        comboRuleElixir.SelectedIndex = 0;
        if (rule.Elixirs != null)
        {
            var elixirId = rule.Elixirs.FirstOrDefault()?.ItemId;
            for (int i = 1; i < comboRuleElixir.Items.Count; i++)
            {
                if (comboRuleElixir.Items[i] is ElixirComboboxItem exi &&
                    exi.Items.FirstOrDefault()?.ItemId == elixirId)
                {
                    comboRuleElixir.SelectedIndex = i;
                    break;
                }
            }
        }

        // Select matching powder in editor combo
        comboRulePowder.SelectedIndex = 0;
        if (rule.LuckyPowders != null)
        {
            var powderId = rule.LuckyPowders.FirstOrDefault()?.ItemId;
            for (int i = 1; i < comboRulePowder.Items.Count; i++)
            {
                if (comboRulePowder.Items[i] is LuckyPowderComboboxItem pdi &&
                    pdi.Items.FirstOrDefault()?.ItemId == powderId)
                {
                    comboRulePowder.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    #endregion Events
}
