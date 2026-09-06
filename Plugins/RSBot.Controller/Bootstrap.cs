using System.Windows.Forms;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Plugins;

namespace RSBot.Controller;

public class Bootstrap : IPlugin
{
    private static Views.Main _view;

    public string Author => "RSBot Team";
    public string Description => "Automates multi-account sessions: login, run bot, sort inventory, repeat.";
    public string Name => "RSBot.Controller";
    public string Title => "Controller";
    public string Version => "1.0.0";
    public bool Enabled { get; set; }
    public bool DisplayAsTab => true;
    public int Index => 99;
    public bool RequireIngame => false;

    public Control View
    {
        get
        {
            if (_view == null || _view.IsDisposed || _view.Disposing)
                _view = new Views.Main();
            return _view;
        }
    }

    public void Initialize()
    {
        AppService.Initialize();
    }

    public void OnLoadCharacter() { }

    public void Translate()
    {
        LanguageManager.Translate(View, Kernel.Language);
    }

    public void Enable()
    {
        if (View != null) View.Enabled = true;
    }

    public void Disable()
    {
        if (View != null) View.Enabled = false;
    }
}
