using System.Windows.Forms;

namespace RSBot.Repetition.Views;

partial class Main
{
    private System.ComponentModel.IContainer components = null;

    // Account queue
    private System.Windows.Forms.GroupBox grpQueue;
    private System.Windows.Forms.DataGridView dgvAccounts;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUsername;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.Panel panelQueueButtons;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Button btnReset;
    private System.Windows.Forms.Button btnReload;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Button btnUp;
    private System.Windows.Forms.Button btnDown;

    // Shared scripts
    private System.Windows.Forms.GroupBox grpScripts;
    private System.Windows.Forms.ListBox lstScripts;
    private System.Windows.Forms.Panel panelScriptButtons;
    private System.Windows.Forms.Button btnScriptAdd;
    private System.Windows.Forms.Button btnScriptRemove;
    private System.Windows.Forms.Button btnScriptUp;
    private System.Windows.Forms.Button btnScriptDown;

    // Bottom bar
    private System.Windows.Forms.Panel panelBottom;
    private System.Windows.Forms.Label lblDelay;
    private System.Windows.Forms.NumericUpDown nudDelay;
    private System.Windows.Forms.Label lblDelayUnit;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Label lblStatus;

    private void InitializeComponent()
    {
        grpQueue          = new System.Windows.Forms.GroupBox();
        dgvAccounts       = new System.Windows.Forms.DataGridView();
        colUsername        = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colStatus          = new System.Windows.Forms.DataGridViewTextBoxColumn();
        panelQueueButtons  = new System.Windows.Forms.Panel();
        btnAdd             = new System.Windows.Forms.Button();
        btnRemove          = new System.Windows.Forms.Button();
        btnReset           = new System.Windows.Forms.Button();
        btnReload          = new System.Windows.Forms.Button();
        btnClear           = new System.Windows.Forms.Button();
        btnUp              = new System.Windows.Forms.Button();
        btnDown            = new System.Windows.Forms.Button();

        grpScripts         = new System.Windows.Forms.GroupBox();
        lstScripts         = new System.Windows.Forms.ListBox();
        panelScriptButtons = new System.Windows.Forms.Panel();
        btnScriptAdd       = new System.Windows.Forms.Button();
        btnScriptRemove    = new System.Windows.Forms.Button();
        btnScriptUp        = new System.Windows.Forms.Button();
        btnScriptDown      = new System.Windows.Forms.Button();

        panelBottom  = new System.Windows.Forms.Panel();
        lblDelay     = new System.Windows.Forms.Label();
        nudDelay     = new System.Windows.Forms.NumericUpDown();
        lblDelayUnit = new System.Windows.Forms.Label();
        btnStart     = new System.Windows.Forms.Button();
        lblStatus    = new System.Windows.Forms.Label();

        grpQueue.SuspendLayout();
        grpScripts.SuspendLayout();
        panelQueueButtons.SuspendLayout();
        panelScriptButtons.SuspendLayout();
        panelBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDelay).BeginInit();
        SuspendLayout();

        // ── dgvAccounts ────────────────────────────────────────────
        dgvAccounts.Dock = DockStyle.Fill;
        dgvAccounts.AllowUserToAddRows = false;
        dgvAccounts.AllowUserToDeleteRows = false;
        dgvAccounts.ReadOnly = true;
        dgvAccounts.RowHeadersVisible = false;
        dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAccounts.MultiSelect = false;
        dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAccounts.Columns.AddRange(colUsername, colStatus);

        colUsername.HeaderText = "Username";
        colUsername.FillWeight = 70;
        colUsername.SortMode = DataGridViewColumnSortMode.NotSortable;

        colStatus.HeaderText = "Status";
        colStatus.FillWeight = 30;
        colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;

        // ── panelQueueButtons ──────────────────────────────────────
        // Layout: Add | Remove | Reset | Reload | Clear | ▲ | ▼
        panelQueueButtons.Dock = DockStyle.Bottom;
        panelQueueButtons.Height = 30;
        panelQueueButtons.Padding = new Padding(2, 3, 2, 0);
        panelQueueButtons.Controls.Add(btnAdd);
        panelQueueButtons.Controls.Add(btnRemove);
        panelQueueButtons.Controls.Add(btnReset);
        panelQueueButtons.Controls.Add(btnReload);
        panelQueueButtons.Controls.Add(btnClear);
        panelQueueButtons.Controls.Add(btnUp);
        panelQueueButtons.Controls.Add(btnDown);

        btnAdd.Text    = "Add";
        btnAdd.Location = new System.Drawing.Point(2, 3);
        btnAdd.Size     = new System.Drawing.Size(44, 24);
        btnAdd.Click   += btnAdd_Click;

        btnRemove.Text    = "Remove";
        btnRemove.Location = new System.Drawing.Point(50, 3);
        btnRemove.Size     = new System.Drawing.Size(55, 24);
        btnRemove.Click   += btnRemove_Click;

        btnReset.Text    = "Reset";
        btnReset.Location = new System.Drawing.Point(109, 3);
        btnReset.Size     = new System.Drawing.Size(44, 24);
        btnReset.Click   += btnReset_Click;

        btnReload.Text    = "Reload";
        btnReload.Location = new System.Drawing.Point(157, 3);
        btnReload.Size     = new System.Drawing.Size(50, 24);
        btnReload.Click   += btnReload_Click;

        btnClear.Text    = "Clear";
        btnClear.Location = new System.Drawing.Point(211, 3);
        btnClear.Size     = new System.Drawing.Size(44, 24);
        btnClear.Click   += btnClear_Click;

        btnUp.Text    = "▲";
        btnUp.Location = new System.Drawing.Point(261, 3);
        btnUp.Size     = new System.Drawing.Size(36, 24);
        btnUp.Click   += btnUp_Click;

        btnDown.Text    = "▼";
        btnDown.Location = new System.Drawing.Point(301, 3);
        btnDown.Size     = new System.Drawing.Size(36, 24);
        btnDown.Click   += btnDown_Click;

        // ── grpQueue ───────────────────────────────────────────────
        grpQueue.Text = "Account Queue";
        grpQueue.Dock = DockStyle.Fill;
        grpQueue.Padding = new Padding(4, 4, 4, 4);
        grpQueue.Controls.Add(dgvAccounts);       // Fill first
        grpQueue.Controls.Add(panelQueueButtons); // Bottom last

        // ── lstScripts ─────────────────────────────────────────────
        lstScripts.Dock = DockStyle.Fill;
        lstScripts.SelectionMode = SelectionMode.One;
        lstScripts.DoubleClick += lstScripts_DoubleClick;

        // ── panelScriptButtons ─────────────────────────────────────
        // Layout: Add | Remove | ▲ | ▼
        panelScriptButtons.Dock = DockStyle.Bottom;
        panelScriptButtons.Height = 30;
        panelScriptButtons.Padding = new Padding(2, 3, 2, 0);
        panelScriptButtons.Controls.Add(btnScriptAdd);
        panelScriptButtons.Controls.Add(btnScriptRemove);
        panelScriptButtons.Controls.Add(btnScriptUp);
        panelScriptButtons.Controls.Add(btnScriptDown);

        btnScriptAdd.Text    = "Add";
        btnScriptAdd.Location = new System.Drawing.Point(2, 3);
        btnScriptAdd.Size     = new System.Drawing.Size(52, 24);
        btnScriptAdd.Click   += btnScriptAdd_Click;

        btnScriptRemove.Text    = "Remove";
        btnScriptRemove.Location = new System.Drawing.Point(58, 3);
        btnScriptRemove.Size     = new System.Drawing.Size(60, 24);
        btnScriptRemove.Click   += btnScriptRemove_Click;

        btnScriptUp.Text    = "▲";
        btnScriptUp.Location = new System.Drawing.Point(124, 3);
        btnScriptUp.Size     = new System.Drawing.Size(44, 24);
        btnScriptUp.Click   += btnScriptUp_Click;

        btnScriptDown.Text    = "▼";
        btnScriptDown.Location = new System.Drawing.Point(172, 3);
        btnScriptDown.Size     = new System.Drawing.Size(44, 24);
        btnScriptDown.Click   += btnScriptDown_Click;

        // ── grpScripts ─────────────────────────────────────────────
        grpScripts.Text = "Shared Scripts (applied to every account)";
        grpScripts.Dock = DockStyle.Bottom;
        grpScripts.Height = 140;
        grpScripts.Padding = new Padding(4, 4, 4, 4);
        grpScripts.Controls.Add(lstScripts);         // Fill first
        grpScripts.Controls.Add(panelScriptButtons); // Bottom last

        // ── panelBottom ────────────────────────────────────────────
        panelBottom.Dock = DockStyle.Bottom;
        panelBottom.Height = 60;
        panelBottom.Padding = new Padding(6, 6, 6, 4);
        panelBottom.Controls.Add(lblDelay);
        panelBottom.Controls.Add(nudDelay);
        panelBottom.Controls.Add(lblDelayUnit);
        panelBottom.Controls.Add(btnStart);
        panelBottom.Controls.Add(lblStatus);

        lblDelay.Text = "Wait before start bot:";
        lblDelay.Location = new System.Drawing.Point(6, 8);
        lblDelay.AutoSize = true;

        nudDelay.Location = new System.Drawing.Point(148, 6);
        nudDelay.Size = new System.Drawing.Size(54, 22);
        nudDelay.Minimum = 1;
        nudDelay.Maximum = 300;
        nudDelay.Value = 10;
        nudDelay.ValueChanged += nudDelay_ValueChanged;

        lblDelayUnit.Text = "seconds";
        lblDelayUnit.Location = new System.Drawing.Point(206, 8);
        lblDelayUnit.AutoSize = true;

        btnStart.Text = "Start";
        btnStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnStart.Location = new System.Drawing.Point(286, 4);
        btnStart.Size = new System.Drawing.Size(80, 26);
        btnStart.Click += btnStart_Click;

        lblStatus.Text = "Status: Idle";
        lblStatus.Location = new System.Drawing.Point(6, 36);
        lblStatus.AutoSize = true;

        // ── Main — dock order: last added = outermost bottom ───────
        AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        Name = "Main";
        Controls.Add(grpQueue);     // Fill
        Controls.Add(grpScripts);   // Bottom (inner — above panelBottom)
        Controls.Add(panelBottom);  // Bottom (outer — at very bottom)

        panelQueueButtons.ResumeLayout(false);
        panelScriptButtons.ResumeLayout(false);
        panelBottom.ResumeLayout(false);
        panelBottom.PerformLayout();
        grpQueue.ResumeLayout(false);
        grpScripts.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudDelay).EndInit();
        ResumeLayout(false);
    }
}
