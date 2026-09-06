using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RSBot.AutoScripts;
using RSBot.Core;
using RSBot.Core.Event;
using SDUI.Controls;

namespace RSBot.AutoScripts.Views;

[ToolboxItem(false)]
public partial class Main : DoubleBufferedControl
{
    private const string ConfigKeyScripts = "RSBot.AutoScript.Scripts";
    private const string ConfigKeyLoops   = "RSBot.AutoScript.Loops";
    private const string ConfigKeyDelay   = "RSBot.AutoScript.LoopDelay";

    public Main()
    {
        CheckForIllegalCrossThreadCalls = false;
        InitializeComponent();
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        EventManager.SubscribeEvent("OnAutoScriptRunning", new Action<int>(OnAutoScriptRunning));
        EventManager.SubscribeEvent("OnAutoScriptsStopped", OnAutoScriptsStopped);
        EventManager.SubscribeEvent("OnStopBot", new Action(OnBotStopped));
    }

    private void OnAutoScriptRunning(int index)
    {
        if (IsDisposed || Disposing) return;

        void Update()
        {
            var scripts = AppService.Bot.ScriptPaths;
            if (scripts.Count == 0) return;

            var safeIndex = index % scripts.Count;
            lstScripts.SelectedIndex = safeIndex;
            lblStatus.Text = $"Running {safeIndex + 1}/{scripts.Count}: {Path.GetFileName(scripts[safeIndex])}";
            SetRunningState(true);
        }

        if (InvokeRequired) BeginInvoke(Update);
        else Update();
    }

    private void OnAutoScriptsStopped()
    {
        if (IsDisposed || Disposing) return;

        void Update()
        {
            SetRunningState(false);
            lstScripts.SelectedIndex = -1;
        }

        if (InvokeRequired) BeginInvoke(Update);
        else Update();
    }

    private void OnBotStopped()
    {
        if (IsDisposed || Disposing) return;
        void Update() => SetRunningState(false);
        if (InvokeRequired) BeginInvoke(Update);
        else Update();
    }

    private void SetRunningState(bool running)
    {
        btnStart.Enabled = !running;
        btnStop.Enabled = running;
        lblStatus.Text = running ? lblStatus.Text : "Status: Idle";
    }

    public void LoadSettings()
    {
        nudLoops.Value = Math.Max(0, PlayerConfig.Get(ConfigKeyLoops, 1));
        nudDelay.Value = Math.Max(0, PlayerConfig.Get(ConfigKeyDelay, 0));

        var raw = PlayerConfig.Get(ConfigKeyScripts, string.Empty);
        lstScripts.Items.Clear();
        AppService.Bot.ScriptPaths.Clear();

        if (!string.IsNullOrWhiteSpace(raw))
        {
            foreach (var path in raw.Split(';').Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                AppService.Bot.ScriptPaths.Add(path);
                lstScripts.Items.Add(Path.GetFileName(path));
            }
        }

        ApplyOptions();
        SetRunningState(AppService.Bot.IsRunning);
    }

    private void ApplyOptions()
    {
        AppService.Bot.LoopCount = (int)nudLoops.Value;
        AppService.Bot.LoopDelay = (int)nudDelay.Value * 1000;
    }

    private void SaveSettings()
    {
        PlayerConfig.Set(ConfigKeyScripts, string.Join(";", AppService.Bot.ScriptPaths));
        PlayerConfig.Set(ConfigKeyLoops, (int)nudLoops.Value);
        PlayerConfig.Set(ConfigKeyDelay, (int)nudDelay.Value);
        PlayerConfig.Save();
        ApplyOptions();
    }

    private void nudOptions_ValueChanged(object sender, EventArgs e) => SaveSettings();

    private void btnStart_Click(object sender, EventArgs e)
    {
        if (AppService.Bot.ScriptPaths.Count == 0)
        {
            MessageBox.Show("Add at least one script to the list.", "Auto Scripts",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ApplyOptions();
        AppService.Bot.Start();
        SetRunningState(true);
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        AppService.Bot.Stop();
        SetRunningState(false);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Add Script",
            Filter = "RSBot Script (*.rbs)|*.rbs|All Files (*.*)|*.*",
            InitialDirectory = Path.Combine(Kernel.BasePath, "Data", "Scripts"),
            Multiselect = true,
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        foreach (var file in dlg.FileNames)
        {
            AppService.Bot.ScriptPaths.Add(file);
            lstScripts.Items.Add(Path.GetFileName(file));
        }

        SaveSettings();
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
        var idx = lstScripts.SelectedIndex;
        if (idx < 0) return;

        AppService.Bot.ScriptPaths.RemoveAt(idx);
        lstScripts.Items.RemoveAt(idx);

        if (lstScripts.Items.Count > 0)
            lstScripts.SelectedIndex = Math.Min(idx, lstScripts.Items.Count - 1);

        SaveSettings();
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
        var idx = lstScripts.SelectedIndex;
        if (idx <= 0) return;

        SwapItems(idx, idx - 1);
        lstScripts.SelectedIndex = idx - 1;
        SaveSettings();
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
        var idx = lstScripts.SelectedIndex;
        if (idx < 0 || idx >= lstScripts.Items.Count - 1) return;

        SwapItems(idx, idx + 1);
        lstScripts.SelectedIndex = idx + 1;
        SaveSettings();
    }

    private void SwapItems(int a, int b)
    {
        (AppService.Bot.ScriptPaths[a], AppService.Bot.ScriptPaths[b]) =
            (AppService.Bot.ScriptPaths[b], AppService.Bot.ScriptPaths[a]);

        var tmp = lstScripts.Items[a];
        lstScripts.Items[a] = lstScripts.Items[b];
        lstScripts.Items[b] = tmp;
    }

    private void lstScripts_DoubleClick(object sender, EventArgs e)
    {
        var idx = lstScripts.SelectedIndex;
        if (idx < 0 || idx >= AppService.Bot.ScriptPaths.Count) return;

        using var dlg = new OpenFileDialog
        {
            Title = "Replace Script",
            Filter = "RSBot Script (*.rbs)|*.rbs|All Files (*.*)|*.*",
            InitialDirectory = Path.GetDirectoryName(AppService.Bot.ScriptPaths[idx])
                               ?? Path.Combine(Kernel.BasePath, "Data", "Scripts"),
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        AppService.Bot.ScriptPaths[idx] = dlg.FileName;
        lstScripts.Items[idx] = Path.GetFileName(dlg.FileName);
        SaveSettings();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
}
