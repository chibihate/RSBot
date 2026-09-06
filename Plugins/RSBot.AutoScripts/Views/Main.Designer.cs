using System.Windows.Forms;

namespace RSBot.AutoScripts.Views;

partial class Main
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.GroupBox grpScripts;
    private System.Windows.Forms.ListBox lstScripts;
    private System.Windows.Forms.Panel panelButtons;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Button btnUp;
    private System.Windows.Forms.Button btnDown;
    private System.Windows.Forms.Panel panelOptions;
    private System.Windows.Forms.Label lblLoops;
    private System.Windows.Forms.NumericUpDown nudLoops;
    private System.Windows.Forms.Label lblLoopsHint;
    private System.Windows.Forms.Label lblDelay;
    private System.Windows.Forms.NumericUpDown nudDelay;
    private System.Windows.Forms.Label lblDelayUnit;
    private System.Windows.Forms.Panel panelBottom;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Label lblStatus;

    private void InitializeComponent()
    {
        grpScripts    = new System.Windows.Forms.GroupBox();
        lstScripts    = new System.Windows.Forms.ListBox();
        panelButtons  = new System.Windows.Forms.Panel();
        btnAdd        = new System.Windows.Forms.Button();
        btnRemove     = new System.Windows.Forms.Button();
        btnUp         = new System.Windows.Forms.Button();
        btnDown       = new System.Windows.Forms.Button();
        panelOptions  = new System.Windows.Forms.Panel();
        lblLoops      = new System.Windows.Forms.Label();
        nudLoops      = new System.Windows.Forms.NumericUpDown();
        lblLoopsHint  = new System.Windows.Forms.Label();
        lblDelay      = new System.Windows.Forms.Label();
        nudDelay      = new System.Windows.Forms.NumericUpDown();
        lblDelayUnit  = new System.Windows.Forms.Label();
        panelBottom   = new System.Windows.Forms.Panel();
        btnStart      = new System.Windows.Forms.Button();
        btnStop       = new System.Windows.Forms.Button();
        lblStatus     = new System.Windows.Forms.Label();

        grpScripts.SuspendLayout();
        panelButtons.SuspendLayout();
        panelOptions.SuspendLayout();
        panelBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudLoops).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDelay).BeginInit();
        SuspendLayout();

        // panelButtons -- docked to bottom of GroupBox
        panelButtons.Dock = DockStyle.Bottom;
        panelButtons.Height = 30;
        panelButtons.Padding = new Padding(2, 3, 2, 0);
        panelButtons.Controls.Add(btnAdd);
        panelButtons.Controls.Add(btnRemove);
        panelButtons.Controls.Add(btnUp);
        panelButtons.Controls.Add(btnDown);

        btnAdd.Text = "Add";
        btnAdd.Location = new System.Drawing.Point(2, 3);
        btnAdd.Size = new System.Drawing.Size(60, 24);
        btnAdd.Click += btnAdd_Click;

        btnRemove.Text = "Remove";
        btnRemove.Location = new System.Drawing.Point(66, 3);
        btnRemove.Size = new System.Drawing.Size(62, 24);
        btnRemove.Click += btnRemove_Click;

        btnUp.Text = "▲";
        btnUp.Location = new System.Drawing.Point(134, 3);
        btnUp.Size = new System.Drawing.Size(44, 24);
        btnUp.Click += btnUp_Click;

        btnDown.Text = "▼";
        btnDown.Location = new System.Drawing.Point(182, 3);
        btnDown.Size = new System.Drawing.Size(44, 24);
        btnDown.Click += btnDown_Click;

        // lstScripts
        lstScripts.Dock = DockStyle.Fill;
        lstScripts.SelectionMode = SelectionMode.One;
        lstScripts.DoubleClick += lstScripts_DoubleClick;

        // grpScripts
        grpScripts.Text = "Scripts";
        grpScripts.Dock = DockStyle.Fill;
        grpScripts.Padding = new Padding(4, 4, 4, 4);
        grpScripts.Controls.Add(lstScripts);
        grpScripts.Controls.Add(panelButtons);

        // panelOptions -- Loops / Delay row
        panelOptions.Dock = DockStyle.Bottom;
        panelOptions.Height = 28;

        lblLoops.Text = "Loops:";
        lblLoops.Location = new System.Drawing.Point(4, 6);
        lblLoops.Size = new System.Drawing.Size(44, 18);
        lblLoops.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        nudLoops.Location = new System.Drawing.Point(50, 4);
        nudLoops.Size = new System.Drawing.Size(56, 22);
        nudLoops.Minimum = 0;
        nudLoops.Maximum = 9999;
        nudLoops.Value = 1;
        nudLoops.ValueChanged += nudOptions_ValueChanged;

        lblLoopsHint.Text = "(0=inf)";
        lblLoopsHint.Location = new System.Drawing.Point(110, 6);
        lblLoopsHint.Size = new System.Drawing.Size(48, 18);
        lblLoopsHint.ForeColor = System.Drawing.SystemColors.GrayText;
        lblLoopsHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        lblDelay.Text = "Delay:";
        lblDelay.Location = new System.Drawing.Point(168, 6);
        lblDelay.Size = new System.Drawing.Size(40, 18);
        lblDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        nudDelay.Location = new System.Drawing.Point(210, 4);
        nudDelay.Size = new System.Drawing.Size(64, 22);
        nudDelay.Minimum = 0;
        nudDelay.Maximum = 3600;
        nudDelay.Value = 0;
        nudDelay.ValueChanged += nudOptions_ValueChanged;

        lblDelayUnit.Text = "sec";
        lblDelayUnit.Location = new System.Drawing.Point(278, 6);
        lblDelayUnit.Size = new System.Drawing.Size(30, 18);
        lblDelayUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        panelOptions.Controls.Add(lblLoops);
        panelOptions.Controls.Add(nudLoops);
        panelOptions.Controls.Add(lblLoopsHint);
        panelOptions.Controls.Add(lblDelay);
        panelOptions.Controls.Add(nudDelay);
        panelOptions.Controls.Add(lblDelayUnit);

        // btnStop
        btnStop.Text = "Stop";
        btnStop.Dock = DockStyle.Right;
        btnStop.Width = 80;
        btnStop.Enabled = false;
        btnStop.Click += btnStop_Click;

        // btnStart
        btnStart.Text = "Start";
        btnStart.Dock = DockStyle.Right;
        btnStart.Width = 80;
        btnStart.Click += btnStart_Click;

        // lblStatus
        lblStatus.Text = "Status: Idle";
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblStatus.Padding = new Padding(4, 0, 0, 0);

        // panelBottom -- btnStop added first so it docks rightmost
        panelBottom.Dock = DockStyle.Bottom;
        panelBottom.Height = 30;
        panelBottom.Padding = new Padding(0, 3, 0, 3);
        panelBottom.Controls.Add(lblStatus);
        panelBottom.Controls.Add(btnStart);
        panelBottom.Controls.Add(btnStop);

        // Main
        AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        Name = "Main";
        Controls.Add(grpScripts);    // Fill
        Controls.Add(panelOptions);  // Bottom
        Controls.Add(panelBottom);   // Bottom (below panelOptions)

        ((System.ComponentModel.ISupportInitialize)nudLoops).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudDelay).EndInit();
        panelButtons.ResumeLayout(false);
        panelOptions.ResumeLayout(false);
        panelBottom.ResumeLayout(false);
        grpScripts.ResumeLayout(false);
        ResumeLayout(false);
    }
}
