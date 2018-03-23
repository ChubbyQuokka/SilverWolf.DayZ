using System;
using System.Linq;
using System.Collections.Generic;

using ChubbyQuokka.DayZ.Structures;

using MySql.Data.MySqlClient;

namespace ChubbyQuokka.DayZ.Managers
{
    internal static class MySQLManager
    {
        static DatabaseSettings Settings => DayZConfiguration.DatabaseSettings;

        static MySqlConnection MainConnection;
        static MySqlConnection WorkerConnection;

        public static void Initialize()
        {
            MainConnection = new MySqlConnection(Queries.Connection);
            WorkerConnection = new MySqlConnection(Queries.Connection);

            var cmd1 = CreateCommand();
            cmd1.CommandText = Queries.ShowTables;

            OpenConnection();
            var obj = cmd1.ExecuteScalar();
            CloseConnection();

            if (obj == null)
            {
                var cmd2 = CreateCommand();
                cmd2.CommandText = Queries.CreateTable;

                OpenConnection();
                cmd2.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public static void Destroy()
        {
            MainConnection?.Dispose();
            WorkerConnection?.Dispose();

            MainConnection = null;
            WorkerConnection = null;
        }

        public static List<PlayerHumanity> Refresh()
        {
            List<PlayerHumanity> players = new List<PlayerHumanity>();

            var cmd1 = CreateCommand();
            cmd1.CommandText = Queries.SelectAll;

            OpenConnection();

            var dr1 = cmd1.ExecuteReader();

            while (dr1.Read())
            {
                players.Add(new PlayerHumanity
                {
                    SteamID = dr1.GetUInt64(0),
                    Name = dr1.GetString(1),
                    Humanity = dr1.GetInt32(2)
                });
            }

            dr1.Close();
            dr1.Dispose();

            CloseConnection();

            return players;
        }

        public static void SetPlayerHumanity(ulong steamId, int humanity)
        {
            var cmd1 = CreateCommand();
            cmd1.CommandText = Queries.UpdatePlayerHumanity(steamId, humanity);

            OpenConnection();
            cmd1.ExecuteNonQuery();
            CloseConnection();
        }

        public static void SetPlayerName(ulong steamId, string name)
        {
            var cmd1 = CreateCommand();
            cmd1.CommandText = Queries.UpdatePlayerName(steamId, name);

            OpenConnection();
            cmd1.ExecuteNonQuery();
            CloseConnection();
        }

        public static void InsertFirstTimePlayer(ulong steamId, string name)
        {
            var cmd1 = CreateCommand();
            cmd1.CommandText = Queries.InsertPlayerFirstTime(steamId, name, DayZConfiguration.HumanitySettings.DefaultHumanity);

            OpenConnection();
            cmd1.ExecuteNonQuery();
            CloseConnection();
        }

        static void OpenConnection()
        {
            if (ThreadManager.IsWorkerThread)
            {
                WorkerConnection.Open();
            }
            else
            {
                MainConnection.Open();
            }
        }

        static void CloseConnection()
        {
            if (ThreadManager.IsWorkerThread)
            {
                WorkerConnection.Close();
            }
            else
            {
                MainConnection.Close();
            }
        }

        static MySqlCommand CreateCommand()
        {
            if (ThreadManager.IsWorkerThread)
            {
                return WorkerConnection.CreateCommand();
            }

            return MainConnection.CreateCommand();
        }

        static class Queries
        {
            public static string Connection => $"SERVER={Settings.Address};DATABASE={Settings.Database};UID={Settings.Username};PASSWORD={Settings.Password};PORT={Settings.Port};";

            public static string CreateTable => $"CREATE TABLE `{Settings.TableName}` (`csteamid` BIGINT NOT NULL UNIQUE, `name` VARCHAR(64) NOT NULL, `humanity` INTEGER NOT NULL, PRIMARY KEY (`steamid`))";

            public static string ShowTables => $"SHOW TABLES LIKE '{Settings.TableName}'";

            public static string InsertPlayerFirstTime(ulong player, string name, int humanity) => $"INSERT INTO `{Settings.TableName}` VALUES ('{player}', '{name}', '{humanity}')";

            public static string SelectAll => $"SELECT * FROM `{Settings.TableName}`";

            public static string UpdatePlayerHumanity(ulong player, int humanity) => $"UPDATE `{Settings.TableName}` SET `humanity` = '{humanity}' WHERE `csteamid` = '{player}'";

            public static string UpdatePlayerName(ulong player, string name) => $"UPDATE `{Settings.TableName}` SET `name` = '{name}' WHERE `csteamid` = '{player}'";
        }
    }
}
