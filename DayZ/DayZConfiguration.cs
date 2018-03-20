using System.Xml.Serialization;

using Rocket.API;

using ChubbyQuokka.DayZ.Structures;

namespace ChubbyQuokka.DayZ
{
    public sealed class DayZConfiguration : IRocketPluginConfiguration
    {
        [XmlArray("ItemDrops"), XmlArrayItem("Category")]
        public PlayerItemCategory[] itemDrops;
        public static PlayerItemCategory[] ItemDrops => DayZ.Instance.Configuration.Instance.itemDrops;

        [XmlArray("ItemSpawns"), XmlArrayItem("Category")]
        public PlayerItemCategory[] itemSpawns;
        public static PlayerItemCategory[] ItemSpawns => DayZ.Instance.Configuration.Instance.itemSpawns;

        public void LoadDefaults()
        {
            itemSpawns = new PlayerItemCategory[]
            {
                new PlayerItemCategory
                {
                    MinAmt = 1,
                    MaxAmt = 1,
                    Unique = true,
                    Items = new PlayerItem[]
                    {
                        new PlayerItem{ ItemID = 154, Weight = 1},
                        new PlayerItem{ ItemID = 158, Weight = 1},
                        new PlayerItem{ ItemID = 163, Weight = 1},
                        new PlayerItem{ ItemID = 167, Weight = 1},
                        new PlayerItem{ ItemID = 171, Weight = 1},
                        new PlayerItem{ ItemID = 175, Weight = 1},
                        new PlayerItem{ ItemID = 179, Weight = 1},
                        new PlayerItem{ ItemID = 183, Weight = 1}
                    }
                },
                new PlayerItemCategory
                {
                    MinAmt = 1,
                    MaxAmt = 1,
                    Unique = true,
                    Items = new PlayerItem[]
                    {
                        new PlayerItem{ ItemID = 2, Weight = 1}
                    }
                },
                new PlayerItemCategory
                {
                    MinAmt = 1,
                    MaxAmt = 2,
                    Unique = false,
                    Items = new PlayerItem[]
                    {
                        new PlayerItem{ ItemID = 84, Weight = 5},
                        new PlayerItem{ ItemID = 85, Weight = 2},
                        new PlayerItem{ ItemID = 86, Weight = 1}
                    }
                },
                new PlayerItemCategory
                {
                    MinAmt = 1,
                    MaxAmt = 2,
                    Unique = false,
                    Items = new PlayerItem[]
                    {
                        new PlayerItem{ ItemID = 14, Weight = 1}
                    }
                }
            };

            itemDrops = new PlayerItemCategory[]
            {
                new PlayerItemCategory
                {
                    MinAmt = 0,
                    MaxAmt = 2,
                    Unique = true,
                    Items = new PlayerItem[]
                    {
                        new PlayerItem { ItemID = 10, Weight = 1}
                    }
                },
                new PlayerItemCategory
                {
                    MinAmt = 0,
                    MaxAmt = 2,
                    Unique = true,
                    Items = new PlayerItem[]
                    {
                        new PlayerItem { ItemID = 11, Weight = 1}
                    }
                },
            };
        }
    }
}
