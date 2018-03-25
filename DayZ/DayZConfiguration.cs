using System.Xml.Serialization;

using Rocket.API;

using ChubbyQuokka.DayZ.Structures;

namespace ChubbyQuokka.DayZ
{
    public sealed class DayZConfiguration : IRocketPluginConfiguration
    {
        [XmlElement("DatabaseSettings")]
        public DatabaseSettings databaseSettings;
        public static DatabaseSettings DatabaseSettings => DayZ.Instance.Configuration.Instance.databaseSettings;

        [XmlElement("HumanitySettings")]
        public HumanitySettings humanitySettings;
        public static HumanitySettings HumanitySettings => DayZ.Instance.Configuration.Instance.humanitySettings;

        [XmlElement("ItemSettings")]
        public ItemSettings itemSettings;
        public static ItemSettings ItemSettings => DayZ.Instance.Configuration.Instance.itemSettings;

        public void LoadDefaults()
        {
            databaseSettings = new DatabaseSettings
            {
                Address = "127.0.0.1",
                Database = "unturned",
                Password = "toor",
                Username = "root",
                Port = 3306,
                TableName = "dayz_humanity"
            };

            humanitySettings = new HumanitySettings
            {
                HumanityEffectID = 12500,
                DefaultHumanity = 100,
                HumanityResetOnDeath = true,
                HumanityOnZombieKill = 10,
                HumanityOnMegaKill = 250,
                KillConditionals = new PlayerKillConditional[]
                {
                    new PlayerKillConditional{ Humanity = -50, RangeMin = 0, RangeMax = 250 },
                    new PlayerKillConditional{ Humanity = 50, RangeMin = -251, RangeMax = -1 },
                    new PlayerKillConditional{ Humanity = -100, RangeMin = 251, RangeMax = int.MaxValue },
                    new PlayerKillConditional{ Humanity = 100, RangeMin = int.MinValue, RangeMax = -252 }
                },
                HumanityUse = new HumanityItem[]
                {
                    new HumanityItem{ ItemID = 514, Humanity = -50 }
                },
                HumanityAid = new HumanityItem[]
                {
                    new HumanityItem{ ItemID = 14, Humanity = 25 }
                }
            };
            itemSettings = new ItemSettings
            {
                ItemUseResults = new PlayerItemResult[]
                {
                    new PlayerItemResult { ItemID = 72, OriginalItem = 13, Chance = 1f }
                },
                ItemSpawns = new PlayerItemCategory[]
                {
                    new PlayerItemCategory
                    {
                        MinAmt = 1,
                        MaxAmt = 1,
                        Unique = true,
                        MaxHumanity = int.MaxValue,
                        MinHumanity = int.MinValue,
                        Items = new PlayerItem[]
                        {
                            new PlayerItem{ ItemID = 154, Weight = 1 },
                            new PlayerItem{ ItemID = 158, Weight = 1 },
                            new PlayerItem{ ItemID = 163, Weight = 1 },
                            new PlayerItem{ ItemID = 167, Weight = 1 },
                            new PlayerItem{ ItemID = 171, Weight = 1 },
                            new PlayerItem{ ItemID = 175, Weight = 1 },
                            new PlayerItem{ ItemID = 179, Weight = 1 },
                            new PlayerItem{ ItemID = 183, Weight = 1 }
                        }
                    },
                    new PlayerItemCategory
                    {
                        MinAmt = 1,
                        MaxAmt = 1,
                        Unique = true,
                        MaxHumanity = int.MaxValue,
                        MinHumanity = int.MinValue,
                        Items = new PlayerItem[]
                        {
                            new PlayerItem{ ItemID = 2, Weight = 1 }
                        }
                    },
                    new PlayerItemCategory
                    {
                        MinAmt = 1,
                        MaxAmt = 2,
                        Unique = false,
                        MaxHumanity = int.MaxValue,
                        MinHumanity = int.MinValue,
                        Items = new PlayerItem[]
                        {
                            new PlayerItem{ ItemID = 84, Weight = 5 },
                            new PlayerItem{ ItemID = 85, Weight = 2 },
                            new PlayerItem{ ItemID = 86, Weight = 1 }
                        }
                    },
                    new PlayerItemCategory
                    {
                        MinAmt = 1,
                        MaxAmt = 2,
                        Unique = false,
                        MaxHumanity = int.MaxValue,
                        MinHumanity = int.MinValue,
                        Items = new PlayerItem[]
                        {
                            new PlayerItem{ ItemID = 14, Weight = 1 }
                        }
                    }
                },
                ItemDrops = new PlayerItemCategory[]
                {
                    new PlayerItemCategory
                    {
                        MinAmt = 0,
                        MaxAmt = 2,
                        Unique = true,
                        MaxHumanity = int.MaxValue,
                        MinHumanity = int.MinValue,
                        Items = new PlayerItem[]
                        {
                            new PlayerItem { ItemID = 10, Weight = 1 }
                        }
                    },
                    new PlayerItemCategory
                    {
                        MinAmt = 0,
                        MaxAmt = 2,
                        Unique = true,
                        MaxHumanity = int.MaxValue,
                        MinHumanity = int.MinValue,
                        Items = new PlayerItem[]
                        {
                            new PlayerItem { ItemID = 11, Weight = 1 }
                        }
                    }
                }
            };
        }
    }
}
