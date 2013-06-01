using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Packets.ClientPackets
{
    [ProtoContract]
    public class Message : IPacket
    {
        [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public string Text { get; set; }

        public Message() { }
        public Message(int id, string message)
        {
            ID = id;
            Text = message;
        }

        public void Execute(Client client)
        {
            client.Send<Message>(this);
        }
    }
}
