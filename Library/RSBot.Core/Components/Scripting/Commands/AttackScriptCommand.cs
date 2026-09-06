using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using RSBot.Core.Event;
using RSBot.Core.Objects;
using RSBot.Core.Objects.Spawn;

namespace RSBot.Core.Components.Scripting.Commands;

internal class AttackScriptCommand : IScriptCommand
{
    #region Fields

    private CancellationTokenSource _cts;

    #endregion Fields

    #region Properties

    public string Name => "attack";

    public bool IsBusy { get; private set; }

    public Dictionary<string, string> Arguments => new()
    {
        { "X",       "World X coordinate of the training area center" },
        { "Y",       "World Y coordinate of the training area center" },
        { "Radius",  "Training area radius (5-100)" },
        { "Retries", "Consecutive 2-second checks with no monsters before stopping" },
    };

    #endregion Properties

    #region Methods

    public bool Execute(string[] arguments = null)
    {
        if (arguments == null || arguments.Length != 4)
        {
            Log.Warn("[Script] attack: usage: attack X Y Radius Retries");
            return false;
        }

        if (!float.TryParse(arguments[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x)
            || !float.TryParse(arguments[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y)
            || !int.TryParse(arguments[2], out var radius)
            || !int.TryParse(arguments[3], out var retries)
            || retries < 1)
        {
            Log.Warn("[Script] attack: invalid arguments.");
            return false;
        }

        var localCts = new CancellationTokenSource();
        _cts = localCts;
        Task tickTask = null;

        try
        {
            IsBusy = true;

            // Configure training area (fires OnSetTrainingArea synchronously → Botbase.Reload())
            var pos = new Position(x, y);
            PlayerConfig.Set("RSBot.Area.Region", pos.Region);
            PlayerConfig.Set("RSBot.Area.X", pos.XOffset);
            PlayerConfig.Set("RSBot.Area.Y", pos.YOffset);
            PlayerConfig.Set("RSBot.Area.Z", Game.Player?.Position.ZOffset ?? PlayerConfig.Get<float>("RSBot.Area.Z"));
            PlayerConfig.Set("RSBot.Area.Radius", radius);
            EventManager.FireEvent("OnSetTrainingArea");

            Log.Notify($"[Script] attack: Area ({x}, {y}) r={radius}. Starting training...");

            // Set Running directly so Botbase.Tick() works — bypasses Bot.Start() and avoids
            // OnStartBot/OnStopBot events and Bot.TokenSource churn between attack commands.
            Kernel.Bot.Running = true;
            Kernel.Bot.Botbase.Start();

            var token = localCts.Token;
            tickTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (Game.Ready && Kernel.Bot.Running)
                        Kernel.Bot.Botbase.Tick();

                    await Task.Delay(100);
                }
            });

            // Monitor: every 2s check for live monsters
            var failedChecks = 0;
            while (IsBusy)
            {
                Thread.Sleep(2000);

                if (!IsBusy)
                    break;

                if (SpawnManager.Any<SpawnedMonster>(m => m.HasHealth))
                {
                    if (failedChecks > 0)
                    {
                        Log.Debug("[Script] attack: Monsters detected, counter reset.");
                        failedChecks = 0;
                    }
                }
                else
                {
                    failedChecks++;
                    Log.Debug($"[Script] attack: No monsters ({failedChecks}/{retries}).");

                    if (failedChecks >= retries)
                    {
                        Log.Notify($"[Script] attack: No monsters after {retries} check(s). Stopping training.");
                        break;
                    }
                }
            }

            return true;
        }
        finally
        {
            // Set Running=false first so Tick() returns immediately on the next check
            Kernel.Bot.Running = false;
            localCts.Cancel();
            tickTask?.Wait(500);

            // Clean up botbase state (cancel ongoing action, stop bundles)
            Kernel.Bot.Botbase?.Stop();

            _cts = null;
            IsBusy = false;
        }
    }

    public void Stop()
    {
        IsBusy = false;
        _cts?.Cancel();
    }

    #endregion Methods
}
