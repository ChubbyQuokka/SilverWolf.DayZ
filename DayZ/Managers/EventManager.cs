using System.Linq;
using System.Collections.Generic;

using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;

using SDG.Unturned;

using UnityEngine;

using Steamworks;

using ChubbyQuokka.DayZ.Structures;

namespace ChubbyQuokka.DayZ.Managers
{
    internal static class EventManager
    {
        public static void Initialize()
        {
            UnturnedPlayerEvents.OnPlayerRevive += OnPlayerRevive;
            UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
            UnturnedPlayerEvents.OnPlayerUpdateStat += OnPlayerUpdateStat;

            U.Events.OnPlayerConnected += OnPlayerConnected;
        }

        public static void Destroy()
        {
            UnturnedPlayerEvents.OnPlayerRevive -= OnPlayerRevive;
            UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;
            UnturnedPlayerEvents.OnPlayerUpdateStat -= OnPlayerUpdateStat;

            U.Events.OnPlayerConnected -= OnPlayerConnected;
        }

        static void OnPlayerUpdateStat(UnturnedPlayer player, EPlayerStat stat)
        {
            if (stat == EPlayerStat.KILLS_ZOMBIES_NORMAL)
            {
                HumanityManager.IncrementHumanity(player.CSteamID.m_SteamID, DayZConfiguration.HumanitySettings.HumanityOnZombieKill);
            }
            else if (stat == EPlayerStat.KILLS_ZOMBIES_MEGA)
            {
                HumanityManager.IncrementHumanity(player.CSteamID.m_SteamID, DayZConfiguration.HumanitySettings.HumanityOnMegaKill);
            }
        }

        static void OnPlayerConnected(UnturnedPlayer player)
        {
            HumanityManager.CheckPlayerJoin(player);
        }

        static void OnPlayerRevive(UnturnedPlayer player, Vector3 position, byte angle)
        {
            int humanity = HumanityManager.GetPlayerHumanity(player.CSteamID.m_SteamID);

            List<PlayerItemCategory> categories = DayZConfiguration.ItemSettings.ItemSpawns.Where(x => x.MaxHumanity >= humanity && x.MinHumanity <= humanity).ToList();

            foreach (PlayerItemCategory category in categories)
            {
                ItemManager.GiveCategoryToPlayer(player, category);
            }
        }

        static void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            if (murderer != CSteamID.Nil)
            {
                int murdererHumanity = HumanityManager.GetPlayerHumanity(murderer.m_SteamID);

                PlayerKillConditional kill = DayZConfiguration.HumanitySettings.KillConditionals.FirstOrDefault(x => x.RangeMax >= murdererHumanity && x.RangeMin <= murdererHumanity);
            }

            int humanity = HumanityManager.GetPlayerHumanity(player.CSteamID.m_SteamID);

            List<PlayerItemCategory> categories = DayZConfiguration.ItemSettings.ItemDrops.Where(x => x.MaxHumanity >= humanity && x.MinHumanity <= humanity).ToList();

            foreach (PlayerItemCategory category in categories)
            {
                ItemManager.GiveCategoryToPlayer(player, category);
            }
        }
    }
}
