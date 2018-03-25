using System.Xml.Serialization;

namespace ChubbyQuokka.DayZ.Structures
{
    public sealed class ItemSettings
    {
        [XmlArrayItem("Category"), XmlElement("ItemID")]
        public PlayerItemResult[] ItemUseResults;

        [XmlArrayItem("Category")]
        public PlayerItemCategory[] ItemDrops;

        [XmlArrayItem("Category")]
        public PlayerItemCategory[] ItemSpawns;
    }

    public sealed class HumanitySettings
    {
        public ushort HumanityEffectID;
        public int DefaultHumanity;
        public bool HumanityResetOnDeath;

        public int HumanityOnZombieKill;
        public int HumanityOnMegaKill;

        [XmlArrayItem("Humanity")]
        public PlayerKillConditional[] KillConditionals;

        [XmlArrayItem("ItemID")]
        public HumanityItem[] HumanityUse;

        [XmlArrayItem("ItemID")]
        public HumanityItem[] HumanityAid;
    }

    public sealed class DatabaseSettings
    {
        public string Database;
        public string Username;
        public string Password;
        public string Address;
        public ushort Port;
        public string TableName;
    }

    public sealed class PlayerItemResult
    {
        [XmlAttribute]
        public ushort OriginalItem;

        [XmlText]
        public ushort ItemID;
    }

    public sealed class PlayerKillConditional
    {
        [XmlText]
        public int Humanity;

        [XmlAttribute]
        public int RangeMin;

        [XmlAttribute]
        public int RangeMax;
    }

    public sealed class PlayerItemCategory
    {
        [XmlAttribute]
        public byte MinAmt;

        [XmlAttribute]
        public byte MaxAmt;

        [XmlAttribute]
        public bool Unique;

        [XmlAttribute]
        public int MinHumanity;

        [XmlAttribute]
        public int MaxHumanity;

        [XmlElement("ItemID")]
        public PlayerItem[] Items;
    }

    public sealed class PlayerItem
    {
        [XmlAttribute]
        public ulong Weight;

        [XmlText]
        public byte ItemID;
    }

    public sealed class HumanityItem
    {
        [XmlAttribute]
        public int Humanity;

        [XmlText]
        public ushort ItemID;
    }
}
