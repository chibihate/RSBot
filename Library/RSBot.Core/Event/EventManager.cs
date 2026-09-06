using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RSBot.Core.Event;

public class EventManager
{
    private static readonly List<(string name, Delegate handler)> _listeners = new();
    private static readonly object _listenersLock = new();

    /// <summary>
    ///     Registers the event.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="handler">The handler.</param>
    public static void SubscribeEvent(string name, Delegate handler)
    {
        if (handler == null)
            return;

        lock (_listenersLock)
            _listeners.Add((name, handler));
    }

    /// <summary>
    ///     Registers the event.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="handler">The handler.</param>
    public static void SubscribeEvent(string name, Action handler)
    {
        if (handler == null)
            return;

        lock (_listenersLock)
            _listeners.Add((name, handler));
    }

    /// <summary>
    ///     Fires the event.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="parameters">The parameters.</param>
    public static void FireEvent(string name, params object[] parameters)
    {
        try
        {
            (string name, Delegate handler)[] targets;
            lock (_listenersLock)
                targets = _listeners
                    .Where(o => o.name == name && o.handler.Method.GetParameters().Length == parameters.Length)
                    .ToArray();

            foreach (var target in targets)
                if (Thread.CurrentThread.Name == "Network.PacketProcessor")
                    Task.Run(() => target.handler.DynamicInvoke(parameters));
                else
                    target.handler.DynamicInvoke(parameters);
        }
        catch (Exception e)
        {
            Log.Fatal(e);
        }
    }
}
