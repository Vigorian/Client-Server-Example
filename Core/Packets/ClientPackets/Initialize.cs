using ProtoBuf;
namespace Core.Packets.ClientPackets
{
    [ProtoContract]
    public class Initialize : IPacket
    {
        [ProtoMember(1)]
        public string Message { get; set; }

        public Initialize() { }
        public Initialize(string message)
        {
            Message = message;
        }

        public void Execute(Client client)
        {
            client.Send<Initialize>(this);
        }
    }
}
