using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RSBot.Scripts;
using RSBot.Core;
using RSBot.Core.Event;
using SDUI.Controls;

namespace RSBot.Scripts.Views;

[ToolboxItem(false)]
public partial class Main : DoubleBufferedControl
{
    private const string ConfigKeyScripts  = "RSBot.Scripts.Scripts";
    private const string ConfigKeyLoops    = "RSBot.Scripts.Loops";
    private const string ConfigKeyDelay    = "RSBot.Scripts.LoopDelay";
    private const string ConfigKeySound    = "RSBot.Scripts.SoundPath";
    private const string ConfigKeyScript1  = "RSBot.Scripts.Script1Path";
    private const string ConfigKeyScript2  = "RSBot.Scripts.Script2Path";
    private const string ConfigKeyScript3  = "RSBot.Scripts.Script3Path";
    private const string DefaultSoundPath  = @"C:\Windows\Media\Ring10.wav";

    private bool _loadingSettings;

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
        EventManager.SubscribeEvent("OnMinimapUpdated", new Action<Bitmap>(OnMinimapUpdated));

        AppService.Script1.OnStopped = () => UpdateSlotButtons(1, false);
        AppService.Script2.OnStopped = () => UpdateSlotButtons(2, false);
        AppService.Script3.OnStopped = () => UpdateSlotButtons(3, false);
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

    private void OnMinimapUpdated(Bitmap bitmap)
    {
        if (IsDisposed || Disposing) return;

        void Update()
        {
            var old = picMinimap.Image;
            picMinimap.Image = bitmap;
            old?.Dispose();
            lblMinimapPos.Text = Game.Player?.Position.ToString() ?? string.Empty;
        }

        if (InvokeRequired) BeginInvoke(Update);
        else Update();
    }

    private void SetRunningState(bool running)
    {
        btnStart.Enabled = !running;
        btnStop.Enabled = running;
        lblStatus.Text = running ? lblStatus.Text : "Status: Idle";
    }

    private void UpdateSlotButtons(int slot, bool running)
    {
        if (IsDisposed || Disposing) return;

        void Update()
        {
            switch (slot)
            {
                case 1:
                    btnScript1Play.Enabled = !running;
                    btnScript1Stop.Enabled = running;
                    break;
                case 2:
                    btnScript2Play.Enabled = !running;
                    btnScript2Stop.Enabled = running;
                    break;
                case 3:
                    btnScript3Play.Enabled = !running;
                    btnScript3Stop.Enabled = running;
                    break;
            }
        }

        if (InvokeRequired) BeginInvoke(Update);
        else Update();
    }

    public void LoadSettings()
    {
        _loadingSettings = true;
        nudLoops.Value = Math.Max(0, PlayerConfig.Get(ConfigKeyLoops, 1));
        nudDelay.Value = Math.Max(0, PlayerConfig.Get(ConfigKeyDelay, 0));
        txtSoundPath.Text = PlayerConfig.Get(ConfigKeySound, DefaultSoundPath);

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

        txtScript1Path.Text = PlayerConfig.Get(ConfigKeyScript1, string.Empty);
        txtScript2Path.Text = PlayerConfig.Get(ConfigKeyScript2, string.Empty);
        txtScript3Path.Text = PlayerConfig.Get(ConfigKeyScript3, string.Empty);

        _loadingSettings = false;
        ApplyOptions();
        SetRunningState(AppService.Bot.IsRunning);
        UpdateSlotButtons(1, AppService.Script1.IsRunning);
        UpdateSlotButtons(2, AppService.Script2.IsRunning);
        UpdateSlotButtons(3, AppService.Script3.IsRunning);
    }

    private void ApplyOptions()
    {
        AppService.Bot.LoopCount = (int)nudLoops.Value;
        AppService.Bot.LoopDelay = (int)nudDelay.Value * 1000;
        AppService.Bot.SoundFilePath = txtSoundPath.Text.Trim();
    }

    private void ApplyOptionsToSlot(AutoScriptsService svc)
    {
        svc.LoopCount = (int)nudLoops.Value;
        svc.LoopDelay = (int)nudDelay.Value * 1000;
        svc.SoundFilePath = txtSoundPath.Text.Trim();
    }

    private void SaveSettings()
    {
        if (_loadingSettings) return;
        PlayerConfig.Set(ConfigKeyScripts, string.Join(";", AppService.Bot.ScriptPaths));
        PlayerConfig.Set(ConfigKeyLoops, (int)nudLoops.Value);
        PlayerConfig.Set(ConfigKeyDelay, (int)nudDelay.Value);
        PlayerConfig.Set(ConfigKeySound, txtSoundPath.Text.Trim());
        PlayerConfig.Set(ConfigKeyScript1, txtScript1Path.Text.Trim());
        PlayerConfig.Set(ConfigKeyScript2, txtScript2Path.Text.Trim());
        PlayerConfig.Set(ConfigKeyScript3, txtScript3Path.Text.Trim());
        PlayerConfig.Save();
        ApplyOptions();
    }

    private void nudOptions_ValueChanged(object sender, EventArgs e) => SaveSettings();

    private void txtSoundPath_TextChanged(object sender, EventArgs e)
    {
        AppService.Bot.SoundFilePath = txtSoundPath.Text.Trim();
        SaveSettings();
    }

    private void btnBrowseSound_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Select completion sound",
            Filter = "WAV files (*.wav)|*.wav|All Files (*.*)|*.*",
            InitialDirectory = Path.GetDirectoryName(txtSoundPath.Text) ?? @"C:\Windows\Media",
        };

        if (!string.IsNullOrWhiteSpace(txtSoundPath.Text) && File.Exists(txtSoundPath.Text))
            dlg.FileName = txtSoundPath.Text;

        if (dlg.ShowDialog() != DialogResult.OK) return;
        txtSoundPath.Text = dlg.FileName;
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        if (AppService.Bot.ScriptPaths.Count == 0)
        {
            MessageBox.Show("Add at least one script to the list.", "Scripts",
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

    // ── Script slot handlers ──────────────────────────────────────────────────

    private void BrowseScriptSlot(System.Windows.Forms.TextBox txt)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Select Script",
            Filter = "RSBot Script (*.rbs)|*.rbs|All Files (*.*)|*.*",
            InitialDirectory = string.IsNullOrWhiteSpace(txt.Text)
                ? Path.Combine(Kernel.BasePath, "Data", "Scripts")
                : (Path.GetDirectoryName(txt.Text) ?? Path.Combine(Kernel.BasePath, "Data", "Scripts")),
        };

        if (!string.IsNullOrWhiteSpace(txt.Text) && File.Exists(txt.Text))
            dlg.FileName = txt.Text;

        if (dlg.ShowDialog() != DialogResult.OK) return;
        txt.Text = dlg.FileName;
        SaveSettings();
    }

    private void PlayScriptSlot(AutoScriptsService svc, string path, int slot)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBox.Show($"Set a script path for Script {slot} first.", "Scripts",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        svc.ScriptPaths.Clear();
        svc.ScriptPaths.Add(path);
        ApplyOptionsToSlot(svc);
        svc.Start();
        UpdateSlotButtons(slot, true);
    }

    private void btnScript1Browse_Click(object sender, EventArgs e) => BrowseScriptSlot(txtScript1Path);
    private void btnScript2Browse_Click(object sender, EventArgs e) => BrowseScriptSlot(txtScript2Path);
    private void btnScript3Browse_Click(object sender, EventArgs e) => BrowseScriptSlot(txtScript3Path);

    private void btnScript1Play_Click(object sender, EventArgs e)
        => PlayScriptSlot(AppService.Script1, txtScript1Path.Text.Trim(), 1);

    private void btnScript2Play_Click(object sender, EventArgs e)
        => PlayScriptSlot(AppService.Script2, txtScript2Path.Text.Trim(), 2);

    private void btnScript3Play_Click(object sender, EventArgs e)
        => PlayScriptSlot(AppService.Script3, txtScript3Path.Text.Trim(), 3);

    private void btnScript1Stop_Click(object sender, EventArgs e)
    {
        AppService.Script1.Stop();
        UpdateSlotButtons(1, false);
    }

    private void btnScript2Stop_Click(object sender, EventArgs e)
    {
        AppService.Script2.Stop();
        UpdateSlotButtons(2, false);
    }

    private void btnScript3Stop_Click(object sender, EventArgs e)
    {
        AppService.Script3.Stop();
        UpdateSlotButtons(3, false);
    }

    private void txtScriptPath_TextChanged(object sender, EventArgs e) => SaveSettings();

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
}
