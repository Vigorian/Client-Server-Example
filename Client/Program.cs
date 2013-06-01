using Core;
using Core.Packets;
using Core.Packets.ClientPackets;
using Core.Packets.ServerPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socket_Client
{
    class Program
    {
        static Client Client { get; set; }

        static void Main(string[] args)
        {
            //Initialize the client with a 8kb receive/send buffer.
            Client = new Client(8192);

            //Add the packets to the serializer so we can properly send and receive messages.
            Client.AddTypesToSerializer(typeof(IPacket), new Type[]
            {
                typeof(Initialize),
                typeof(InitializeCommand),
                typeof(Message)
            });

            //Set up the events.
            Client.ClientState += ClientState;
            Client.ClientRead += ClientRead;

            //And finally, connect.
            Client.Connect("localhost", 31);

            while (true)
            {
                string message = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(message))
                    new Message(new Random().Next(int.MinValue, int.MaxValue), message).Execute(Client); //Last line wasn't empty, create a new Message with a random ID and message.
            }
        }

        static void ClientState(Client client, bool connected)
        {
            Console.WriteLine("{0}connected", connected ? "" : "dis");
        }

        static void ClientRead(Client client, IPacket packet)
        {
            Type type = packet.GetType();

            if (type == typeof(InitializeCommand)) //Server wants us to initialize, let's do this. LEEEERRROOOOOYYYYYYY
            {
                HandleInitializeCommand((InitializeCommand)packet, client);
            }

        }

        static void HandleInitializeCommand(InitializeCommand command, Client client)
        {
            new Initialize("hello!").Execute(client); //Initialize the Client.
        }
    }
}
