using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using RSBot.Repetition.Models;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Event;

namespace RSBot.Repetition;

internal class ControllerService
{
    private enum State { Idle, WaitingForLogin, WaitingToStartBot, BotRunning, Cleanup }

    private Thread _thread;
    private CancellationTokenSource _cts;
    private volatile State _state = State.Idle;
    private readonly ManualResetEventSlim _characterLoaded = new(false);
    private readonly ManualResetEventSlim _botStopped = new(false);

    public List<AccountEntry> Accounts { get; } = new();
    public List<string> ScriptPaths { get; } = new();
    public int DelaySeconds { get; set; } = 10;
    public int CurrentIndex { get; private set; }
    public bool IsRunning => _thread?.IsAlive == true;

    public void Initialize()
    {
        EventManager.SubscribeEvent("OnLoadCharacter", OnLoadCharacter);
        EventManager.SubscribeEvent("OnStopBot", OnBotStopped);
    }

    public void Start()
    {
        if (IsRunning) return;

        foreach (var a in Accounts.Where(a => a.Status != AccountStatus.Done))
            a.Status = AccountStatus.Pending;

        CurrentIndex = 0;
        _cts = new CancellationTokenSource();
        _thread = new Thread(() => Loop(_cts.Token)) { IsBackground = true, Name = "RepetitionThread" };
        _thread.Start();
    }

    public void Stop()
    {
        _cts?.Cancel();
        _characterLoaded.Set();
        _botStopped.Set();
        ScriptManager.Stop();
        Log.Notify("[Repetition] Stop requested.");
    }

    private void OnLoadCharacter()
    {
        if (_state == State.WaitingForLogin)
            _characterLoaded.Set();
    }

    private void OnBotStopped()
    {
        if (_state == State.BotRunning && !ScriptManager.Running)
        {
            _botStopped.Set();
            ScriptManager.Stop();
        }
    }

    private void Loop(CancellationToken ct)
    {
        try
        {
            while (CurrentIndex < Accounts.Count && !ct.IsCancellationRequested)
            {
                var entry = Accounts[CurrentIndex];
                entry.Status = AccountStatus.Running;
                FireRefresh();

                Log.Notify($"[Repetition] Account '{entry.Username}' ({CurrentIndex + 1}/{Accounts.Count})");

                RunForAccount(entry, ct);

                if (ct.IsCancellationRequested)
                    break;

                CurrentIndex++;
            }

            if (!ct.IsCancellationRequested)
                Log.Notify("[Repetition] All accounts completed.");
        }
        catch (OperationCanceledException)
        {
            Log.Notify("[Repetition] Stopped.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex);
        }
        finally
        {
            _state = State.Idle;
            FireRefresh();
        }
    }

    private void RunForAccount(AccountEntry entry, CancellationToken ct)
    {
        try
        {
            SelectAccount(entry.Username);
            EventManager.FireEvent("OnRequestSingleLoginMode");

            _characterLoaded.Reset();
            _state = State.WaitingForLogin;

            Log.Notify("[Repetition] Starting client...");
            Game.Start();
            ClientManager.Start().GetAwaiter().GetResult();

            Log.Notify("[Repetition] Waiting for character to enter game...");
            if (!_characterLoaded.Wait(TimeSpan.FromMinutes(5), ct))
            {
                Log.Warn("[Repetition] Timeout waiting for login. Skipping account.");
                entry.Status = AccountStatus.Error;
                Cleanup(ct);
                return;
            }

            _state = State.WaitingToStartBot;
            Log.Notify($"[Repetition] Waiting {DelaySeconds}s before starting bot...");
            WaitSeconds(DelaySeconds, ct);
            ct.ThrowIfCancellationRequested();

            _state = State.BotRunning;
            _botStopped.Reset();
            Log.Notify($"[Repetition] Running {ScriptPaths.Count} script(s)...");

            foreach (var path in ScriptPaths)
            {
                if (_botStopped.IsSet || ct.IsCancellationRequested)
                    break;

                if (!File.Exists(path))
                {
                    Log.Warn($"[Repetition] Script not found, skipping: {Path.GetFileName(path)}");
                    continue;
                }

                Log.Notify($"[Repetition] Running script: {Path.GetFileName(path)}");
                ScriptManager.Load(path);
                ScriptManager.RunScript(ignoreBotRunning: true);
            }

            ct.ThrowIfCancellationRequested();

            entry.Status = AccountStatus.Done;
            FireRefresh();

            Cleanup(ct);
        }
        catch (OperationCanceledException)
        {
            entry.Status = AccountStatus.Error;
            FireRefresh();
            throw;
        }
        catch (Exception ex)
        {
            Log.Error($"[Repetition] Error on account '{entry.Username}': {ex.Message}");
            entry.Status = AccountStatus.Error;
            FireRefresh();
            Cleanup(ct);
        }
    }

    private void Cleanup(CancellationToken ct)
    {
        _state = State.Cleanup;
        try
        {
            EventManager.FireEvent("OnAutoScriptsStop");
            ClientManager.Kill();
            WaitSeconds(3, ct);
        }
        catch (OperationCanceledException) { throw; }
        catch { /* ignore cleanup errors */ }
    }

    private static void SelectAccount(string username)
    {
        GlobalConfig.Set("RSBot.General.AutoLoginAccountUsername", username);
        GlobalConfig.Save();
    }

    private static void WaitSeconds(int seconds, CancellationToken ct)
    {
        for (var i = 0; i < seconds * 10; i++)
        {
            ct.ThrowIfCancellationRequested();
            Thread.Sleep(100);
        }
    }

    private void FireRefresh() => EventManager.FireEvent("OnControllerRefresh");

    public static IEnumerable<string> GetAvailableAccounts()
    {
        try
        {
            var asm = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "RSBot.General");
            if (asm == null)
            {
                Log.Warn("[Repetition] RSBot.General assembly not found in AppDomain.");
                return Enumerable.Empty<string>();
            }

            var type = asm.GetType("RSBot.General.Components.Accounts");
            if (type == null)
            {
                Log.Warn("[Repetition] RSBot.General.Components.Accounts type not found.");
                return Enumerable.Empty<string>();
            }

            var prop = type.GetProperty("SavedAccounts",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (prop == null)
            {
                Log.Warn("[Repetition] SavedAccounts property not found on Accounts type.");
                return Enumerable.Empty<string>();
            }

            var value = prop.GetValue(null);
            if (value is not System.Collections.IEnumerable list)
            {
                Log.Warn($"[Repetition] SavedAccounts is null or not IEnumerable (value={value}).");
                return Enumerable.Empty<string>();
            }

            var result = new List<string>();
            foreach (var account in list)
            {
                var usernameProp = account.GetType().GetProperty("Username");
                if (usernameProp?.GetValue(account) is string username && !string.IsNullOrEmpty(username))
                    result.Add(username);
            }

            Log.Notify($"[Repetition] Loaded {result.Count} account(s) from RSBot.General.");
            return result;
        }
        catch (Exception ex)
        {
            Log.Error($"[Repetition] GetAvailableAccounts failed: {ex.Message}");
            return Enumerable.Empty<string>();
        }
    }
}
