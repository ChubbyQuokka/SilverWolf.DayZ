using System;
using System.Collections.Generic;
using System.Linq;

using ChubbyQuokka.DayZ.Structures;

using Rocket.Unturned.Player;

using SDG.Unturned;

using Steamworks;

namespace ChubbyQuokka.DayZ.Managers
{
    internal static class HumanityManager
    {
        static List<PlayerHumanity> players = new List<PlayerHumanity>();
        static object playerLock = new object();

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

            lock (playerLock)
            {
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
        }

        //CALLED ON EITHER
        public static void IncrementHumanity(ulong steamId, int humanityIncrement)
        {
            if (humanityIncrement != 0)
            {
                ThreadManager.ExecuteWorker(() =>
                {
                    lock (playerLock)
                    {
                        PlayerHumanity current = players.FirstOrDefault(x => x.SteamID == steamId);
                        
                        current.Humanity += humanityIncrement;

                        MySQLManager.SetPlayerHumanity(steamId, current.Humanity);

                        SendNewUI(new CSteamID(steamId), current.Humanity);
                    }
                });
            }
        }

        //CALLED ON EITHER
        public static void AddPlayerFirstTime(ulong steamId, string name)
        {
            ThreadManager.ExecuteWorker(() =>
            {
                lock (playerLock)
                {
                    MySQLManager.InsertFirstTimePlayer(steamId, name);
                    players.Add(new PlayerHumanity { SteamID = steamId, Name = name, Humanity = DayZConfiguration.HumanitySettings.DefaultHumanity });
                }
            });
        }

        //CALLED ON WORKER THREAD
        public static bool PlayerExists(ulong steamId)
        {
            lock (playerLock)
            {
                return players.FirstOrDefault(x => x.SteamID == steamId) != null;
            }
        }

        //CALLED ON EITHER
        public static void SendNewUI(CSteamID id, int humanity)
        {
            ThreadManager.ExecuteMain(() =>
            {
                EffectManager.sendUIEffect(DayZConfiguration.HumanitySettings.HumanityEffectID, 25565, id, true, humanity.ToString());
            });
        }

        public static int GetPlayerHumanity(ulong id)
        {
            lock (playerLock)
            {
                return players.FirstOrDefault(x => x.SteamID == id)?.Humanity ?? DayZConfiguration.HumanitySettings.DefaultHumanity;
            }
        }

        public static void CheckPlayerJoin(UnturnedPlayer p)
        {
            CSteamID id = p.CSteamID;
            string name = p.DisplayName;

            ThreadManager.ExecuteWorker(() => 
            {
                lock (playerLock)
                {
                    if (!PlayerExists(id.m_SteamID))
                    {
                        AddPlayerFirstTime(id.m_SteamID, name);
                    }

                    //It shouldn't be possible for the First query to return a null object, but just in case. :D
                    PlayerHumanity humanity = players.First(x => x.SteamID == id.m_SteamID)
                        ?? new PlayerHumanity
                        {
                            Name = p.DisplayName,
                            SteamID = id.m_SteamID,
                            Humanity = DayZConfiguration.HumanitySettings.DefaultHumanity
                        };

                    if (!humanity.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        humanity.Name = name;
                        MySQLManager.SetPlayerName(id.m_SteamID, name);
                    }

                    SendNewUI(id, humanity.Humanity);
                }
            });
        }
    }
}
