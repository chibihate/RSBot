using System.Collections.Generic;
using System.Windows.Forms;

namespace RSBot.Repetition.Views;

internal class SelectAccountDialog : Form
{
    private readonly ComboBox _combo;
    public string SelectedUsername { get; private set; }

    public SelectAccountDialog(IEnumerable<string> accounts)
    {
        Text = "Select Account";
        Size = new System.Drawing.Size(320, 120);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        _combo = new ComboBox
        {
            Location = new System.Drawing.Point(10, 12),
            Size = new System.Drawing.Size(280, 24),
            DropDownStyle = ComboBoxStyle.DropDownList,
        };
        foreach (var acc in accounts)
            _combo.Items.Add(acc);
        if (_combo.Items.Count > 0) _combo.SelectedIndex = 0;

        var btnOk = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Location = new System.Drawing.Point(130, 46),
            Size = new System.Drawing.Size(70, 26),
        };
        btnOk.Click += (_, _) => { SelectedUsername = _combo.SelectedItem?.ToString(); };

        var btnCancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Location = new System.Drawing.Point(206, 46),
            Size = new System.Drawing.Size(70, 26),
        };

        AcceptButton = btnOk;
        CancelButton = btnCancel;
        Controls.AddRange(new Control[] { _combo, btnOk, btnCancel });
    }
}
