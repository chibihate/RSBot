using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Event;
using RSBot.Core.Objects.Spawn;
using SDUI.Controls;

namespace RSBot.Views;

public partial class PlayerFollowWindow : UIWindow
{
    public PlayerFollowWindow()
    {
        InitializeComponent();
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        EventManager.SubscribeEvent("OnLoadCharacter", RefreshPlayerList);
        EventManager.SubscribeEvent("OnSpawnPlayer",  new Action<SpawnedPlayer>(OnSpawnPlayer));
        EventManager.SubscribeEvent("OnDespawnPlayer", new Action<SpawnedPlayer>(OnDespawnPlayer));
    }

    private void OnSpawnPlayer(SpawnedPlayer player)
    {
        if (IsDisposed || Disposing) return;
        void Add()
        {
            if (!comboTarget.Items.Contains(player.Name))
                comboTarget.Items.Add(player.Name);
        }
        if (InvokeRequired) BeginInvoke(Add); else Add();
    }

    private void OnDespawnPlayer(SpawnedPlayer player)
    {
        if (IsDisposed || Disposing) return;
        void Remove()
        {
            var currentText = comboTarget.Text;
            comboTarget.Items.Remove(player.Name);
            comboTarget.Text = currentText;
        }
        if (InvokeRequired) BeginInvoke(Remove); else Remove();
    }

    private void RefreshPlayerList()
    {
        if (IsDisposed || Disposing) return;
        void Populate()
        {
            var current = comboTarget.Text;
            comboTarget.Items.Clear();

            if (SpawnManager.TryGetEntities<SpawnedPlayer>(out var players))
                foreach (var p in players.OrderBy(p => p.Name))
                    comboTarget.Items.Add(p.Name);

            comboTarget.Text = current;
        }
        if (InvokeRequired) BeginInvoke(Populate); else Populate();
    }

    private void UpdateStatus()
    {
        if (PlayerFollowService.Enabled && !string.IsNullOrWhiteSpace(PlayerFollowService.TargetName))
        {
            lblStatus.Text = $"Following: {PlayerFollowService.TargetName}";
            lblStatus.ForeColor = Color.FromArgb(33, 150, 243);
            TitleColor = Color.FromArgb(33, 150, 243);
        }
        else
        {
            lblStatus.Text = "Idle";
            lblStatus.ForeColor = SystemColors.ControlText;
            TitleColor = Color.Transparent;
        }
    }

    // ── Event handlers ────────────────────────────────────────────────────────

    private void chkEnabled_CheckedChanged(object sender, EventArgs e)
    {
        var name = comboTarget.Text.Trim();
        if (chkEnabled.Checked && string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Enter or select a target player name first.", "Player Follow",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            chkEnabled.Checked = false;
            return;
        }

        PlayerFollowService.SetTarget(name);
        PlayerFollowService.FollowDistance = (double)nudDistance.Value;
        PlayerFollowService.SetEnabled(chkEnabled.Checked);
        UpdateStatus();
    }

    private void comboTarget_TextChanged(object sender, EventArgs e)
    {
        PlayerFollowService.SetTarget(comboTarget.Text.Trim());
        if (PlayerFollowService.Enabled)
            UpdateStatus();
    }

    private void nudDistance_ValueChanged(object sender, EventArgs e)
    {
        PlayerFollowService.FollowDistance = (double)nudDistance.Value;
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        RefreshPlayerList();
    }

    private void PlayerFollowWindow_FormClosed(object sender, FormClosedEventArgs e)
    {
        PlayerFollowService.SetEnabled(false);
    }
}
