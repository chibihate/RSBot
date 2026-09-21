using RSBot.Core.Event;
using RSBot.Core.Objects;

namespace RSBot.Core.Network.Handler.Agent.Action;

internal class ActionCommandStateResponse : IPacketHandler
{
    /// <summary>
    ///     Invokes the specified packet.
    /// </summary>
    /// <param name="packet">The packet.</param>
    public void Invoke(Packet packet)
    {
        var state = packet.ReadByte();
        var recurring = packet.ReadByte();
        Position playerPosition = Game.Player.Position;
        Log.Debug($"[Script] Player position: {playerPosition.Region}({playerPosition.Region.X},{playerPosition.Region.Y}) X={playerPosition.X}, Y={playerPosition.Y} [XOff={playerPosition.XOffset:F1}, YOff={playerPosition.YOffset:F1}]");

        switch (state)
        {
            case 0x01:
                Game.Player.InAction = true;
                Log.Debug("Player has entered in action!");
                EventManager.FireEvent("OnPlayerInAction");
                break;

            case 0x02:
                Game.Player.InAction = recurring != 0;
                Log.Debug("Player has exited in action!");
                EventManager.FireEvent("OnPlayerExitAction");
                break;
        }
    }

    #region Properites

    /// <summary>
    ///     Gets or sets the opcode.
    /// </summary>
    /// <value>
    ///     The opcode.
    /// </value>
    public ushort Opcode => 0xB074;

    /// <summary>
    ///     Gets or sets the destination.
    /// </summary>
    /// <value>
    ///     The destination.
    /// </value>
    public PacketDestination Destination => PacketDestination.Client;

    #endregion Properites
}
