using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Event;
using RSBot.Core.Objects;
using RSBot.Core.Objects.Spawn;
using SDUI.Controls;

namespace RSBot.Views;

public partial class AutoSelectWindow : UIWindow
{
    private readonly GlobalKeyboardHook _hook;
    private static readonly Random _rng = new();
    private Keys _hotkey = Keys.Space;
    private bool _listeningForKey;

    private static readonly (string Display, BadEffect Value)[] BadEffectOptions =
    {
        ("Bleed",       BadEffect.Bleed),
        ("Poisoned",    BadEffect.Poisoned),
        ("Frozen",      BadEffect.Frozen),
        ("Stunned",     BadEffect.Stunned),
        ("Burnt",       BadEffect.Burnt),
        ("Shocked",     BadEffect.Shocked),
        ("Sleep",       BadEffect.Sleep),
        ("Bind",        BadEffect.Bind),
        ("Fear",        BadEffect.Fear),
        ("Darkness",    BadEffect.Darkness),
        ("Petrify",     BadEffect.Petrify),
    };

    public AutoSelectWindow()
    {
        InitializeComponent();

        _hook = new GlobalKeyboardHook();
        _hook.KeyDown += OnGlobalKeyDown;

        foreach (var (display, _) in BadEffectOptions)
            comboBadEffect.Items.Add(display);
        comboBadEffect.SelectedIndex = 0; // Bleed

        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        EventManager.SubscribeEvent("OnLoadCharacter", RefreshFollowList);
        EventManager.SubscribeEvent("OnSpawnPlayer",   new Action<SpawnedPlayer>(OnSpawnPlayer));
        EventManager.SubscribeEvent("OnDespawnPlayer", new Action<SpawnedPlayer>(OnDespawnPlayer));
    }

    private void OnSpawnPlayer(SpawnedPlayer player)
    {
        if (IsDisposed || Disposing) return;
        void Add()
        {
            if (!comboFollowTarget.Items.Contains(player.Name))
                comboFollowTarget.Items.Add(player.Name);

            var guildName = player.Guild?.Name;
            if (!string.IsNullOrEmpty(guildName) && lstGuilds.Items.IndexOf(guildName) < 0 && lstGuilds.Items.Count < 8)
                lstGuilds.Items.Add(guildName, false);
        }
        if (InvokeRequired) BeginInvoke(Add); else Add();
    }

    private void OnDespawnPlayer(SpawnedPlayer player)
    {
        if (IsDisposed || Disposing) return;
        void Remove()
        {
            var current = comboFollowTarget.Text;
            comboFollowTarget.Items.Remove(player.Name);
            comboFollowTarget.Text = current;
        }
        if (InvokeRequired) BeginInvoke(Remove); else Remove();
    }

    private void RefreshFollowList()
    {
        if (IsDisposed || Disposing) return;
        void Populate()
        {
            var current = comboFollowTarget.Text;
            comboFollowTarget.Items.Clear();
            if (SpawnManager.TryGetEntities<SpawnedPlayer>(out var players))
                foreach (var p in players.OrderBy(p => p.Name))
                    comboFollowTarget.Items.Add(p.Name);
            comboFollowTarget.Text = current;
        }
        if (InvokeRequired) BeginInvoke(Populate); else Populate();
    }

    private void RefreshGuildList()
    {
        if (IsDisposed || Disposing) return;
        void Populate()
        {
            var checked_ = lstGuilds.CheckedItems.Cast<string>().ToHashSet();
            lstGuilds.Items.Clear();
            if (SpawnManager.TryGetEntities<SpawnedPlayer>(out var players))
                foreach (var name in players
                    .Select(p => p.Guild?.Name)
                    .Where(n => !string.IsNullOrEmpty(n))
                    .Distinct()
                    .OrderBy(n => n))
                    lstGuilds.Items.Add(name, checked_.Contains(name));
        }
        if (InvokeRequired) BeginInvoke(Populate); else Populate();
    }

    private void OnGlobalKeyDown(Keys key)
    {
        if (key == _hotkey)
            SelectNext();
    }

    public void SelectNext()
    {
        if (Game.Player == null || Game.Player.State.LifeState != LifeState.Alive)
            return;

        // If current target still has the selected bad effect, keep focus
        if (chkKeepFocus.Checked && chkPriorityBadEffect.Checked && comboBadEffect.SelectedIndex >= 0)
        {
            var current = Game.SelectedEntity as SpawnedPlayer;
            if (current != null
                && current.State.LifeState == LifeState.Alive
                && (current.BadEffect & BadEffectOptions[comboBadEffect.SelectedIndex].Value) != 0)
                return;
        }

        if (chkFollowTarget.Checked && !string.IsNullOrWhiteSpace(comboFollowTarget.Text) && TrySelectFollowTarget())
            return;

        var radius    = (double)nudRadius.Value;
        var currentId = Game.SelectedEntity?.UniqueId;
        var candidates = new List<SpawnedPlayer>();

        var allowed = AllowedJobs();
        if (allowed.Count > 0 &&
            SpawnManager.TryGetEntities<SpawnedPlayer>(
                p => p.WearsJobSuite
                     && allowed.Contains(p.Job)
                     && p.State.LifeState == LifeState.Alive
                     && !p.IsBehindObstacle
                     && p.DistanceToPlayer <= radius
                     && p.UniqueId != currentId,
                out var jobTargets))
            candidates.AddRange(jobTargets);

        var targetGuilds = lstGuilds.CheckedItems.Cast<string>().ToHashSet();
        if (targetGuilds.Count > 0 &&
            SpawnManager.TryGetEntities<SpawnedPlayer>(
                p => !string.IsNullOrEmpty(p.Guild?.Name)
                     && targetGuilds.Contains(p.Guild.Name)
                     && p.State.LifeState == LifeState.Alive
                     && !p.IsBehindObstacle
                     && p.DistanceToPlayer <= radius
                     && p.UniqueId != currentId,
                out var enemyTargets))
            candidates.AddRange(enemyTargets);

        if (candidates.Count == 0)
            return;

        SpawnedPlayer pick;
        if (chkPriorityBadEffect.Checked && comboBadEffect.SelectedIndex >= 0)
        {
            var effect   = BadEffectOptions[comboBadEffect.SelectedIndex].Value;
            var priority = candidates.Where(p => (p.BadEffect & effect) != 0).ToList();
            var pool     = priority.Count > 0 ? priority : candidates;
            pick = pool[_rng.Next(pool.Count)];
        }
        else
        {
            pick = candidates[_rng.Next(candidates.Count)];
        }

        pick.TrySelect();
    }

    private bool TrySelectFollowTarget()
    {
        var name = comboFollowTarget.Text.Trim();
        if (string.IsNullOrEmpty(name))
            return false;

        var followEntity = SpawnManager.GetEntity<SpawnedPlayer>(p => p.Name == name);
        if (followEntity == null || followEntity.TargetId == 0)
            return false;

        if (Game.SelectedEntity?.UniqueId == followEntity.TargetId)
            return true;

        if (!SpawnManager.TryGetEntity<SpawnedBionic>(followEntity.TargetId, out var target))
            return false;

        if (target.State.LifeState != LifeState.Alive || target.IsBehindObstacle)
            return false;

        if (target.DistanceToPlayer > (double)nudRadius.Value)
            return false;

        return target.TrySelect();
    }

    private List<JobType> AllowedJobs()
    {
        var jobs = new List<JobType>(3);
        if (chkThief.Checked)  jobs.Add(JobType.Thief);
        if (chkTrade.Checked)  jobs.Add(JobType.Trade);
        if (chkHunter.Checked) jobs.Add(JobType.Hunter);
        return jobs;
    }

    private void UpdateStatus()
    {
        if (chkEnabled.Checked)
        {
            lblStatus.Text      = "Active";
            lblStatus.ForeColor = Color.FromArgb(33, 150, 243);
            TitleColor          = Color.FromArgb(33, 150, 243);
        }
        else
        {
            lblStatus.Text      = "Idle";
            lblStatus.ForeColor = SystemColors.ControlText;
            TitleColor          = Color.Transparent;
        }
    }

    private void chkEnabled_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEnabled.Checked)
            _hook.Start();
        else
            _hook.Stop();

        UpdateStatus();
    }

    private void chkFollowTarget_CheckedChanged(object sender, EventArgs e)
    {
        comboFollowTarget.Enabled  = chkFollowTarget.Checked;
        btnRefreshFollow.Enabled   = chkFollowTarget.Checked;
    }

    private void chkPriorityBadEffect_CheckedChanged(object sender, EventArgs e)
    {
        comboBadEffect.Enabled = chkPriorityBadEffect.Checked;
    }

    private void btnRefreshFollow_Click(object sender, EventArgs e)
    {
        RefreshFollowList();
    }

    private void btnRefreshGuilds_Click(object sender, EventArgs e)
    {
        RefreshGuildList();
    }

    private void btnHotkey_Click(object sender, EventArgs e)
    {
        _listeningForKey    = true;
        btnHotkey.Text      = "Press any key...";
        btnHotkey.ForeColor = Color.FromArgb(33, 150, 243);
    }

    private void AutoSelectWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (!_listeningForKey)
            return;

        e.Handled        = true;
        e.SuppressKeyPress = true;
        _listeningForKey = false;
        _hotkey          = e.KeyCode;
        btnHotkey.Text      = e.KeyCode.ToString();
        btnHotkey.ForeColor = SystemColors.ControlText;
    }

    private void AutoSelectWindow_FormClosed(object sender, FormClosedEventArgs e)
    {
        _hook.Dispose();
    }
}
