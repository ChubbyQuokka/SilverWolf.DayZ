using Rocket.Unturned.Events;
using Rocket.Unturned.Player;

using SDG.Unturned;

using UnityEngine;

namespace ChubbyQuokka.DayZ.Managers
{
    internal static class EventManager
    {
        public static void Initialize()
        {
            UnturnedPlayerEvents.OnPlayerRevive += OnPlayerRevive;
            UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
        }

        public static void Destroy()
        {
            UnturnedPlayerEvents.OnPlayerRevive -= OnPlayerRevive;
            UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;
        }

        static void OnPlayerRevive(UnturnedPlayer player, Vector3 position, byte angle)
        {

        }

        static void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, Steamworks.CSteamID murderer)
        {

        }
    }
}
