namespace RSBot.Views;

partial class AutoSelectWindow
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelTop            = new SDUI.Controls.Panel();
        chkEnabled          = new System.Windows.Forms.CheckBox();
        lblStatus           = new SDUI.Controls.Label();
        panelBody           = new SDUI.Controls.Panel();
        lblJobs             = new SDUI.Controls.Label();
        chkThief            = new System.Windows.Forms.CheckBox();
        chkTrade            = new System.Windows.Forms.CheckBox();
        chkHunter           = new System.Windows.Forms.CheckBox();
        lblRadius           = new SDUI.Controls.Label();
        nudRadius           = new System.Windows.Forms.NumericUpDown();
        lblUnit             = new SDUI.Controls.Label();
        lblFollowTarget     = new SDUI.Controls.Label();
        comboFollowTarget   = new SDUI.Controls.ComboBox();
        btnRefreshFollow    = new SDUI.Controls.Button();
        lblHotkey           = new SDUI.Controls.Label();
        btnHotkey           = new SDUI.Controls.Button();

        panelTop.SuspendLayout();
        panelBody.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudRadius).BeginInit();
        SuspendLayout();

        // ── panelTop ─────────────────────────────────────────────────────────
        panelTop.BackColor   = System.Drawing.Color.Transparent;
        panelTop.Border      = new System.Windows.Forms.Padding(0, 0, 0, 1);
        panelTop.BorderColor = System.Drawing.Color.Transparent;
        panelTop.Dock        = System.Windows.Forms.DockStyle.Top;
        panelTop.Location    = new System.Drawing.Point(1, 32);
        panelTop.Name        = "panelTop";
        panelTop.Radius      = 0;
        panelTop.ShadowDepth = 4F;
        panelTop.Size        = new System.Drawing.Size(338, 40);
        panelTop.TabIndex    = 0;
        panelTop.Controls.Add(chkEnabled);
        panelTop.Controls.Add(lblStatus);

        // ── chkEnabled ───────────────────────────────────────────────────────
        chkEnabled.Text     = "Enable auto-select";
        chkEnabled.Location = new System.Drawing.Point(12, 11);
        chkEnabled.Size     = new System.Drawing.Size(130, 19);
        chkEnabled.TabIndex = 0;
        chkEnabled.CheckedChanged += chkEnabled_CheckedChanged;

        // ── lblStatus ────────────────────────────────────────────────────────
        lblStatus.ApplyGradient = false;
        lblStatus.AutoSize      = true;
        lblStatus.Text          = "Idle";
        lblStatus.Location      = new System.Drawing.Point(250, 13);
        lblStatus.Name          = "lblStatus";
        lblStatus.TabIndex      = 1;

        // ── panelBody ────────────────────────────────────────────────────────
        panelBody.BackColor   = System.Drawing.Color.Transparent;
        panelBody.Border      = new System.Windows.Forms.Padding(0);
        panelBody.BorderColor = System.Drawing.Color.Transparent;
        panelBody.Dock        = System.Windows.Forms.DockStyle.Fill;
        panelBody.Location    = new System.Drawing.Point(1, 72);
        panelBody.Name        = "panelBody";
        panelBody.Radius      = 0;
        panelBody.ShadowDepth = 4F;
        panelBody.TabIndex    = 1;
        panelBody.Controls.Add(lblJobs);
        panelBody.Controls.Add(chkThief);
        panelBody.Controls.Add(chkTrade);
        panelBody.Controls.Add(chkHunter);
        panelBody.Controls.Add(lblRadius);
        panelBody.Controls.Add(nudRadius);
        panelBody.Controls.Add(lblUnit);
        panelBody.Controls.Add(lblFollowTarget);
        panelBody.Controls.Add(comboFollowTarget);
        panelBody.Controls.Add(btnRefreshFollow);
        panelBody.Controls.Add(lblHotkey);
        panelBody.Controls.Add(btnHotkey);

        // ── lblJobs ──────────────────────────────────────────────────────────
        lblJobs.ApplyGradient = false;
        lblJobs.AutoSize      = true;
        lblJobs.Text          = "Job types:";
        lblJobs.Location      = new System.Drawing.Point(12, 14);
        lblJobs.Name          = "lblJobs";
        lblJobs.TabIndex      = 0;

        // ── chkThief ─────────────────────────────────────────────────────────
        chkThief.Text     = "Thief";
        chkThief.Checked  = true;
        chkThief.Location = new System.Drawing.Point(12, 38);
        chkThief.Size     = new System.Drawing.Size(65, 19);
        chkThief.TabIndex = 1;

        // ── chkTrade ─────────────────────────────────────────────────────────
        chkTrade.Text     = "Trade";
        chkTrade.Location = new System.Drawing.Point(88, 38);
        chkTrade.Size     = new System.Drawing.Size(65, 19);
        chkTrade.TabIndex = 2;

        // ── chkHunter ────────────────────────────────────────────────────────
        chkHunter.Text     = "Hunter";
        chkHunter.Location = new System.Drawing.Point(164, 38);
        chkHunter.Size     = new System.Drawing.Size(70, 19);
        chkHunter.TabIndex = 3;

        // ── lblRadius ────────────────────────────────────────────────────────
        lblRadius.ApplyGradient = false;
        lblRadius.AutoSize      = true;
        lblRadius.Text          = "Radius:";
        lblRadius.Location      = new System.Drawing.Point(12, 72);
        lblRadius.Name          = "lblRadius";
        lblRadius.TabIndex      = 4;

        // ── nudRadius ────────────────────────────────────────────────────────
        nudRadius.Location  = new System.Drawing.Point(70, 69);
        nudRadius.Name      = "nudRadius";
        nudRadius.Minimum   = 1;
        nudRadius.Maximum   = 1000;
        nudRadius.Value     = 100;
        nudRadius.Size      = new System.Drawing.Size(70, 22);
        nudRadius.TabIndex  = 5;

        // ── lblUnit ──────────────────────────────────────────────────────────
        lblUnit.ApplyGradient = false;
        lblUnit.AutoSize      = true;
        lblUnit.Text          = "units";
        lblUnit.Location      = new System.Drawing.Point(146, 72);
        lblUnit.Name          = "lblUnit";
        lblUnit.TabIndex      = 6;

        // ── lblFollowTarget ──────────────────────────────────────────────────
        lblFollowTarget.ApplyGradient = false;
        lblFollowTarget.AutoSize      = true;
        lblFollowTarget.Text          = "Follow target of:";
        lblFollowTarget.Location      = new System.Drawing.Point(12, 104);
        lblFollowTarget.Name          = "lblFollowTarget";
        lblFollowTarget.TabIndex      = 7;

        // ── comboFollowTarget ────────────────────────────────────────────────
        comboFollowTarget.DrawMode          = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        comboFollowTarget.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDown;
        comboFollowTarget.DropDownHeight    = 120;
        comboFollowTarget.FormattingEnabled = true;
        comboFollowTarget.IntegralHeight    = false;
        comboFollowTarget.ItemHeight        = 17;
        comboFollowTarget.Location          = new System.Drawing.Point(115, 100);
        comboFollowTarget.Name              = "comboFollowTarget";
        comboFollowTarget.Radius            = 5;
        comboFollowTarget.ShadowDepth       = 4F;
        comboFollowTarget.Size              = new System.Drawing.Size(175, 23);
        comboFollowTarget.TabIndex          = 8;

        // ── btnRefreshFollow ─────────────────────────────────────────────────
        btnRefreshFollow.Text             = "↺";
        btnRefreshFollow.Color            = System.Drawing.Color.FromArgb(33, 150, 243);
        btnRefreshFollow.ForeColor        = System.Drawing.Color.White;
        btnRefreshFollow.Location         = new System.Drawing.Point(296, 99);
        btnRefreshFollow.Name             = "btnRefreshFollow";
        btnRefreshFollow.Radius           = 6;
        btnRefreshFollow.ShadowDepth      = 4F;
        btnRefreshFollow.Size             = new System.Drawing.Size(32, 25);
        btnRefreshFollow.TabIndex         = 9;
        btnRefreshFollow.UseVisualStyleBackColor = false;
        btnRefreshFollow.Click           += btnRefreshFollow_Click;

        // ── lblHotkey ────────────────────────────────────────────────────────
        lblHotkey.ApplyGradient = false;
        lblHotkey.AutoSize      = true;
        lblHotkey.Text          = "Hotkey:";
        lblHotkey.Location      = new System.Drawing.Point(12, 140);
        lblHotkey.Name          = "lblHotkey";
        lblHotkey.TabIndex      = 10;

        // ── btnHotkey ────────────────────────────────────────────────────────
        btnHotkey.Text        = "Space";
        btnHotkey.Location    = new System.Drawing.Point(70, 135);
        btnHotkey.Name        = "btnHotkey";
        btnHotkey.Radius      = 6;
        btnHotkey.ShadowDepth = 4F;
        btnHotkey.Size        = new System.Drawing.Size(120, 28);
        btnHotkey.TabIndex    = 11;
        btnHotkey.UseVisualStyleBackColor = false;
        btnHotkey.Click      += btnHotkey_Click;

        // ── Window ───────────────────────────────────────────────────────────
        AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Dpi;
        BackColor           = System.Drawing.Color.FromArgb(251, 251, 251);
        ClientSize          = new System.Drawing.Size(340, 210);
        ControlBox          = false;
        KeyPreview          = true;
        MaximizeBox         = false;
        Padding             = new System.Windows.Forms.Padding(1, 32, 1, 1);
        ShowInTaskbar       = false;
        Text                = "Auto Select";
        Name                = "AutoSelectWindow";
        FormClosed         += AutoSelectWindow_FormClosed;
        KeyDown            += AutoSelectWindow_KeyDown;

        Controls.Add(panelBody);
        Controls.Add(panelTop);

        ((System.ComponentModel.ISupportInitialize)nudRadius).EndInit();
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        panelBody.ResumeLayout(false);
        panelBody.PerformLayout();
        ResumeLayout(false);
    }

    private SDUI.Controls.Panel panelTop;
    private System.Windows.Forms.CheckBox chkEnabled;
    private SDUI.Controls.Label lblStatus;
    private SDUI.Controls.Panel panelBody;
    private SDUI.Controls.Label lblJobs;
    private System.Windows.Forms.CheckBox chkThief;
    private System.Windows.Forms.CheckBox chkTrade;
    private System.Windows.Forms.CheckBox chkHunter;
    private SDUI.Controls.Label lblRadius;
    private System.Windows.Forms.NumericUpDown nudRadius;
    private SDUI.Controls.Label lblUnit;
    private SDUI.Controls.Label lblFollowTarget;
    private SDUI.Controls.ComboBox comboFollowTarget;
    private SDUI.Controls.Button btnRefreshFollow;
    private SDUI.Controls.Label lblHotkey;
    private SDUI.Controls.Button btnHotkey;
}
