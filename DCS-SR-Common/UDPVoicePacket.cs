using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Ciribob.DCS.SimpleRadio.Standalone.Common
{
    [ProtoContract]
    public class UDPVoicePacket
    {
        [ProtoMember(1)]
        public byte[] AudioPart1 { get; set; }

        [ProtoMember(2)]
        public byte[] AudioPart2 { get; set; }

        [ProtoMember(3)]
        public double Frequency { get; set; }

        [ProtoMember(4)]
        public byte Modulation { get; set; }

        [ProtoMember(5)]
        public byte Encryption { get; set; }

        [ProtoMember(6)]
        public uint UnitId { get; set; }

        [ProtoMember(7)]
        public string ClientGUID { get; set; }

        [ProtoMember(8)]
        public bool Intercom { get; set; }

        [ProtoMember(9)]
        public DCSPoint DcsPoint { get; set; }

        public static UDPVoicePacket DeserialisePacket(byte[] message)
        {
            UDPVoicePacket result;
            using (var stream = new MemoryStream(message))
            {
                result = Serializer.Deserialize<UDPVoicePacket>(stream);
            }
            return result;
        }

        public byte[] SerialisePacket()
        {
            byte[] udpPacketBytes;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, this);
                udpPacketBytes = ms.ToArray();
            }
            return udpPacketBytes;
        }
    }

    [ProtoContract]
    public class DCSPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
