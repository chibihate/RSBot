using System.Windows.Forms;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Event;
using RSBot.Core.Plugins;

namespace RSBot.AutoScripts;

public class Bootstrap : IPlugin
{
    public string Author => "RSBot Team";
    public string Description => "Runs multiple scripts in sequence.";
    public string Name => "RSBot.AutoScripts";
    public string Title => "Auto Scripts";
    public string Version => "1.0.0";
    public bool Enabled { get; set; }
    public bool DisplayAsTab => true;
    public int Index => 10;
    public bool RequireIngame => true;

    public Control View => AppService.View;

    public void Initialize()
    {
        AppService.Bot = new AutoScriptsService();
        EventManager.SubscribeEvent("OnAutoScriptsStart", OnAutoScriptsStart);
        EventManager.SubscribeEvent("OnAutoScriptsStop", () => AppService.Bot.Stop());
        Log.Debug("[AutoScripts] Plugin initialized.");
    }

    public void OnLoadCharacter()
    {
        AppService.View?.LoadSettings();
    }

    /// <summary>
    /// Fired by Controller (or any external caller) to start scripts with injected config.
    /// Reads script paths, loop count and delay directly from PlayerConfig.
    /// </summary>
    private void OnAutoScriptsStart()
    {
        var raw = PlayerConfig.Get("RSBot.AutoScript.Scripts", string.Empty);
        AppService.Bot.ScriptPaths.Clear();
        foreach (var path in raw.Split(';'))
        {
            if (!string.IsNullOrWhiteSpace(path))
                AppService.Bot.ScriptPaths.Add(path);
        }

        AppService.Bot.LoopCount = PlayerConfig.Get("RSBot.AutoScript.Loops", 1);
        AppService.Bot.LoopDelay = PlayerConfig.Get("RSBot.AutoScript.LoopDelay", 0) * 1000;
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
