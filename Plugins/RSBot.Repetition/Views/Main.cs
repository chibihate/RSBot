using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RSBot.Repetition;
using RSBot.Repetition.Models;
using RSBot.Core;
using RSBot.Core.Event;
using SDUI.Controls;

namespace RSBot.Repetition.Views;

[ToolboxItem(false)]
public partial class Main : DoubleBufferedControl
{
    private const string ConfigKeyAccounts = "RSBot.Controller.Accounts";
    private const string ConfigKeyDelay    = "RSBot.Controller.DelaySeconds";
    private const string ConfigKeyScripts  = "RSBot.Controller.Scripts";

    public Main()
    {
        CheckForIllegalCrossThreadCalls = false;
        InitializeComponent();
        SubscribeEvents();
        LoadSettings();
    }

    private void SubscribeEvents()
    {
        EventManager.SubscribeEvent("OnControllerRefresh", OnControllerRefresh);
    }

    private void OnControllerRefresh()
    {
        if (IsDisposed || Disposing) return;
        if (InvokeRequired) BeginInvoke(RefreshGrid);
        else RefreshGrid();
    }

    public void LoadSettings()
    {
        nudDelay.Value = GlobalConfig.Get(ConfigKeyDelay, 10);
        AppService.Controller.DelaySeconds = (int)nudDelay.Value;

        var rawAccounts = GlobalConfig.Get(ConfigKeyAccounts, string.Empty);
        AppService.Controller.Accounts.Clear();
        dgvAccounts.Rows.Clear();

        if (!string.IsNullOrWhiteSpace(rawAccounts))
        {
            foreach (var username in rawAccounts.Split(';').Where(u => !string.IsNullOrWhiteSpace(u)))
            {
                var entry = new AccountEntry { Username = username };
                AppService.Controller.Accounts.Add(entry);
                dgvAccounts.Rows.Add(entry.Username, entry.StatusText);
            }
        }
        else
        {
            foreach (var username in ControllerService.GetAvailableAccounts())
            {
                var entry = new AccountEntry { Username = username };
                AppService.Controller.Accounts.Add(entry);
                dgvAccounts.Rows.Add(entry.Username, entry.StatusText);
            }
        }

        var rawScripts = GlobalConfig.Get(ConfigKeyScripts, string.Empty);
        AppService.Controller.ScriptPaths.Clear();
        lstScripts.Items.Clear();

        foreach (var path in rawScripts.Split(';').Where(p => !string.IsNullOrWhiteSpace(p)))
        {
            AppService.Controller.ScriptPaths.Add(path);
            lstScripts.Items.Add(Path.GetFileName(path));
        }

        UpdateButtons();
    }

    private void SaveSettings()
    {
        GlobalConfig.Set(ConfigKeyAccounts,
            string.Join(";", AppService.Controller.Accounts.Select(a => a.Username)));
        GlobalConfig.Set(ConfigKeyScripts,
            string.Join(";", AppService.Controller.ScriptPaths));
        GlobalConfig.Set(ConfigKeyDelay, (int)nudDelay.Value);
        GlobalConfig.Save();
    }

    private void RefreshGrid()
    {
        var accounts = AppService.Controller.Accounts;
        for (var i = 0; i < dgvAccounts.Rows.Count && i < accounts.Count; i++)
        {
            dgvAccounts.Rows[i].Cells[1].Value = accounts[i].StatusText;
            dgvAccounts.Rows[i].DefaultCellStyle.BackColor =
                accounts[i].Status == AccountStatus.Running
                    ? System.Drawing.Color.LightYellow
                    : System.Drawing.Color.Empty;
        }

        var running = AppService.Controller.IsRunning;
        btnStart.Text = running ? "Stop" : "Start";
        lblStatus.Text = running
            ? $"Running account {AppService.Controller.CurrentIndex + 1}/{AppService.Controller.Accounts.Count}"
            : "Status: Idle";
    }

    private void UpdateButtons()
    {
        var running = AppService.Controller.IsRunning;
        btnStart.Text           = running ? "Stop" : "Start";
        btnAdd.Enabled          = !running;
        btnRemove.Enabled       = !running;
        btnUp.Enabled           = !running;
        btnDown.Enabled         = !running;
        btnReset.Enabled        = !running;
        btnReload.Enabled       = !running;
        btnClear.Enabled        = !running;
        btnScriptAdd.Enabled    = !running;
        btnScriptRemove.Enabled = !running;
        btnScriptUp.Enabled     = !running;
        btnScriptDown.Enabled   = !running;
        nudDelay.Enabled        = !running;
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        if (AppService.Controller.IsRunning)
        {
            AppService.Controller.Stop();
            btnStart.Text = "Start";
        }
        else
        {
            if (AppService.Controller.Accounts.Count == 0)
            {
                MessageBox.Show("Add at least one account to the queue.", "Repetition",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadSettings();
            AppService.Controller.Start();
            btnStart.Text = "Stop";
        }

        UpdateButtons();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        var existing = new HashSet<string>(
            AppService.Controller.Accounts.Select(a => a.Username),
            StringComparer.OrdinalIgnoreCase);
        var available = ControllerService.GetAvailableAccounts()
            .Where(u => !existing.Contains(u))
            .ToList();

        string username;
        if (available.Count > 0)
        {
            using var dlg = new SelectAccountDialog(available);
            if (dlg.ShowDialog() != DialogResult.OK) return;
            username = dlg.SelectedUsername;
        }
        else
        {
            using var dlg = new InputDialog("Add Account", "Enter account username:");
            if (dlg.ShowDialog() != DialogResult.OK) return;
            username = dlg.Value;
        }

        if (string.IsNullOrWhiteSpace(username)) return;

        var entry = new AccountEntry { Username = username };
        AppService.Controller.Accounts.Add(entry);
        dgvAccounts.Rows.Add(entry.Username, entry.StatusText);
        SaveSettings();
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
        var idx = dgvAccounts.CurrentRow?.Index ?? -1;
        if (idx < 0 || idx >= AppService.Controller.Accounts.Count) return;

        AppService.Controller.Accounts.RemoveAt(idx);
        dgvAccounts.Rows.RemoveAt(idx);
        SaveSettings();
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
        var idx = dgvAccounts.CurrentRow?.Index ?? -1;
        if (idx <= 0) return;
        SwapRows(idx, idx - 1);
        dgvAccounts.Rows[idx - 1].Selected = true;
        SaveSettings();
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
        var idx = dgvAccounts.CurrentRow?.Index ?? -1;
        if (idx < 0 || idx >= dgvAccounts.Rows.Count - 1) return;
        SwapRows(idx, idx + 1);
        dgvAccounts.Rows[idx + 1].Selected = true;
        SaveSettings();
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
        foreach (var a in AppService.Controller.Accounts)
            a.Status = AccountStatus.Pending;
        RefreshGrid();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        AppService.Controller.Accounts.Clear();
        dgvAccounts.Rows.Clear();
        SaveSettings();
    }

    private void btnReload_Click(object sender, EventArgs e)
    {
        var accounts = ControllerService.GetAvailableAccounts().ToList();
        if (accounts.Count == 0)
        {
            MessageBox.Show("No accounts found in RSBot.General.", "Reload Accounts",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AppService.Controller.Accounts.Clear();
        dgvAccounts.Rows.Clear();

        foreach (var username in accounts)
        {
            var entry = new AccountEntry { Username = username };
            AppService.Controller.Accounts.Add(entry);
            dgvAccounts.Rows.Add(entry.Username, entry.StatusText);
        }

        SaveSettings();
    }

    private void SwapRows(int a, int b)
    {
        (AppService.Controller.Accounts[a], AppService.Controller.Accounts[b]) =
            (AppService.Controller.Accounts[b], AppService.Controller.Accounts[a]);

        var usernameA = dgvAccounts.Rows[a].Cells[0].Value;
        var statusA   = dgvAccounts.Rows[a].Cells[1].Value;
        dgvAccounts.Rows[a].Cells[0].Value = dgvAccounts.Rows[b].Cells[0].Value;
        dgvAccounts.Rows[a].Cells[1].Value = dgvAccounts.Rows[b].Cells[1].Value;
        dgvAccounts.Rows[b].Cells[0].Value = usernameA;
        dgvAccounts.Rows[b].Cells[1].Value = statusA;
    }

    private void btnScriptAdd_Click(object sender, EventArgs e)
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
            AppService.Controller.ScriptPaths.Add(file);
            lstScripts.Items.Add(Path.GetFileName(file));
        }

        SaveSettings();
    }

    private void btnScriptRemove_Click(object sender, EventArgs e)
    {
        var idx = lstScripts.SelectedIndex;
        if (idx < 0) return;

        AppService.Controller.ScriptPaths.RemoveAt(idx);
        lstScripts.Items.RemoveAt(idx);

        if (lstScripts.Items.Count > 0)
            lstScripts.SelectedIndex = Math.Min(idx, lstScripts.Items.Count - 1);

        SaveSettings();
    }

    private void btnScriptUp_Click(object sender, EventArgs e)
    {
        var idx = lstScripts.SelectedIndex;
        if (idx <= 0) return;
        SwapScripts(idx, idx - 1);
        lstScripts.SelectedIndex = idx - 1;
        SaveSettings();
    }

    private void btnScriptDown_Click(object sender, EventArgs e)
    {
        var idx = lstScripts.SelectedIndex;
        if (idx < 0 || idx >= lstScripts.Items.Count - 1) return;
        SwapScripts(idx, idx + 1);
        lstScripts.SelectedIndex = idx + 1;
        SaveSettings();
    }

    private void SwapScripts(int a, int b)
    {
        (AppService.Controller.ScriptPaths[a], AppService.Controller.ScriptPaths[b]) =
            (AppService.Controller.ScriptPaths[b], AppService.Controller.ScriptPaths[a]);

        var tmp = lstScripts.Items[a];
        lstScripts.Items[a] = lstScripts.Items[b];
        lstScripts.Items[b] = tmp;
    }

    private void lstScripts_DoubleClick(object sender, EventArgs e)
    {
        var idx = lstScripts.SelectedIndex;
        if (idx < 0 || idx >= AppService.Controller.ScriptPaths.Count) return;

        using var dlg = new OpenFileDialog
        {
            Title = "Replace Script",
            Filter = "RSBot Script (*.rbs)|*.rbs|All Files (*.*)|*.*",
            InitialDirectory = Path.GetDirectoryName(AppService.Controller.ScriptPaths[idx])
                               ?? Path.Combine(Kernel.BasePath, "Data", "Scripts"),
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        AppService.Controller.ScriptPaths[idx] = dlg.FileName;
        lstScripts.Items[idx] = Path.GetFileName(dlg.FileName);
        SaveSettings();
    }

    private void nudDelay_ValueChanged(object sender, EventArgs e)
    {
        AppService.Controller.DelaySeconds = (int)nudDelay.Value;
        SaveSettings();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
}
