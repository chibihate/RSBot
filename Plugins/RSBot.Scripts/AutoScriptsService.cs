using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Event;

namespace RSBot.Scripts;

internal class AutoScriptsService
{
    private Thread _thread;
    private volatile bool _running;
    private volatile int _currentIndex;
    private volatile int _currentLoop;

    public List<string> ScriptPaths { get; } = new();
    public bool IsRunning => _running;

    /// <summary>Number of loops to run. 0 = infinite.</summary>
    public int LoopCount { get; set; } = 1;

    /// <summary>Delay in milliseconds between loops.</summary>
    public int LoopDelay { get; set; } = 0;

    /// <summary>Path to a .wav file played on normal completion. Empty = no sound.</summary>
    public string SoundFilePath { get; set; } = string.Empty;

    /// <summary>When true, fires the global OnAutoScriptsStopped event on completion (only for the main list service).</summary>
    public bool FireGlobalEvents { get; set; } = true;

    /// <summary>Called when the service finishes. Use for per-slot UI updates.</summary>
    public Action OnStopped { get; set; }

    public void Start()
    {
        if (_running) return;

        _running = true;
        _currentIndex = 0;
        _currentLoop = 0;
        _thread = new Thread(Loop) { IsBackground = true, Name = "AutoScriptThread" };
        _thread.Start();
        Log.Notify("[Scripts] Started.");
    }

    public void Stop()
    {
        _running = false;
        ScriptManager.Stop();
        Log.Notify("[Scripts] Stopped.");
    }

    private void Loop()
    {
        try
        {
            while (_running)
            {
                if (!Game.Ready || Game.Player == null)
                {
                    Thread.Sleep(500);
                    continue;
                }

                if (ScriptPaths.Count == 0)
                {
                    Log.Warn("[Scripts] No scripts configured.");
                    break;
                }

                _currentIndex = 0;
                while (_running && _currentIndex < ScriptPaths.Count)
                {
                    var path = ScriptPaths[_currentIndex];

                    if (!File.Exists(path))
                    {
                        Log.Warn($"[Scripts] Script not found, skipping: {path}");
                    }
                    else
                    {
                        Log.Notify($"[Scripts] [{LoopLabel}] Script {_currentIndex + 1}/{ScriptPaths.Count}: {Path.GetFileName(path)}");
                        EventManager.FireEvent("OnAutoScriptRunning", _currentIndex);

                        ScriptManager.Load(path);
                        ScriptManager.RunScript(ignoreBotRunning: true);
                    }

                    if (!_running) return;
                    _currentIndex++;
                }

                if (!_running) return;

                _currentLoop++;

                if (LoopCount > 0 && _currentLoop >= LoopCount)
                {
                    Log.Notify($"[Scripts] Completed {_currentLoop} loop(s).");
                    PlayCompletionSound();
                    break;
                }

                if (LoopDelay > 0)
                {
                    var loopStr = LoopCount == 0 ? "inf" : LoopCount.ToString();
                    Log.Notify($"[Scripts] Loop {_currentLoop}/{loopStr} done. Waiting {LoopDelay / 1000}s before next loop...");
                    var elapsed = 0;
                    while (_running && elapsed < LoopDelay)
                    {
                        Thread.Sleep(200);
                        elapsed += 200;
                    }
                }
                else
                {
                    var loopStr = LoopCount == 0 ? "inf" : LoopCount.ToString();
                    Log.Notify($"[Scripts] Loop {_currentLoop}/{loopStr} done. Starting next loop...");
                }
            }
        }
        catch (ThreadInterruptedException) { }
        catch (Exception ex)
        {
            Log.Fatal(ex);
        }
        finally
        {
            _running = false;
            if (FireGlobalEvents)
                EventManager.FireEvent("OnAutoScriptsStopped");
            OnStopped?.Invoke();
        }
    }

    private void PlayCompletionSound()
    {
        var path = SoundFilePath;
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return;

        try
        {
            using var player = new SoundPlayer(path);
            player.Play();
        }
        catch (Exception ex)
        {
            Log.Warn($"[Scripts] Could not play sound: {ex.Message}");
        }
    }

    private string LoopLabel => LoopCount == 0
        ? $"Loop {_currentLoop + 1}/inf"
        : $"Loop {_currentLoop + 1}/{LoopCount}";
}
