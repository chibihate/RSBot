using System.Windows.Forms;

namespace RSBot.Repetition.Views;

internal class InputDialog : Form
{
    private readonly System.Windows.Forms.TextBox _input;
    public string Value => _input.Text.Trim();

    public InputDialog(string title, string prompt)
    {
        Text = title;
        Size = new System.Drawing.Size(320, 120);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        var lbl = new System.Windows.Forms.Label
        {
            Text = prompt,
            Location = new System.Drawing.Point(10, 12),
            AutoSize = true,
        };

        _input = new System.Windows.Forms.TextBox
        {
            Location = new System.Drawing.Point(10, 30),
            Size = new System.Drawing.Size(280, 22),
        };

        var btnOk = new System.Windows.Forms.Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Location = new System.Drawing.Point(130, 58),
            Size = new System.Drawing.Size(70, 26),
        };

        var btnCancel = new System.Windows.Forms.Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Location = new System.Drawing.Point(206, 58),
            Size = new System.Drawing.Size(70, 26),
        };

        AcceptButton = btnOk;
        CancelButton = btnCancel;
        Controls.AddRange(new Control[] { lbl, _input, btnOk, btnCancel });
    }
}
