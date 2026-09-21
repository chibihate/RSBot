using System.Windows.Forms;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Event;
using RSBot.Core.Plugins;

namespace RSBot.Scripts;

public class Bootstrap : IPlugin
{
    public string Author => "RSBot Team";
    public string Description => "Runs multiple scripts in sequence.";
    public string Name => "RSBot.Scripts";
    public string Title => "Scripts";
    public string Version => "1.0.0";
    public bool Enabled { get; set; }
    public bool DisplayAsTab => true;
    public int Index => 10;
    public bool RequireIngame => true;

    public Control View => AppService.View;

    public void Initialize()
    {
        AppService.Bot = new AutoScriptsService { FireGlobalEvents = true };
        AppService.Script1 = new AutoScriptsService { FireGlobalEvents = false };
        AppService.Script2 = new AutoScriptsService { FireGlobalEvents = false };
        AppService.Script3 = new AutoScriptsService { FireGlobalEvents = false };

        EventManager.SubscribeEvent("OnAutoScriptsStart", OnAutoScriptsStart);
        EventManager.SubscribeEvent("OnAutoScriptsStop", () => AppService.Bot.Stop());
        EventManager.SubscribeEvent("OnAgentServerDisconnected", OnDisconnect);
        EventManager.SubscribeEvent("OnExitClient", OnDisconnect);
        Log.Debug("[Scripts] Plugin initialized.");
    }

    public void OnLoadCharacter()
    {
        AppService.View?.LoadSettings();
    }

    private void OnDisconnect()
    {
        AppService.Bot.Stop();
        AppService.Script1.Stop();
        AppService.Script2.Stop();
        AppService.Script3.Stop();
    }

    private void OnAutoScriptsStart()
    {
        var raw = PlayerConfig.Get("RSBot.Scripts.Scripts", string.Empty);
        AppService.Bot.ScriptPaths.Clear();
        foreach (var path in raw.Split(';'))
        {
            if (!string.IsNullOrWhiteSpace(path))
                AppService.Bot.ScriptPaths.Add(path);
        }

        AppService.Bot.LoopCount = PlayerConfig.Get("RSBot.Scripts.Loops", 1);
        AppService.Bot.LoopDelay = PlayerConfig.Get("RSBot.Scripts.LoopDelay", 0) * 1000;
        AppService.Bot.Start();
    }

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
