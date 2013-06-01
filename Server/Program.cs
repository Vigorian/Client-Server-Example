using Core;
using Core.Packets;
using Core.Packets.ClientPackets;
using Core.Packets.ServerPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socket_Server
{
    class Program
    {
        static Server Server { get; set; }

        static void Main(string[] args)
        {
            //Initialize the Server with an 8kb buffer.
            Server = new Server(8192);

            //Add packet types so the serializer can understand and properly convert them.
            Server.AddTypesToSerializer(typeof(IPacket), new Type[]
            {
                typeof(Initialize),
                typeof(InitializeCommand),
                typeof(Message)
            });

            //Set up the Client/Server events.
            Server.ServerState += ServerState;
            Server.ClientState += ClientState;
            Server.ClientRead += ClientRead;

            //And finally, listen on port 31.
            Server.Listen(31);

            while (true)
                Console.ReadLine();
        }

        static void ServerState(Server server, bool listening)
        {
            Console.WriteLine("listening: " + listening);
        }

        static void ClientState(Server server, Client client, bool connected)
        {
            if (connected)
            {
                client.Value = new UserState(); //Initialize the UserState so we can store values in there if we need to.

                new InitializeCommand().Execute(client); //Tell the Client to initialize and send us information.
            }
        }

        static void ClientRead(Server server, Client client, IPacket packet)
        {
            Type type = packet.GetType(); //Get the packet type so we can determine what to do with it.

            if (type == typeof(Initialize))
            {
                HandleInitialize((Initialize)packet); //Initialize packet, handle it.
            }
            else if (type == typeof(Message))
            {
                HandleMessage((Message)packet); //Message packet, handle it.
            }

        }

        static void HandleInitialize(Initialize packet)
        {
            Console.WriteLine("initialize: " + packet.Message);
        }

        static void HandleMessage(Message message)
        {
            Console.WriteLine("id: {0}; message: {1}", message.ID, message.Text);
        }
    }
}
