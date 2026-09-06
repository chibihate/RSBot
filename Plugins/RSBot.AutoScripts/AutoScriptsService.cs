using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Event;

namespace RSBot.AutoScripts;

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

    public void Start()
    {
        if (_running) return;

        _running = true;
        _currentIndex = 0;
        _currentLoop = 0;
        _thread = new Thread(Loop) { IsBackground = true, Name = "AutoScriptThread" };
        _thread.Start();
        Log.Notify("[AutoScripts] Started.");
    }

    public void Stop()
    {
        _running = false;
        ScriptManager.Stop();
        Log.Notify("[AutoScripts] Stopped.");
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
                    Log.Warn("[AutoScripts] No scripts configured.");
                    break;
                }

                // Run all scripts for this loop iteration
                _currentIndex = 0;
                while (_running && _currentIndex < ScriptPaths.Count)
                {
                    var path = ScriptPaths[_currentIndex];

                    if (!File.Exists(path))
                    {
                        Log.Warn($"[AutoScripts] Script not found, skipping: {path}");
                    }
                    else
                    {
                        Log.Notify($"[AutoScripts] [{LoopLabel}] Script {_currentIndex + 1}/{ScriptPaths.Count}: {Path.GetFileName(path)}");
                        EventManager.FireEvent("OnAutoScriptRunning", _currentIndex);

                        ScriptManager.Load(path);
                        ScriptManager.RunScript(ignoreBotRunning: true);
                    }

                    if (!_running) return;
                    _currentIndex++;
                }

                if (!_running) return;

                _currentLoop++;

                // Check if we've completed all loops
                if (LoopCount > 0 && _currentLoop >= LoopCount)
                {
                    Log.Notify($"[AutoScripts] Completed {_currentLoop} loop(s).");
                    break;
                }

                // Delay between loops
                if (LoopDelay > 0)
                {
                    var loopStr = LoopCount == 0 ? "inf" : LoopCount.ToString();
                    Log.Notify($"[AutoScripts] Loop {_currentLoop}/{loopStr} done. Waiting {LoopDelay / 1000}s before next loop...");
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
                    Log.Notify($"[AutoScripts] Loop {_currentLoop}/{loopStr} done. Starting next loop...");
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
            EventManager.FireEvent("OnAutoScriptsStopped");
        }
    }

    private string LoopLabel => LoopCount == 0
        ? $"Loop {_currentLoop + 1}/inf"
        : $"Loop {_currentLoop + 1}/{LoopCount}";
}
