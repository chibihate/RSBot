namespace RSBot.Views;

partial class PlayerFollowWindow
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelTop      = new SDUI.Controls.Panel();
        chkEnabled    = new System.Windows.Forms.CheckBox();
        lblStatus     = new SDUI.Controls.Label();
        panelBody     = new SDUI.Controls.Panel();
        lblTarget     = new SDUI.Controls.Label();
        comboTarget   = new SDUI.Controls.ComboBox();
        btnRefresh    = new SDUI.Controls.Button();
        lblDistance   = new SDUI.Controls.Label();
        nudDistance   = new System.Windows.Forms.NumericUpDown();
        lblUnit       = new SDUI.Controls.Label();

        panelTop.SuspendLayout();
        panelBody.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudDistance).BeginInit();
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
        panelTop.Size        = new System.Drawing.Size(378, 40);
        panelTop.TabIndex    = 0;
        panelTop.Controls.Add(chkEnabled);
        panelTop.Controls.Add(lblStatus);

        // ── chkEnabled ───────────────────────────────────────────────────────
        chkEnabled.Text     = "Enable follow";
        chkEnabled.Location = new System.Drawing.Point(12, 11);
        chkEnabled.Size     = new System.Drawing.Size(110, 19);
        chkEnabled.TabIndex = 0;
        chkEnabled.CheckedChanged += chkEnabled_CheckedChanged;

        // ── lblStatus ────────────────────────────────────────────────────────
        lblStatus.ApplyGradient = false;
        lblStatus.AutoSize      = true;
        lblStatus.Text          = "Idle";
        lblStatus.Location      = new System.Drawing.Point(240, 13);
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
        panelBody.Controls.Add(lblTarget);
        panelBody.Controls.Add(comboTarget);
        panelBody.Controls.Add(btnRefresh);
        panelBody.Controls.Add(lblDistance);
        panelBody.Controls.Add(nudDistance);
        panelBody.Controls.Add(lblUnit);

        // ── lblTarget ────────────────────────────────────────────────────────
        lblTarget.ApplyGradient = false;
        lblTarget.AutoSize      = true;
        lblTarget.Text          = "Target player:";
        lblTarget.Location      = new System.Drawing.Point(12, 14);
        lblTarget.Name          = "lblTarget";
        lblTarget.TabIndex      = 0;

        // ── comboTarget ──────────────────────────────────────────────────────
        comboTarget.DrawMode        = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        comboTarget.DropDownStyle   = System.Windows.Forms.ComboBoxStyle.DropDown;  // editable
        comboTarget.DropDownHeight  = 120;
        comboTarget.FormattingEnabled = true;
        comboTarget.IntegralHeight  = false;
        comboTarget.ItemHeight       = 17;
        comboTarget.Location         = new System.Drawing.Point(110, 10);
        comboTarget.Name             = "comboTarget";
        comboTarget.Radius           = 5;
        comboTarget.ShadowDepth      = 4F;
        comboTarget.Size             = new System.Drawing.Size(210, 23);
        comboTarget.TabIndex         = 1;
        comboTarget.TextChanged     += comboTarget_TextChanged;

        // ── btnRefresh ───────────────────────────────────────────────────────
        btnRefresh.Text             = "↺";
        btnRefresh.Color            = System.Drawing.Color.FromArgb(33, 150, 243);
        btnRefresh.ForeColor        = System.Drawing.Color.White;
        btnRefresh.Location         = new System.Drawing.Point(326, 9);
        btnRefresh.Name             = "btnRefresh";
        btnRefresh.Radius           = 6;
        btnRefresh.ShadowDepth      = 4F;
        btnRefresh.Size             = new System.Drawing.Size(32, 25);
        btnRefresh.TabIndex         = 2;
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click           += btnRefresh_Click;

        // ── lblDistance ──────────────────────────────────────────────────────
        lblDistance.ApplyGradient = false;
        lblDistance.AutoSize      = true;
        lblDistance.Text          = "Follow distance:";
        lblDistance.Location      = new System.Drawing.Point(12, 50);
        lblDistance.Name          = "lblDistance";
        lblDistance.TabIndex      = 3;

        // ── nudDistance ──────────────────────────────────────────────────────
        nudDistance.Location      = new System.Drawing.Point(110, 47);
        nudDistance.Name          = "nudDistance";
        nudDistance.Minimum       = 1;
        nudDistance.Maximum       = 500;
        nudDistance.Value         = 10;
        nudDistance.Size          = new System.Drawing.Size(64, 22);
        nudDistance.TabIndex      = 4;
        nudDistance.ValueChanged += nudDistance_ValueChanged;

        // ── lblUnit ──────────────────────────────────────────────────────────
        lblUnit.ApplyGradient = false;
        lblUnit.AutoSize      = true;
        lblUnit.Text          = "units";
        lblUnit.Location      = new System.Drawing.Point(180, 50);
        lblUnit.Name          = "lblUnit";
        lblUnit.TabIndex      = 5;

        // ── Window ───────────────────────────────────────────────────────────
        AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Dpi;
        BackColor           = System.Drawing.Color.FromArgb(251, 251, 251);
        ClientSize          = new System.Drawing.Size(380, 160);
        ControlBox          = false;
        MaximizeBox         = false;
        Padding             = new System.Windows.Forms.Padding(1, 32, 1, 1);
        ShowInTaskbar       = false;
        Text                = "Player Follow";
        Name                = "PlayerFollowWindow";
        FormClosed         += PlayerFollowWindow_FormClosed;

        Controls.Add(panelBody);
        Controls.Add(panelTop);

        ((System.ComponentModel.ISupportInitialize)nudDistance).EndInit();
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
    private SDUI.Controls.Label lblTarget;
    private SDUI.Controls.ComboBox comboTarget;
    private SDUI.Controls.Button btnRefresh;
    private SDUI.Controls.Label lblDistance;
    private System.Windows.Forms.NumericUpDown nudDistance;
    private SDUI.Controls.Label lblUnit;
}
