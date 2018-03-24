using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChubbyQuokka.DayZ.Structures;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace ChubbyQuokka.DayZ.Managers
{
    internal static class HumanityManager
    {
        static List<PlayerHumanity> players = new List<PlayerHumanity>();

        public static void Initialize()
        {
            Refresh();
        }

        public static void Destroy()
        {
            players.Clear();
        }

        public static void Refresh()
        {
            List<PlayerHumanity> newPlayers = MySQLManager.Refresh();

            foreach (SteamPlayer client in Provider.clients)
            {
                PlayerHumanity newHumanity = newPlayers.FirstOrDefault(x => x.SteamID == client.playerID.steamID.m_SteamID);
                PlayerHumanity playerHumanity = players.FirstOrDefault(x => x.SteamID == client.playerID.steamID.m_SteamID);

                if (newHumanity.Humanity != playerHumanity.Humanity)
                {
                    SendNewUI(client.playerID.steamID, newHumanity.Humanity);
                }
            }

            players = newPlayers;
        }

        //CALLED ON EITHER
        public static void IncrementHumanity(ulong steamId, int humanityIncrement)
        {
            if (humanityIncrement != 0)
            {
                ThreadManager.ExecuteWorker(() =>
                {
                    int current = players.First(x => x.SteamID == steamId).Humanity;

                    current += humanityIncrement;

                    SendNewUI(new CSteamID(steamId), humanityIncrement);
                });
            }
        }

        //CALLED ON EITHER
        public static void AddPlayerFirstTime(ulong steamId, string name)
        {
            ThreadManager.ExecuteWorker(() =>
            {
                MySQLManager.InsertFirstTimePlayer(steamId, name);
                players.Add(new PlayerHumanity { SteamID = steamId, Name = name, Humanity = DayZConfiguration.HumanitySettings.DefaultHumanity });
            });
        }

        //CALLED ON WORKER THREAD
        public static bool PlayerExists(ulong steamId)
        {
            return players.FirstOrDefault(x => x.SteamID == steamId) != null;
        }

        //CALLED ON EITHER
        public static void SendNewUI(CSteamID id, int humanity)
        {
            ThreadManager.ExecuteMain(() =>
            {
                EffectManager.sendUIEffect(DayZConfiguration.HumanitySettings.HumanityEffectID, 25565, id, true, humanity.ToString());
            });
        }

        public static void CheckPlayerJoin(UnturnedPlayer p)
        {
            CSteamID id = p.CSteamID;
            string name = p.DisplayName;

            ThreadManager.ExecuteWorker(() => 
            {
                if (!PlayerExists(id.m_SteamID))
                {
                    AddPlayerFirstTime(id.m_SteamID, name);
                }

                //It shouldn't be possible for the First query to return a null object, but just in case. :D
                PlayerHumanity humanity = players.First(x => x.SteamID == id.m_SteamID) ?? default(PlayerHumanity);

                if (!humanity.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    humanity.Name = name;
                    MySQLManager.SetPlayerName(id.m_SteamID, name);
                }

                SendNewUI(id, humanity.Humanity);
            });
        }
    }
}
