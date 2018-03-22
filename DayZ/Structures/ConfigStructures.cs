using System.Xml.Serialization;

namespace ChubbyQuokka.DayZ.Structures
{
    public sealed class PlayerItemCategory
    {
        [XmlAttribute]
        public byte MinAmt;

        [XmlAttribute]
        public byte MaxAmt;

        [XmlAttribute]
        public bool Unique;

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

    public sealed class DatabaseSettings
    {
        public string Database;
        public string Username;
        public string Password;
        public string Address;
        public ushort Port;
        public string TableName;
    }
}
