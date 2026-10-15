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
        panelTop               = new SDUI.Controls.Panel();
        chkEnabled             = new System.Windows.Forms.CheckBox();
        lblStatus              = new SDUI.Controls.Label();
        panelBody              = new SDUI.Controls.Panel();
        lblJobs                = new SDUI.Controls.Label();
        chkThief               = new System.Windows.Forms.CheckBox();
        chkTrade               = new System.Windows.Forms.CheckBox();
        chkHunter              = new System.Windows.Forms.CheckBox();
        lblGuilds              = new SDUI.Controls.Label();
        lstGuilds              = new System.Windows.Forms.CheckedListBox();
        btnRefreshGuilds       = new SDUI.Controls.Button();
        lblRadius              = new SDUI.Controls.Label();
        nudRadius              = new System.Windows.Forms.NumericUpDown();
        lblUnit                = new SDUI.Controls.Label();
        chkFollowTarget        = new System.Windows.Forms.CheckBox();
        comboFollowTarget      = new SDUI.Controls.ComboBox();
        btnRefreshFollow       = new SDUI.Controls.Button();
        chkPriorityBadEffect   = new System.Windows.Forms.CheckBox();
        comboBadEffect         = new SDUI.Controls.ComboBox();
        chkKeepFocus           = new System.Windows.Forms.CheckBox();
        lblHotkey              = new SDUI.Controls.Label();
        btnHotkey              = new SDUI.Controls.Button();

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
        panelBody.Controls.Add(lblGuilds);
        panelBody.Controls.Add(lstGuilds);
        panelBody.Controls.Add(btnRefreshGuilds);
        panelBody.Controls.Add(lblRadius);
        panelBody.Controls.Add(nudRadius);
        panelBody.Controls.Add(lblUnit);
        panelBody.Controls.Add(chkFollowTarget);
        panelBody.Controls.Add(comboFollowTarget);
        panelBody.Controls.Add(btnRefreshFollow);
        panelBody.Controls.Add(chkPriorityBadEffect);
        panelBody.Controls.Add(comboBadEffect);
        panelBody.Controls.Add(chkKeepFocus);
        panelBody.Controls.Add(lblHotkey);
        panelBody.Controls.Add(btnHotkey);

        // ── lblJobs ──────────────────────────────────────────────────────────
        lblJobs.ApplyGradient = false;
        lblJobs.AutoSize      = true;
        lblJobs.Text          = "Target types:";
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
        chkHunter.Size     = new System.Drawing.Size(65, 19);
        chkHunter.TabIndex = 3;

        // ── lblGuilds ────────────────────────────────────────────────────────
        lblGuilds.ApplyGradient = false;
        lblGuilds.AutoSize      = true;
        lblGuilds.Text          = "Target guilds:";
        lblGuilds.Location      = new System.Drawing.Point(12, 68);
        lblGuilds.Name          = "lblGuilds";
        lblGuilds.TabIndex      = 4;

        // ── lstGuilds ────────────────────────────────────────────────────────
        lstGuilds.Location      = new System.Drawing.Point(12, 86);
        lstGuilds.Name          = "lstGuilds";
        lstGuilds.Size          = new System.Drawing.Size(282, 76);
        lstGuilds.TabIndex      = 5;
        lstGuilds.CheckOnClick  = true;

        // ── btnRefreshGuilds ─────────────────────────────────────────────────
        btnRefreshGuilds.Text             = "↺";
        btnRefreshGuilds.Color            = System.Drawing.Color.FromArgb(33, 150, 243);
        btnRefreshGuilds.ForeColor        = System.Drawing.Color.White;
        btnRefreshGuilds.Location         = new System.Drawing.Point(300, 85);
        btnRefreshGuilds.Name             = "btnRefreshGuilds";
        btnRefreshGuilds.Radius           = 6;
        btnRefreshGuilds.ShadowDepth      = 4F;
        btnRefreshGuilds.Size             = new System.Drawing.Size(32, 25);
        btnRefreshGuilds.TabIndex         = 6;
        btnRefreshGuilds.UseVisualStyleBackColor = false;
        btnRefreshGuilds.Click           += btnRefreshGuilds_Click;

        // ── lblRadius ────────────────────────────────────────────────────────
        lblRadius.ApplyGradient = false;
        lblRadius.AutoSize      = true;
        lblRadius.Text          = "Radius:";
        lblRadius.Location      = new System.Drawing.Point(12, 174);
        lblRadius.Name          = "lblRadius";
        lblRadius.TabIndex      = 7;

        // ── nudRadius ────────────────────────────────────────────────────────
        nudRadius.Location  = new System.Drawing.Point(70, 171);
        nudRadius.Name      = "nudRadius";
        nudRadius.Minimum   = 1;
        nudRadius.Maximum   = 1000;
        nudRadius.Value     = 20;
        nudRadius.Size      = new System.Drawing.Size(70, 22);
        nudRadius.TabIndex  = 8;

        // ── lblUnit ──────────────────────────────────────────────────────────
        lblUnit.ApplyGradient = false;
        lblUnit.AutoSize      = true;
        lblUnit.Text          = "units";
        lblUnit.Location      = new System.Drawing.Point(146, 174);
        lblUnit.Name          = "lblUnit";
        lblUnit.TabIndex      = 9;

        // ── chkFollowTarget ──────────────────────────────────────────────────
        chkFollowTarget.Text     = "Follow target of:";
        chkFollowTarget.Location = new System.Drawing.Point(12, 206);
        chkFollowTarget.Size     = new System.Drawing.Size(120, 19);
        chkFollowTarget.TabIndex = 10;
        chkFollowTarget.CheckedChanged += chkFollowTarget_CheckedChanged;

        // ── comboFollowTarget ────────────────────────────────────────────────
        comboFollowTarget.DrawMode          = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        comboFollowTarget.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDown;
        comboFollowTarget.DropDownHeight    = 120;
        comboFollowTarget.Enabled           = false;
        comboFollowTarget.FormattingEnabled = true;
        comboFollowTarget.IntegralHeight    = false;
        comboFollowTarget.ItemHeight        = 17;
        comboFollowTarget.Location          = new System.Drawing.Point(138, 202);
        comboFollowTarget.Name              = "comboFollowTarget";
        comboFollowTarget.Radius            = 5;
        comboFollowTarget.ShadowDepth       = 4F;
        comboFollowTarget.Size              = new System.Drawing.Size(152, 23);
        comboFollowTarget.TabIndex          = 11;

        // ── btnRefreshFollow ─────────────────────────────────────────────────
        btnRefreshFollow.Text             = "↺";
        btnRefreshFollow.Color            = System.Drawing.Color.FromArgb(33, 150, 243);
        btnRefreshFollow.Enabled          = false;
        btnRefreshFollow.ForeColor        = System.Drawing.Color.White;
        btnRefreshFollow.Location         = new System.Drawing.Point(296, 201);
        btnRefreshFollow.Name             = "btnRefreshFollow";
        btnRefreshFollow.Radius           = 6;
        btnRefreshFollow.ShadowDepth      = 4F;
        btnRefreshFollow.Size             = new System.Drawing.Size(32, 25);
        btnRefreshFollow.TabIndex         = 12;
        btnRefreshFollow.UseVisualStyleBackColor = false;
        btnRefreshFollow.Click           += btnRefreshFollow_Click;

        // ── chkPriorityBadEffect ─────────────────────────────────────────────
        chkPriorityBadEffect.Text     = "Priority bad effect:";
        chkPriorityBadEffect.Location = new System.Drawing.Point(12, 240);
        chkPriorityBadEffect.Size     = new System.Drawing.Size(130, 19);
        chkPriorityBadEffect.TabIndex = 13;
        chkPriorityBadEffect.CheckedChanged += chkPriorityBadEffect_CheckedChanged;

        // ── comboBadEffect ───────────────────────────────────────────────────
        comboBadEffect.DrawMode          = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        comboBadEffect.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboBadEffect.DropDownHeight    = 120;
        comboBadEffect.Enabled           = false;
        comboBadEffect.FormattingEnabled = true;
        comboBadEffect.IntegralHeight    = false;
        comboBadEffect.ItemHeight        = 17;
        comboBadEffect.Location          = new System.Drawing.Point(148, 236);
        comboBadEffect.Name              = "comboBadEffect";
        comboBadEffect.Radius            = 5;
        comboBadEffect.ShadowDepth       = 4F;
        comboBadEffect.Size              = new System.Drawing.Size(142, 23);
        comboBadEffect.TabIndex          = 14;

        // ── chkKeepFocus ─────────────────────────────────────────────────────
        chkKeepFocus.Text     = "Keep focus if bad effect active";
        chkKeepFocus.Location = new System.Drawing.Point(12, 265);
        chkKeepFocus.Size     = new System.Drawing.Size(210, 19);
        chkKeepFocus.TabIndex = 15;

        // ── lblHotkey ────────────────────────────────────────────────────────
        lblHotkey.ApplyGradient = false;
        lblHotkey.AutoSize      = true;
        lblHotkey.Text          = "Hotkey:";
        lblHotkey.Location      = new System.Drawing.Point(12, 303);
        lblHotkey.Name          = "lblHotkey";
        lblHotkey.TabIndex      = 16;

        // ── btnHotkey ────────────────────────────────────────────────────────
        btnHotkey.Text        = "Space";
        btnHotkey.Location    = new System.Drawing.Point(70, 298);
        btnHotkey.Name        = "btnHotkey";
        btnHotkey.Radius      = 6;
        btnHotkey.ShadowDepth = 4F;
        btnHotkey.Size        = new System.Drawing.Size(120, 28);
        btnHotkey.TabIndex    = 17;
        btnHotkey.UseVisualStyleBackColor = false;
        btnHotkey.Click      += btnHotkey_Click;

        // ── Window ───────────────────────────────────────────────────────────
        AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Dpi;
        BackColor           = System.Drawing.Color.FromArgb(251, 251, 251);
        ClientSize          = new System.Drawing.Size(340, 375);
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
    private SDUI.Controls.Label lblGuilds;
    private System.Windows.Forms.CheckedListBox lstGuilds;
    private SDUI.Controls.Button btnRefreshGuilds;
    private SDUI.Controls.Label lblRadius;
    private System.Windows.Forms.NumericUpDown nudRadius;
    private SDUI.Controls.Label lblUnit;
    private System.Windows.Forms.CheckBox chkFollowTarget;
    private SDUI.Controls.ComboBox comboFollowTarget;
    private SDUI.Controls.Button btnRefreshFollow;
    private System.Windows.Forms.CheckBox chkPriorityBadEffect;
    private SDUI.Controls.ComboBox comboBadEffect;
    private System.Windows.Forms.CheckBox chkKeepFocus;
    private SDUI.Controls.Label lblHotkey;
    private SDUI.Controls.Button btnHotkey;
}
