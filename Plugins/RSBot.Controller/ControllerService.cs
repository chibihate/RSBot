using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RSBot.Controller.Models;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Event;

namespace RSBot.Controller;

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
        EventManager.SubscribeEvent("OnAutoScriptsStopped", OnAutoScriptsStopped);
    }

    public void Start()
    {
        if (IsRunning) return;

        // Reset statuses for accounts not yet done
        foreach (var a in Accounts.Where(a => a.Status != AccountStatus.Done))
            a.Status = AccountStatus.Pending;

        CurrentIndex = 0;
        _cts = new CancellationTokenSource();
        _thread = new Thread(() => Loop(_cts.Token)) { IsBackground = true, Name = "ControllerThread" };
        _thread.Start();
    }

    public void Stop()
    {
        _cts?.Cancel();
        _characterLoaded.Set();
        _botStopped.Set();
        Log.Notify("[Controller] Stop requested.");
    }

    private void OnLoadCharacter()
    {
        if (_state == State.WaitingForLogin)
            _characterLoaded.Set();
    }

    private void OnBotStopped()
    {
        if (_state == State.BotRunning)
            _botStopped.Set();
    }

    private void OnAutoScriptsStopped()
    {
        if (_state == State.BotRunning)
            _botStopped.Set();
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

                Log.Notify($"[Controller] Account '{entry.Username}' ({CurrentIndex + 1}/{Accounts.Count})");

                RunForAccount(entry, ct);

                if (ct.IsCancellationRequested)
                    break;

                CurrentIndex++;
            }

            if (!ct.IsCancellationRequested)
                Log.Notify("[Controller] All accounts completed.");
        }
        catch (OperationCanceledException)
        {
            Log.Notify("[Controller] Stopped.");
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
            // Step 1: Select account + request single login mode
            SelectAccount(entry.Username);
            EventManager.FireEvent("OnRequestSingleLoginMode");

            // Step 2: Start client
            _characterLoaded.Reset();
            _state = State.WaitingForLogin;

            Log.Notify("[Controller] Starting client...");
            Game.Start();
            ClientManager.Start().GetAwaiter().GetResult();

            // Step 3: Wait for character to load (5 min timeout)
            Log.Notify("[Controller] Waiting for character to enter game...");
            if (!_characterLoaded.Wait(TimeSpan.FromMinutes(5), ct))
            {
                Log.Warn("[Controller] Timeout waiting for login. Skipping account.");
                entry.Status = AccountStatus.Error;
                Cleanup(ct);
                return;
            }

            // Step 4: Inject shared scripts into account's PlayerConfig, then wait delay
            _state = State.WaitingToStartBot;
            if (ScriptPaths.Count > 0)
            {
                PlayerConfig.Set("RSBot.AutoScript.Enabled", true);
                PlayerConfig.Set("RSBot.AutoScript.Scripts", string.Join(";", ScriptPaths));
                PlayerConfig.Save();
                Log.Notify($"[Controller] Injected {ScriptPaths.Count} script(s) into PlayerConfig.");
            }

            Log.Notify($"[Controller] Waiting {DelaySeconds}s before starting bot...");
            WaitSeconds(DelaySeconds, ct);
            ct.ThrowIfCancellationRequested();

            // Step 5: Start AutoScripts
            _state = State.BotRunning;
            _botStopped.Reset();
            Log.Notify("[Controller] Starting AutoScripts...");
            EventManager.FireEvent("OnAutoScriptsStart");

            // Step 6: Wait for scripts to finish (no timeout — runs until done)
            Log.Notify("[Controller] AutoScripts running. Waiting for completion...");
            while (!_botStopped.IsSet && !ct.IsCancellationRequested)
                _botStopped.Wait(TimeSpan.FromSeconds(1), ct);

            ct.ThrowIfCancellationRequested();

            entry.Status = AccountStatus.Done;
            FireRefresh();

            // Step 7: Kill client and wait before next account
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
            Log.Error($"[Controller] Error on account '{entry.Username}': {ex.Message}");
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

    // Reads account usernames from RSBot.General via reflection (no hard reference needed)
    public static IEnumerable<string> GetAvailableAccounts()
    {
        try
        {
            var asm = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "RSBot.General");
            if (asm == null)
            {
                Log.Warn("[Controller] RSBot.General assembly not found in AppDomain.");
                return Enumerable.Empty<string>();
            }

            var type = asm.GetType("RSBot.General.Components.Accounts");
            if (type == null)
            {
                Log.Warn("[Controller] RSBot.General.Components.Accounts type not found.");
                return Enumerable.Empty<string>();
            }

            var prop = type.GetProperty("SavedAccounts",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (prop == null)
            {
                Log.Warn("[Controller] SavedAccounts property not found on Accounts type.");
                return Enumerable.Empty<string>();
            }

            var value = prop.GetValue(null);
            if (value is not System.Collections.IEnumerable list)
            {
                Log.Warn($"[Controller] SavedAccounts is null or not IEnumerable (value={value}).");
                return Enumerable.Empty<string>();
            }

            var result = new List<string>();
            foreach (var account in list)
            {
                var usernameProp = account.GetType().GetProperty("Username");
                if (usernameProp?.GetValue(account) is string username && !string.IsNullOrEmpty(username))
                    result.Add(username);
            }

            Log.Notify($"[Controller] Loaded {result.Count} account(s) from RSBot.General.");
            return result;
        }
        catch (Exception ex)
        {
            Log.Error($"[Controller] GetAvailableAccounts failed: {ex.Message}");
            return Enumerable.Empty<string>();
        }
    }
}
