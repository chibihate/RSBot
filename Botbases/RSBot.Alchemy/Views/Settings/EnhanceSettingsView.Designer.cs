
using System.Windows.Forms;
using ComboBox = SDUI.Controls.ComboBox;
using GroupBox = SDUI.Controls.GroupBox;
using Label = SDUI.Controls.Label;
using TabControl = SDUI.Controls.TabControl;
using Panel = SDUI.Controls.Panel;
using RadioButton = SDUI.Controls.Radio;
using CheckBox = SDUI.Controls.CheckBox;

namespace RSBot.Alchemy.Views.Settings
{
    partial class EnhanceSettingsView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabSettings = new TabPage();
            tabRules = new TabPage();

            // Settings tab controls
            lblMaxOptLevel = new Label();
            numMaxEnhancement = new SDUI.Controls.NumUpDown();
            lblPlus = new Label();
            lblCurrentOptLevel = new Label();
            lblElixir = new Label();
            comboElixir = new ComboBox();
            linkRefreshItemList = new Label();
            lblLuckyPowder = new Label();
            comboLuckyPowder = new ComboBox();
            checkUseLuckyStones = new CheckBox();
            lblLuckyFrom = new Label();
            numLuckyFromLevel = new SDUI.Controls.NumUpDown();
            lblLuckyCount = new Label();
            checkUseImmortalStones = new CheckBox();
            lblImmortalCount = new Label();
            checkUseAstralStones = new CheckBox();
            lblAstralCount = new Label();
            checkUseSteadyStones = new CheckBox();
            lblSteadyStonesCount = new Label();

            // Rules tab controls
            listRules = new System.Windows.Forms.ListView();
            colRuleLevel = new ColumnHeader();
            colRuleElixir = new ColumnHeader();
            colRulePowder = new ColumnHeader();
            colRuleMaxLucky = new ColumnHeader();
            lblRuleEditor = new Label();
            lblRuleLevel = new Label();
            numRuleLevel = new SDUI.Controls.NumUpDown();
            lblRuleElixir = new Label();
            comboRuleElixir = new ComboBox();
            lblRulePowder = new Label();
            comboRulePowder = new ComboBox();
            lblRuleLucky = new Label();
            numRuleLucky = new SDUI.Controls.NumUpDown();
            btnAddRule = new System.Windows.Forms.Button();
            btnRemoveRule = new System.Windows.Forms.Button();
            btnRemoveAllRules = new System.Windows.Forms.Button();

            tabControl.SuspendLayout();
            tabSettings.SuspendLayout();
            tabRules.SuspendLayout();
            SuspendLayout();

            // ── tabControl ──────────────────────────────────────────────────
            tabControl.Controls.Add(tabSettings);
            tabControl.Controls.Add(tabRules);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.TabIndex = 0;

            // ── tabSettings ─────────────────────────────────────────────────
            tabSettings.Controls.Add(lblMaxOptLevel);
            tabSettings.Controls.Add(numMaxEnhancement);
            tabSettings.Controls.Add(lblPlus);
            tabSettings.Controls.Add(lblCurrentOptLevel);
            tabSettings.Controls.Add(lblElixir);
            tabSettings.Controls.Add(comboElixir);
            tabSettings.Controls.Add(linkRefreshItemList);
            tabSettings.Controls.Add(lblLuckyPowder);
            tabSettings.Controls.Add(comboLuckyPowder);
            tabSettings.Controls.Add(checkUseLuckyStones);
            tabSettings.Controls.Add(lblLuckyFrom);
            tabSettings.Controls.Add(numLuckyFromLevel);
            tabSettings.Controls.Add(lblLuckyCount);
            tabSettings.Controls.Add(checkUseImmortalStones);
            tabSettings.Controls.Add(lblImmortalCount);
            tabSettings.Controls.Add(checkUseAstralStones);
            tabSettings.Controls.Add(lblAstralCount);
            tabSettings.Controls.Add(checkUseSteadyStones);
            tabSettings.Controls.Add(lblSteadyStonesCount);
            tabSettings.Name = "tabSettings";
            tabSettings.Text = "Settings";
            tabSettings.BackColor = System.Drawing.Color.Transparent;

            // ── tabRules ────────────────────────────────────────────────────
            tabRules.Controls.Add(listRules);
            tabRules.Controls.Add(lblRuleEditor);
            tabRules.Controls.Add(lblRuleLevel);
            tabRules.Controls.Add(numRuleLevel);
            tabRules.Controls.Add(lblRuleElixir);
            tabRules.Controls.Add(comboRuleElixir);
            tabRules.Controls.Add(lblRulePowder);
            tabRules.Controls.Add(comboRulePowder);
            tabRules.Controls.Add(lblRuleLucky);
            tabRules.Controls.Add(numRuleLucky);
            tabRules.Controls.Add(btnAddRule);
            tabRules.Controls.Add(btnRemoveRule);
            tabRules.Controls.Add(btnRemoveAllRules);
            tabRules.Name = "tabRules";
            tabRules.Text = "Rules";
            tabRules.BackColor = System.Drawing.Color.Transparent;

            // ────────────────────────────────────────────────────────────────
            // Settings tab — controls
            // ────────────────────────────────────────────────────────────────

            // lblMaxOptLevel
            lblMaxOptLevel.ApplyGradient = false;
            lblMaxOptLevel.AutoSize = true;
            lblMaxOptLevel.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblMaxOptLevel.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblMaxOptLevel.GradientAnimation = false;
            lblMaxOptLevel.Location = new System.Drawing.Point(13, 14);
            lblMaxOptLevel.Name = "lblMaxOptLevel";
            lblMaxOptLevel.TabIndex = 0;
            lblMaxOptLevel.Text = "Max enhancement:";

            // numMaxEnhancement
            numMaxEnhancement.BackColor = System.Drawing.Color.Transparent;
            numMaxEnhancement.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            numMaxEnhancement.Location = new System.Drawing.Point(169, 12);
            numMaxEnhancement.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numMaxEnhancement.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxEnhancement.MinimumSize = new System.Drawing.Size(80, 25);
            numMaxEnhancement.Name = "numMaxEnhancement";
            numMaxEnhancement.Size = new System.Drawing.Size(80, 25);
            numMaxEnhancement.TabIndex = 1;
            numMaxEnhancement.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxEnhancement.ValueChanged += config_CheckedChange;
            numMaxEnhancement.Validated += config_CheckedChange;

            // lblPlus
            lblPlus.ApplyGradient = false;
            lblPlus.AutoSize = true;
            lblPlus.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblPlus.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblPlus.GradientAnimation = false;
            lblPlus.Location = new System.Drawing.Point(152, 14);
            lblPlus.Name = "lblPlus";
            lblPlus.TabIndex = 2;
            lblPlus.Text = "+";

            // lblCurrentOptLevel
            lblCurrentOptLevel.ApplyGradient = false;
            lblCurrentOptLevel.AutoSize = true;
            lblCurrentOptLevel.BackColor = System.Drawing.Color.Transparent;
            lblCurrentOptLevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblCurrentOptLevel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            lblCurrentOptLevel.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblCurrentOptLevel.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblCurrentOptLevel.GradientAnimation = false;
            lblCurrentOptLevel.Location = new System.Drawing.Point(262, 10);
            lblCurrentOptLevel.Name = "lblCurrentOptLevel";
            lblCurrentOptLevel.TabIndex = 12;
            lblCurrentOptLevel.Text = "+0";

            // lblElixir
            lblElixir.ApplyGradient = false;
            lblElixir.AutoSize = true;
            lblElixir.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblElixir.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblElixir.GradientAnimation = false;
            lblElixir.Location = new System.Drawing.Point(83, 50);
            lblElixir.Name = "lblElixir";
            lblElixir.TabIndex = 5;
            lblElixir.Text = "Elixir:";

            // comboElixir
            comboElixir.BackColor = System.Drawing.Color.Black;
            comboElixir.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            comboElixir.DropDownHeight = 100;
            comboElixir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboElixir.FormattingEnabled = true;
            comboElixir.IntegralHeight = false;
            comboElixir.ItemHeight = 17;
            comboElixir.Location = new System.Drawing.Point(140, 47);
            comboElixir.Name = "comboElixir";
            comboElixir.Radius = 5;
            comboElixir.ShadowDepth = 4F;
            comboElixir.Size = new System.Drawing.Size(193, 23);
            comboElixir.TabIndex = 6;
            comboElixir.SelectedIndexChanged += config_CheckedChange;

            // linkRefreshItemList
            linkRefreshItemList.ApplyGradient = false;
            linkRefreshItemList.AutoSize = true;
            linkRefreshItemList.Cursor = System.Windows.Forms.Cursors.Hand;
            linkRefreshItemList.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            linkRefreshItemList.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            linkRefreshItemList.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            linkRefreshItemList.GradientAnimation = false;
            linkRefreshItemList.Location = new System.Drawing.Point(340, 40);
            linkRefreshItemList.Name = "linkRefreshItemList";
            linkRefreshItemList.TabIndex = 7;
            linkRefreshItemList.Text = "🗘";
            linkRefreshItemList.Click += linkRefreshItemList_Click;

            // lblLuckyPowder
            lblLuckyPowder.ApplyGradient = false;
            lblLuckyPowder.AutoSize = true;
            lblLuckyPowder.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblLuckyPowder.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblLuckyPowder.GradientAnimation = false;
            lblLuckyPowder.Location = new System.Drawing.Point(42, 78);
            lblLuckyPowder.Name = "lblLuckyPowder";
            lblLuckyPowder.TabIndex = 20;
            lblLuckyPowder.Text = "Lucky Powder:";

            // comboLuckyPowder
            comboLuckyPowder.BackColor = System.Drawing.Color.Black;
            comboLuckyPowder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            comboLuckyPowder.DropDownHeight = 100;
            comboLuckyPowder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboLuckyPowder.FormattingEnabled = true;
            comboLuckyPowder.IntegralHeight = false;
            comboLuckyPowder.ItemHeight = 17;
            comboLuckyPowder.Location = new System.Drawing.Point(140, 75);
            comboLuckyPowder.Name = "comboLuckyPowder";
            comboLuckyPowder.Radius = 5;
            comboLuckyPowder.ShadowDepth = 4F;
            comboLuckyPowder.Size = new System.Drawing.Size(193, 23);
            comboLuckyPowder.TabIndex = 21;
            comboLuckyPowder.SelectedIndexChanged += config_CheckedChange;

            // checkUseLuckyStones
            checkUseLuckyStones.AutoSize = true;
            checkUseLuckyStones.BackColor = System.Drawing.Color.Transparent;
            checkUseLuckyStones.Depth = 0;
            checkUseLuckyStones.Location = new System.Drawing.Point(141, 130);
            checkUseLuckyStones.Margin = new System.Windows.Forms.Padding(0);
            checkUseLuckyStones.MouseLocation = new System.Drawing.Point(-1, -1);
            checkUseLuckyStones.Name = "checkUseLuckyStones";
            checkUseLuckyStones.Ripple = true;
            checkUseLuckyStones.Size = new System.Drawing.Size(142, 30);
            checkUseLuckyStones.TabIndex = 3;
            checkUseLuckyStones.Text = "Use lucky stones";
            checkUseLuckyStones.UseVisualStyleBackColor = false;
            checkUseLuckyStones.CheckedChanged += config_CheckedChange;

            // lblLuckyFrom
            lblLuckyFrom.ApplyGradient = false;
            lblLuckyFrom.AutoSize = true;
            lblLuckyFrom.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblLuckyFrom.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblLuckyFrom.GradientAnimation = false;
            lblLuckyFrom.Location = new System.Drawing.Point(283, 134);
            lblLuckyFrom.Name = "lblLuckyFrom";
            lblLuckyFrom.TabIndex = 22;
            lblLuckyFrom.Text = "from +";

            // numLuckyFromLevel
            numLuckyFromLevel.BackColor = System.Drawing.Color.Transparent;
            numLuckyFromLevel.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            numLuckyFromLevel.Location = new System.Drawing.Point(325, 130);
            numLuckyFromLevel.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numLuckyFromLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numLuckyFromLevel.MinimumSize = new System.Drawing.Size(50, 25);
            numLuckyFromLevel.Name = "numLuckyFromLevel";
            numLuckyFromLevel.Size = new System.Drawing.Size(50, 25);
            numLuckyFromLevel.TabIndex = 23;
            numLuckyFromLevel.Value = new decimal(new int[] { 5, 0, 0, 0 });
            numLuckyFromLevel.ValueChanged += config_CheckedChange;

            // lblLuckyCount
            lblLuckyCount.ApplyGradient = false;
            lblLuckyCount.AutoSize = true;
            lblLuckyCount.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblLuckyCount.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblLuckyCount.GradientAnimation = false;
            lblLuckyCount.Location = new System.Drawing.Point(380, 134);
            lblLuckyCount.Name = "lblLuckyCount";
            lblLuckyCount.TabIndex = 9;
            lblLuckyCount.Text = "x0";

            // checkUseImmortalStones
            checkUseImmortalStones.AutoSize = true;
            checkUseImmortalStones.BackColor = System.Drawing.Color.Transparent;
            checkUseImmortalStones.Depth = 0;
            checkUseImmortalStones.Location = new System.Drawing.Point(141, 153);
            checkUseImmortalStones.Margin = new System.Windows.Forms.Padding(0);
            checkUseImmortalStones.MouseLocation = new System.Drawing.Point(-1, -1);
            checkUseImmortalStones.Name = "checkUseImmortalStones";
            checkUseImmortalStones.Ripple = true;
            checkUseImmortalStones.Size = new System.Drawing.Size(170, 30);
            checkUseImmortalStones.TabIndex = 4;
            checkUseImmortalStones.Text = "Use immortal stones";
            checkUseImmortalStones.UseVisualStyleBackColor = false;
            checkUseImmortalStones.CheckedChanged += config_CheckedChange;

            // lblImmortalCount
            lblImmortalCount.ApplyGradient = false;
            lblImmortalCount.AutoSize = true;
            lblImmortalCount.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblImmortalCount.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblImmortalCount.GradientAnimation = false;
            lblImmortalCount.Location = new System.Drawing.Point(340, 157);
            lblImmortalCount.Name = "lblImmortalCount";
            lblImmortalCount.TabIndex = 9;
            lblImmortalCount.Text = "x0";

            // checkUseAstralStones
            checkUseAstralStones.AutoSize = true;
            checkUseAstralStones.BackColor = System.Drawing.Color.Transparent;
            checkUseAstralStones.Depth = 0;
            checkUseAstralStones.Location = new System.Drawing.Point(141, 176);
            checkUseAstralStones.Margin = new System.Windows.Forms.Padding(0);
            checkUseAstralStones.MouseLocation = new System.Drawing.Point(-1, -1);
            checkUseAstralStones.Name = "checkUseAstralStones";
            checkUseAstralStones.Ripple = true;
            checkUseAstralStones.Size = new System.Drawing.Size(145, 30);
            checkUseAstralStones.TabIndex = 8;
            checkUseAstralStones.Text = "Use astral stones";
            checkUseAstralStones.UseVisualStyleBackColor = false;
            checkUseAstralStones.CheckedChanged += config_CheckedChange;

            // lblAstralCount
            lblAstralCount.ApplyGradient = false;
            lblAstralCount.AutoSize = true;
            lblAstralCount.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblAstralCount.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblAstralCount.GradientAnimation = false;
            lblAstralCount.Location = new System.Drawing.Point(340, 180);
            lblAstralCount.Name = "lblAstralCount";
            lblAstralCount.TabIndex = 9;
            lblAstralCount.Text = "x0";

            // checkUseSteadyStones
            checkUseSteadyStones.AutoSize = true;
            checkUseSteadyStones.BackColor = System.Drawing.Color.Transparent;
            checkUseSteadyStones.Depth = 0;
            checkUseSteadyStones.Location = new System.Drawing.Point(141, 199);
            checkUseSteadyStones.Margin = new System.Windows.Forms.Padding(0);
            checkUseSteadyStones.MouseLocation = new System.Drawing.Point(-1, -1);
            checkUseSteadyStones.Name = "checkUseSteadyStones";
            checkUseSteadyStones.Ripple = true;
            checkUseSteadyStones.Size = new System.Drawing.Size(152, 30);
            checkUseSteadyStones.TabIndex = 8;
            checkUseSteadyStones.Text = "Use steady stones";
            checkUseSteadyStones.UseVisualStyleBackColor = false;
            checkUseSteadyStones.CheckedChanged += config_CheckedChange;

            // lblSteadyStonesCount
            lblSteadyStonesCount.ApplyGradient = false;
            lblSteadyStonesCount.AutoSize = true;
            lblSteadyStonesCount.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblSteadyStonesCount.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblSteadyStonesCount.GradientAnimation = false;
            lblSteadyStonesCount.Location = new System.Drawing.Point(340, 203);
            lblSteadyStonesCount.Name = "lblSteadyStonesCount";
            lblSteadyStonesCount.TabIndex = 9;
            lblSteadyStonesCount.Text = "x0";

            // ────────────────────────────────────────────────────────────────
            // Rules tab — controls
            // ────────────────────────────────────────────────────────────────

            // listRules
            listRules.Columns.AddRange(new ColumnHeader[] { colRuleLevel, colRuleElixir, colRulePowder, colRuleMaxLucky });
            listRules.FullRowSelect = true;
            listRules.GridLines = true;
            listRules.HideSelection = false;
            listRules.Location = new System.Drawing.Point(5, 5);
            listRules.MultiSelect = false;
            listRules.Name = "listRules";
            listRules.Size = new System.Drawing.Size(405, 138);
            listRules.TabIndex = 30;
            listRules.View = System.Windows.Forms.View.Details;
            listRules.SelectedIndexChanged += listRules_SelectedIndexChanged;

            colRuleLevel.Text = "Level";
            colRuleLevel.Width = 55;
            colRuleElixir.Text = "Elixir";
            colRuleElixir.Width = 110;
            colRulePowder.Text = "Lucky Powder";
            colRulePowder.Width = 110;
            colRuleMaxLucky.Text = "Max Lucky";
            colRuleMaxLucky.Width = 80;

            // lblRuleEditor
            lblRuleEditor.ApplyGradient = false;
            lblRuleEditor.AutoSize = true;
            lblRuleEditor.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            lblRuleEditor.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblRuleEditor.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblRuleEditor.GradientAnimation = false;
            lblRuleEditor.Location = new System.Drawing.Point(5, 150);
            lblRuleEditor.Name = "lblRuleEditor";
            lblRuleEditor.TabIndex = 31;
            lblRuleEditor.Text = "Rule Editor:";

            // lblRuleLevel
            lblRuleLevel.ApplyGradient = false;
            lblRuleLevel.AutoSize = true;
            lblRuleLevel.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblRuleLevel.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblRuleLevel.GradientAnimation = false;
            lblRuleLevel.Location = new System.Drawing.Point(5, 175);
            lblRuleLevel.Name = "lblRuleLevel";
            lblRuleLevel.TabIndex = 32;
            lblRuleLevel.Text = "Level +";

            // numRuleLevel
            numRuleLevel.BackColor = System.Drawing.Color.Transparent;
            numRuleLevel.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            numRuleLevel.Location = new System.Drawing.Point(60, 173);
            numRuleLevel.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numRuleLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRuleLevel.MinimumSize = new System.Drawing.Size(55, 25);
            numRuleLevel.Name = "numRuleLevel";
            numRuleLevel.Size = new System.Drawing.Size(55, 25);
            numRuleLevel.TabIndex = 33;
            numRuleLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblRuleElixir
            lblRuleElixir.ApplyGradient = false;
            lblRuleElixir.AutoSize = true;
            lblRuleElixir.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblRuleElixir.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblRuleElixir.GradientAnimation = false;
            lblRuleElixir.Location = new System.Drawing.Point(125, 175);
            lblRuleElixir.Name = "lblRuleElixir";
            lblRuleElixir.TabIndex = 34;
            lblRuleElixir.Text = "Elixir:";

            // comboRuleElixir
            comboRuleElixir.BackColor = System.Drawing.Color.Black;
            comboRuleElixir.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            comboRuleElixir.DropDownHeight = 100;
            comboRuleElixir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboRuleElixir.FormattingEnabled = true;
            comboRuleElixir.IntegralHeight = false;
            comboRuleElixir.ItemHeight = 17;
            comboRuleElixir.Location = new System.Drawing.Point(165, 173);
            comboRuleElixir.Name = "comboRuleElixir";
            comboRuleElixir.Radius = 5;
            comboRuleElixir.ShadowDepth = 4F;
            comboRuleElixir.Size = new System.Drawing.Size(150, 23);
            comboRuleElixir.TabIndex = 35;

            // lblRulePowder
            lblRulePowder.ApplyGradient = false;
            lblRulePowder.AutoSize = true;
            lblRulePowder.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblRulePowder.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblRulePowder.GradientAnimation = false;
            lblRulePowder.Location = new System.Drawing.Point(5, 205);
            lblRulePowder.Name = "lblRulePowder";
            lblRulePowder.TabIndex = 36;
            lblRulePowder.Text = "Powder:";

            // comboRulePowder
            comboRulePowder.BackColor = System.Drawing.Color.Black;
            comboRulePowder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            comboRulePowder.DropDownHeight = 100;
            comboRulePowder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboRulePowder.FormattingEnabled = true;
            comboRulePowder.IntegralHeight = false;
            comboRulePowder.ItemHeight = 17;
            comboRulePowder.Location = new System.Drawing.Point(60, 203);
            comboRulePowder.Name = "comboRulePowder";
            comboRulePowder.Radius = 5;
            comboRulePowder.ShadowDepth = 4F;
            comboRulePowder.Size = new System.Drawing.Size(150, 23);
            comboRulePowder.TabIndex = 37;

            // lblRuleLucky
            lblRuleLucky.ApplyGradient = false;
            lblRuleLucky.AutoSize = true;
            lblRuleLucky.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lblRuleLucky.Gradient = new System.Drawing.Color[] { System.Drawing.Color.Gray, System.Drawing.Color.Black };
            lblRuleLucky.GradientAnimation = false;
            lblRuleLucky.Location = new System.Drawing.Point(222, 205);
            lblRuleLucky.Name = "lblRuleLucky";
            lblRuleLucky.TabIndex = 38;
            lblRuleLucky.Text = "Max Lucky (0=off):";

            // numRuleLucky
            numRuleLucky.BackColor = System.Drawing.Color.Transparent;
            numRuleLucky.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            numRuleLucky.Location = new System.Drawing.Point(348, 203);
            numRuleLucky.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numRuleLucky.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numRuleLucky.MinimumSize = new System.Drawing.Size(52, 25);
            numRuleLucky.Name = "numRuleLucky";
            numRuleLucky.Size = new System.Drawing.Size(52, 25);
            numRuleLucky.TabIndex = 39;
            numRuleLucky.Value = new decimal(new int[] { 0, 0, 0, 0 });

            // btnAddRule
            btnAddRule.Location = new System.Drawing.Point(5, 235);
            btnAddRule.Name = "btnAddRule";
            btnAddRule.Size = new System.Drawing.Size(90, 26);
            btnAddRule.TabIndex = 40;
            btnAddRule.Text = "Add / Update";
            btnAddRule.Click += btnAddRule_Click;

            // btnRemoveRule
            btnRemoveRule.Location = new System.Drawing.Point(103, 235);
            btnRemoveRule.Name = "btnRemoveRule";
            btnRemoveRule.Size = new System.Drawing.Size(90, 26);
            btnRemoveRule.TabIndex = 41;
            btnRemoveRule.Text = "Remove";
            btnRemoveRule.Click += btnRemoveRule_Click;

            // btnRemoveAllRules
            btnRemoveAllRules.Location = new System.Drawing.Point(201, 235);
            btnRemoveAllRules.Name = "btnRemoveAllRules";
            btnRemoveAllRules.Size = new System.Drawing.Size(90, 26);
            btnRemoveAllRules.TabIndex = 42;
            btnRemoveAllRules.Text = "Remove All";
            btnRemoveAllRules.Click += btnRemoveAllRules_Click;

            // ────────────────────────────────────────────────────────────────
            // EnhanceSettingsView
            // ────────────────────────────────────────────────────────────────
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Controls.Add(tabControl);
            Enabled = false;
            Font = new System.Drawing.Font("Segoe UI", 9F);
            Name = "EnhanceSettingsView";
            Size = new System.Drawing.Size(438, 306);

            tabControl.ResumeLayout(false);
            tabSettings.ResumeLayout(false);
            tabSettings.PerformLayout();
            tabRules.ResumeLayout(false);
            tabRules.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabSettings;
        private TabPage tabRules;

        // Settings tab
        private Label lblMaxOptLevel;
        private Label lblPlus;
        private Label lblCurrentOptLevel;
        private CheckBox checkUseLuckyStones;
        private Label lblLuckyFrom;
        private SDUI.Controls.NumUpDown numLuckyFromLevel;
        private CheckBox checkUseImmortalStones;
        private Label lblElixir;
        private ComboBox comboElixir;
        private Label linkRefreshItemList;
        private CheckBox checkUseAstralStones;
        private Label lblLuckyCount;
        private Label lblImmortalCount;
        private Label lblAstralCount;
        private CheckBox checkUseSteadyStones;
        private Label lblSteadyStonesCount;
        private SDUI.Controls.NumUpDown numMaxEnhancement;
        private Label lblLuckyPowder;
        private ComboBox comboLuckyPowder;

        // Rules tab
        private System.Windows.Forms.ListView listRules;
        private ColumnHeader colRuleLevel;
        private ColumnHeader colRuleElixir;
        private ColumnHeader colRulePowder;
        private ColumnHeader colRuleMaxLucky;
        private Label lblRuleEditor;
        private Label lblRuleLevel;
        private SDUI.Controls.NumUpDown numRuleLevel;
        private Label lblRuleElixir;
        private ComboBox comboRuleElixir;
        private Label lblRulePowder;
        private ComboBox comboRulePowder;
        private Label lblRuleLucky;
        private SDUI.Controls.NumUpDown numRuleLucky;
        private System.Windows.Forms.Button btnAddRule;
        private System.Windows.Forms.Button btnRemoveRule;
        private System.Windows.Forms.Button btnRemoveAllRules;
    }
}
