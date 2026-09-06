using System;
using RSBot.Core.Event;
using RSBot.Core.Objects.Spawn;

namespace RSBot.Core.Components
{
    public static class PlayerFollowService
    {
        private static bool _enabled;
        private static bool _subscribed;
        private static string _targetName;

        /// <summary>Follow distance threshold in game units. Default: 10.</summary>
        public static double FollowDistance { get; set; } = 10.0;

        public static bool Enabled => _enabled;

        /// <summary>Name of the player to follow. Case-sensitive.</summary>
        public static string TargetName
        {
            get => _targetName;
            set => _targetName = value;
        }

        public static void SetEnabled(bool enabled)
        {
            EnsureSubscribed();
            _enabled = enabled;
        }

        public static void SetTarget(string name)
        {
            _targetName = name;
        }

        private static void EnsureSubscribed()
        {
            if (_subscribed) return;
            EventManager.SubscribeEvent("OnTick", new Action(OnTick));
            _subscribed = true;
        }

        private static void OnTick()
        {
            if (!_enabled) return;
            if (!Game.Ready || Game.Player == null) return;
            if (string.IsNullOrWhiteSpace(_targetName)) return;
            if (Game.Player.InAction || Game.Player.Movement.Moving) return;

            var target = SpawnManager.GetEntity<SpawnedPlayer>(
                p => string.Equals(p.Name, _targetName, StringComparison.OrdinalIgnoreCase));

            if (target == null) return;

            if (target.Position.DistanceToPlayer() >= FollowDistance)
                Game.Player.MoveTo(target.Position, false);
        }
    }
}
