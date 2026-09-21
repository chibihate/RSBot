using System.Windows.Forms;

namespace RSBot.Scripts.Views;

partial class Main
{
    private System.ComponentModel.IContainer components = null;

    // Minimap
    private System.Windows.Forms.Panel pnlContent;
    private System.Windows.Forms.Panel pnlMinimap;
    private System.Windows.Forms.PictureBox picMinimap;
    private System.Windows.Forms.Label lblMinimapPos;

    // Script list controls
    private System.Windows.Forms.GroupBox grpScripts;
    private System.Windows.Forms.ListBox lstScripts;
    private System.Windows.Forms.Panel panelButtons;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Button btnUp;
    private System.Windows.Forms.Button btnDown;

    // Quick script slots
    private System.Windows.Forms.GroupBox grpQuickScripts;
    private System.Windows.Forms.TextBox txtScript1Path;
    private System.Windows.Forms.Button btnScript1Browse;
    private System.Windows.Forms.Button btnScript1Play;
    private System.Windows.Forms.Button btnScript1Stop;
    private System.Windows.Forms.TextBox txtScript2Path;
    private System.Windows.Forms.Button btnScript2Browse;
    private System.Windows.Forms.Button btnScript2Play;
    private System.Windows.Forms.Button btnScript2Stop;
    private System.Windows.Forms.TextBox txtScript3Path;
    private System.Windows.Forms.Button btnScript3Browse;
    private System.Windows.Forms.Button btnScript3Play;
    private System.Windows.Forms.Button btnScript3Stop;

    // Options
    private System.Windows.Forms.Panel panelOptions;
    private System.Windows.Forms.Label lblLoops;
    private System.Windows.Forms.NumericUpDown nudLoops;
    private System.Windows.Forms.Label lblLoopsHint;
    private System.Windows.Forms.Label lblDelay;
    private System.Windows.Forms.NumericUpDown nudDelay;
    private System.Windows.Forms.Label lblDelayUnit;

    // Sound
    private System.Windows.Forms.Panel panelSound;
    private System.Windows.Forms.Label lblSound;
    private System.Windows.Forms.TextBox txtSoundPath;
    private System.Windows.Forms.Button btnBrowseSound;

    // Bottom bar (list Start/Stop)
    private System.Windows.Forms.Panel panelBottom;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Label lblStatus;

    private void InitializeComponent()
    {
        pnlContent       = new System.Windows.Forms.Panel();
        pnlMinimap       = new System.Windows.Forms.Panel();
        picMinimap       = new System.Windows.Forms.PictureBox();
        lblMinimapPos    = new System.Windows.Forms.Label();
        grpScripts       = new System.Windows.Forms.GroupBox();
        lstScripts       = new System.Windows.Forms.ListBox();
        panelButtons     = new System.Windows.Forms.Panel();
        btnAdd           = new System.Windows.Forms.Button();
        btnRemove        = new System.Windows.Forms.Button();
        btnUp            = new System.Windows.Forms.Button();
        btnDown          = new System.Windows.Forms.Button();
        grpQuickScripts  = new System.Windows.Forms.GroupBox();
        txtScript1Path   = new System.Windows.Forms.TextBox();
        btnScript1Browse = new System.Windows.Forms.Button();
        btnScript1Play   = new System.Windows.Forms.Button();
        btnScript1Stop   = new System.Windows.Forms.Button();
        txtScript2Path   = new System.Windows.Forms.TextBox();
        btnScript2Browse = new System.Windows.Forms.Button();
        btnScript2Play   = new System.Windows.Forms.Button();
        btnScript2Stop   = new System.Windows.Forms.Button();
        txtScript3Path   = new System.Windows.Forms.TextBox();
        btnScript3Browse = new System.Windows.Forms.Button();
        btnScript3Play   = new System.Windows.Forms.Button();
        btnScript3Stop   = new System.Windows.Forms.Button();
        panelOptions     = new System.Windows.Forms.Panel();
        lblLoops         = new System.Windows.Forms.Label();
        nudLoops         = new System.Windows.Forms.NumericUpDown();
        lblLoopsHint     = new System.Windows.Forms.Label();
        lblDelay         = new System.Windows.Forms.Label();
        nudDelay         = new System.Windows.Forms.NumericUpDown();
        lblDelayUnit     = new System.Windows.Forms.Label();
        panelSound       = new System.Windows.Forms.Panel();
        lblSound         = new System.Windows.Forms.Label();
        txtSoundPath     = new System.Windows.Forms.TextBox();
        btnBrowseSound   = new System.Windows.Forms.Button();
        panelBottom      = new System.Windows.Forms.Panel();
        btnStart         = new System.Windows.Forms.Button();
        btnStop          = new System.Windows.Forms.Button();
        lblStatus        = new System.Windows.Forms.Label();

        pnlContent.SuspendLayout();
        pnlMinimap.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picMinimap).BeginInit();
        grpScripts.SuspendLayout();
        panelButtons.SuspendLayout();
        grpQuickScripts.SuspendLayout();
        panelOptions.SuspendLayout();
        panelSound.SuspendLayout();
        panelBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudLoops).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDelay).BeginInit();
        SuspendLayout();

        // ── panelButtons ─────────────────────────────────────────────────────
        panelButtons.Dock    = DockStyle.Bottom;
        panelButtons.Height  = 30;
        panelButtons.Padding = new Padding(2, 3, 2, 0);
        panelButtons.Controls.Add(btnAdd);
        panelButtons.Controls.Add(btnRemove);
        panelButtons.Controls.Add(btnUp);
        panelButtons.Controls.Add(btnDown);

        btnAdd.Text     = "Add";
        btnAdd.Location = new System.Drawing.Point(2, 3);
        btnAdd.Size     = new System.Drawing.Size(60, 24);
        btnAdd.Click   += btnAdd_Click;

        btnRemove.Text     = "Remove";
        btnRemove.Location = new System.Drawing.Point(66, 3);
        btnRemove.Size     = new System.Drawing.Size(62, 24);
        btnRemove.Click   += btnRemove_Click;

        btnUp.Text     = "▲";
        btnUp.Location = new System.Drawing.Point(134, 3);
        btnUp.Size     = new System.Drawing.Size(44, 24);
        btnUp.Click   += btnUp_Click;

        btnDown.Text     = "▼";
        btnDown.Location = new System.Drawing.Point(182, 3);
        btnDown.Size     = new System.Drawing.Size(44, 24);
        btnDown.Click   += btnDown_Click;

        // ── lstScripts ───────────────────────────────────────────────────────
        lstScripts.Dock          = DockStyle.Fill;
        lstScripts.SelectionMode = SelectionMode.One;
        lstScripts.DoubleClick  += lstScripts_DoubleClick;

        // ── grpScripts ───────────────────────────────────────────────────────
        grpScripts.Text    = "Scripts";
        grpScripts.Dock    = DockStyle.Fill;
        grpScripts.Padding = new Padding(4, 4, 4, 4);
        grpScripts.Controls.Add(lstScripts);
        grpScripts.Controls.Add(panelButtons);

        // ── pnlMinimap ───────────────────────────────────────────────────────
        lblMinimapPos.Dock      = DockStyle.Bottom;
        lblMinimapPos.Height    = 16;
        lblMinimapPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        lblMinimapPos.Font      = new System.Drawing.Font(Font.FontFamily, 7f);
        lblMinimapPos.ForeColor = System.Drawing.SystemColors.GrayText;

        picMinimap.Dock      = DockStyle.Fill;
        picMinimap.SizeMode  = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        picMinimap.BackColor = System.Drawing.Color.Black;

        pnlMinimap.Dock  = DockStyle.Right;
        pnlMinimap.Width = 155;
        pnlMinimap.Controls.Add(picMinimap);
        pnlMinimap.Controls.Add(lblMinimapPos);

        // ── pnlContent ───────────────────────────────────────────────────────
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Controls.Add(grpScripts);   // Fill — added first = laid out last
        pnlContent.Controls.Add(pnlMinimap);   // Right — added after = laid out first

        // ── grpQuickScripts ──────────────────────────────────────────────────
        grpQuickScripts.Text    = "Quick Scripts";
        grpQuickScripts.Dock    = DockStyle.Bottom;
        grpQuickScripts.Height  = 104;
        grpQuickScripts.Padding = new Padding(4, 2, 4, 2);

        BuildSlotRow(grpQuickScripts, "Script 1:", 0,
            txtScript1Path, btnScript1Browse, btnScript1Play, btnScript1Stop,
            btnScript1Browse_Click, btnScript1Play_Click, btnScript1Stop_Click);

        BuildSlotRow(grpQuickScripts, "Script 2:", 1,
            txtScript2Path, btnScript2Browse, btnScript2Play, btnScript2Stop,
            btnScript2Browse_Click, btnScript2Play_Click, btnScript2Stop_Click);

        BuildSlotRow(grpQuickScripts, "Script 3:", 2,
            txtScript3Path, btnScript3Browse, btnScript3Play, btnScript3Stop,
            btnScript3Browse_Click, btnScript3Play_Click, btnScript3Stop_Click);

        // ── panelOptions ─────────────────────────────────────────────────────
        panelOptions.Dock   = DockStyle.Bottom;
        panelOptions.Height = 28;

        lblLoops.Text      = "Loops:";
        lblLoops.Location  = new System.Drawing.Point(4, 6);
        lblLoops.Size      = new System.Drawing.Size(44, 18);
        lblLoops.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        nudLoops.Location      = new System.Drawing.Point(50, 4);
        nudLoops.Size          = new System.Drawing.Size(56, 22);
        nudLoops.Minimum       = 0;
        nudLoops.Maximum       = 9999;
        nudLoops.Value         = 1;
        nudLoops.ValueChanged += nudOptions_ValueChanged;

        lblLoopsHint.Text      = "(0=inf)";
        lblLoopsHint.Location  = new System.Drawing.Point(110, 6);
        lblLoopsHint.Size      = new System.Drawing.Size(48, 18);
        lblLoopsHint.ForeColor = System.Drawing.SystemColors.GrayText;
        lblLoopsHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        lblDelay.Text      = "Delay:";
        lblDelay.Location  = new System.Drawing.Point(168, 6);
        lblDelay.Size      = new System.Drawing.Size(40, 18);
        lblDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        nudDelay.Location      = new System.Drawing.Point(210, 4);
        nudDelay.Size          = new System.Drawing.Size(64, 22);
        nudDelay.Minimum       = 0;
        nudDelay.Maximum       = 3600;
        nudDelay.Value         = 0;
        nudDelay.ValueChanged += nudOptions_ValueChanged;

        lblDelayUnit.Text      = "sec";
        lblDelayUnit.Location  = new System.Drawing.Point(278, 6);
        lblDelayUnit.Size      = new System.Drawing.Size(30, 18);
        lblDelayUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        panelOptions.Controls.Add(lblLoops);
        panelOptions.Controls.Add(nudLoops);
        panelOptions.Controls.Add(lblLoopsHint);
        panelOptions.Controls.Add(lblDelay);
        panelOptions.Controls.Add(nudDelay);
        panelOptions.Controls.Add(lblDelayUnit);

        // ── panelSound ───────────────────────────────────────────────────────
        panelSound.Dock   = DockStyle.Bottom;
        panelSound.Height = 28;

        lblSound.Text      = "Sound:";
        lblSound.Location  = new System.Drawing.Point(4, 6);
        lblSound.Size      = new System.Drawing.Size(44, 18);
        lblSound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        btnBrowseSound.Text   = "...";
        btnBrowseSound.Dock   = DockStyle.Right;
        btnBrowseSound.Width  = 28;
        btnBrowseSound.Click += btnBrowseSound_Click;

        txtSoundPath.Dock         = DockStyle.Fill;
        txtSoundPath.Margin       = new Padding(0);
        txtSoundPath.TextChanged += txtSoundPath_TextChanged;

        var panelSoundInner = new System.Windows.Forms.Panel();
        panelSoundInner.Location = new System.Drawing.Point(52, 4);
        panelSoundInner.Size     = new System.Drawing.Size(panelSound.Width - 56, 20);
        panelSoundInner.Anchor   = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        panelSoundInner.Controls.Add(txtSoundPath);
        panelSoundInner.Controls.Add(btnBrowseSound);

        panelSound.Controls.Add(lblSound);
        panelSound.Controls.Add(panelSoundInner);

        // ── panelBottom ──────────────────────────────────────────────────────
        btnStop.Text    = "Stop";
        btnStop.Dock    = DockStyle.Right;
        btnStop.Width   = 80;
        btnStop.Enabled = false;
        btnStop.Click  += btnStop_Click;

        btnStart.Text   = "Start";
        btnStart.Dock   = DockStyle.Right;
        btnStart.Width  = 80;
        btnStart.Click += btnStart_Click;

        lblStatus.Text      = "Status: Idle";
        lblStatus.Dock      = DockStyle.Fill;
        lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblStatus.Padding   = new Padding(4, 0, 0, 0);

        panelBottom.Dock    = DockStyle.Bottom;
        panelBottom.Height  = 30;
        panelBottom.Padding = new Padding(0, 3, 0, 3);
        panelBottom.Controls.Add(lblStatus);
        panelBottom.Controls.Add(btnStart);
        panelBottom.Controls.Add(btnStop);

        // ── Main ─────────────────────────────────────────────────────────────
        AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        AutoScaleMode       = AutoScaleMode.Font;
        Name = "Main";
        Controls.Add(pnlContent);       // Fill (contains grpScripts + minimap)
        Controls.Add(grpQuickScripts);  // Bottom (above panelSound)
        Controls.Add(panelSound);       // Bottom
        Controls.Add(panelOptions);     // Bottom
        Controls.Add(panelBottom);      // Bottom (very bottom)

        ((System.ComponentModel.ISupportInitialize)nudLoops).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudDelay).EndInit();
        ((System.ComponentModel.ISupportInitialize)picMinimap).EndInit();
        panelButtons.ResumeLayout(false);
        grpQuickScripts.ResumeLayout(false);
        panelOptions.ResumeLayout(false);
        panelSound.ResumeLayout(false);
        panelBottom.ResumeLayout(false);
        grpScripts.ResumeLayout(false);
        pnlMinimap.ResumeLayout(false);
        pnlContent.ResumeLayout(false);
        ResumeLayout(false);
    }

    private void BuildSlotRow(
        System.Windows.Forms.GroupBox parent,
        string label,
        int rowIndex,
        System.Windows.Forms.TextBox txtPath,
        System.Windows.Forms.Button btnBrowse,
        System.Windows.Forms.Button btnPlay,
        System.Windows.Forms.Button btnStop,
        System.EventHandler browseClick,
        System.EventHandler playClick,
        System.EventHandler stopClick)
    {
        const int rowH   = 26;
        const int topOff = 18;
        const int pad    = 4;
        int top = topOff + rowIndex * rowH + 2;

        var lbl = new System.Windows.Forms.Label
        {
            Text      = label,
            Location  = new System.Drawing.Point(pad, top + 3),
            Size      = new System.Drawing.Size(58, 18),
            TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
        };

        btnStop.Text    = "■";
        btnStop.Size    = new System.Drawing.Size(28, 22);
        btnStop.Anchor  = AnchorStyles.Right | AnchorStyles.Top;
        btnStop.Top     = top;
        btnStop.Left    = parent.ClientSize.Width - pad - 28;
        btnStop.Enabled = false;
        btnStop.Click  += stopClick;

        btnPlay.Text   = "▶";
        btnPlay.Size   = new System.Drawing.Size(30, 22);
        btnPlay.Anchor = AnchorStyles.Right | AnchorStyles.Top;
        btnPlay.Top    = top;
        btnPlay.Left   = btnStop.Left - 32;
        btnPlay.Click += playClick;

        btnBrowse.Text   = "...";
        btnBrowse.Size   = new System.Drawing.Size(28, 22);
        btnBrowse.Anchor = AnchorStyles.Right | AnchorStyles.Top;
        btnBrowse.Top    = top;
        btnBrowse.Left   = btnPlay.Left - 30;
        btnBrowse.Click += browseClick;

        txtPath.Location     = new System.Drawing.Point(lbl.Right + 2, top + 1);
        txtPath.Size         = new System.Drawing.Size(btnBrowse.Left - lbl.Right - 4, 20);
        txtPath.Anchor       = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        txtPath.TextChanged += txtScriptPath_TextChanged;

        parent.Controls.Add(lbl);
        parent.Controls.Add(txtPath);
        parent.Controls.Add(btnBrowse);
        parent.Controls.Add(btnPlay);
        parent.Controls.Add(btnStop);
    }
}
