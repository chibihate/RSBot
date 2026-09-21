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

    public AutoSelectWindow()
    {
        InitializeComponent();

        _hook = new GlobalKeyboardHook();
        _hook.KeyDown += OnGlobalKeyDown;

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
        void Add() { if (!comboFollowTarget.Items.Contains(player.Name)) comboFollowTarget.Items.Add(player.Name); }
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

    private void OnGlobalKeyDown(Keys key)
    {
        if (key == _hotkey)
            SelectNext();
    }

    public void SelectNext()
    {
        if (Game.Player == null || Game.Player.State.LifeState != LifeState.Alive)
            return;

        if (!string.IsNullOrWhiteSpace(comboFollowTarget.Text) && TrySelectFollowTarget())
            return;

        var allowed = AllowedJobs();
        if (allowed.Count == 0)
            return;

        var radius    = (double)nudRadius.Value;
        var currentId = Game.SelectedEntity?.UniqueId;

        if (!SpawnManager.TryGetEntities<SpawnedPlayer>(
                p => p.WearsJobSuite
                     && allowed.Contains(p.Job)
                     && p.State.LifeState == LifeState.Alive
                     && !p.IsBehindObstacle
                     && p.DistanceToPlayer <= radius
                     && p.UniqueId != currentId,
                out var candidates))
            return;

        var list = candidates.ToList();
        if (list.Count == 0)
            return;

        list[_rng.Next(list.Count)].TrySelect();
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

    private void btnRefreshFollow_Click(object sender, EventArgs e)
    {
        RefreshFollowList();
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
