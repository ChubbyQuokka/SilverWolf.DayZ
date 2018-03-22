using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChubbyQuokka.DayZ.Structures;

using MySql.Data.MySqlClient;

namespace ChubbyQuokka.DayZ.Managers
{
    internal static class MySQLManager
    {
        static DatabaseSettings Settings => DayZConfiguration.DatabaseSettings;

        static MySqlConnection MainConnection;
        static MySqlConnection WorkerConnection;

        static class Queries
        {
            public static string Connection => $"SERVER={Settings.Address};DATABASE={Settings.Database};UID={Settings.Username};PASSWORD={Settings.Password};PORT={Settings.Port};";
        }
    }
}
