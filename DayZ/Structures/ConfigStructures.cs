using System.Xml.Serialization;

namespace ChubbyQuokka.DayZ.Structures
{
    public sealed class PlayerItemCategory
    {
        [XmlAttribute]
        public byte MinAmt;

        [XmlAttribute]
        public byte MaxAmt;

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
}
