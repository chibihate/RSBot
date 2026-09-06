using System.Collections.Generic;
using System.Threading;
using RSBot.Core.Network;

namespace RSBot.Core.Components.Scripting.Commands;

internal class LuckDrawScriptCommand : IScriptCommand
{
    #region Properties

    public string Name => "luckdraw";

    public bool IsBusy { get; private set; }

    public Dictionary<string, string> Arguments => new()
    {
        { "Runs", "Number of times to spin the luck draw" },
        { "Delay", "Delay in milliseconds between each spin" },
    };

    #endregion Properties

    #region Methods

    public bool Execute(string[] arguments = null)
    {
        if (arguments == null || arguments.Length < 2)
        {
            Log.Warn("[Script] Invalid luckdraw command: usage is 'luckdraw <runs> <delay>'.");
            return false;
        }

        if (!int.TryParse(arguments[0], out var runs) || runs < 1)
        {
            Log.Warn("[Script] luckdraw: 'runs' must be a positive integer.");
            return false;
        }

        if (!int.TryParse(arguments[1], out var delay) || delay < 0)
        {
            Log.Warn("[Script] luckdraw: 'delay' must be a non-negative integer (milliseconds).");
            return false;
        }

        try
        {
            IsBusy = true;
            Log.Notify($"[Script] Luck draw: {runs} spin(s) with {delay}ms delay.");

            for (var i = 0; i < runs && IsBusy; i++)
            {
                var packet = new Packet(0x189D);
                packet.WriteByte(0x01);
                PacketManager.SendPacket(packet, PacketDestination.Server);

                Log.Debug($"[Script] Luck draw spin {i + 1}/{runs}");

                if (i < runs - 1 && delay > 0)
                    Thread.Sleep(delay);
            }

            return true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void Stop()
    {
        IsBusy = false;
    }

    #endregion Methods
}
