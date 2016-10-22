using System;

using Nano3.Net;
using Nano3.Net.Example;

namespace Neno3NetTestClient
{
    class Program
    {
        private static AsyncTCPNetClientExamp _client;

        static void Main(string[] args)
        {
            NetProtocol.Initialize(new Protocol_12());

            _client = new AsyncTCPNetClientExamp();

            _client.DisconnectEvent += OnDisconnected;
            _client.ConnectEvent += OnConnected;

            _client.Commands.Add(new ComGetClientID(OnUniqueIDWriter));
            _client.Commands.Add(new ComNewConnection(OnNewConnectionWriter));
            _client.Commands.Add(new ComDisconnection(OnDisconnectionWriter));
            _client.Commands.Add(new ComReceiveMessage(OnMessageReceive));

            _client.Connect(NetHelper.GetIPEndPoint("127.0.0.1", "20000"));

            string s = null;
            while (s != "Stop") {
                s = Console.ReadLine();
                if (s != "Stop") {
                    SendMessage(_client.ClientID, s);
                }
            }
        }

        private static void OnConnected(object sender, IConnector connector)
        {
            Console.WriteLine("Connected to: " + ((SocketConnector)connector).CSocket.RemoteEndPoint.ToString());
        }

        private static void OnDisconnected(object sender, object disconnectState)
        {
            Console.WriteLine("Server was stopped");
        }

        private static void OnUniqueIDWriter(IConnector connector, ArgUniqueID arg)
        {
            Console.WriteLine("My clientID: " + arg.UniqueID);
            _client.ClientID = arg.UniqueID;
        }

        private static void OnNewConnectionWriter(IConnector connector, ArgUniqueID arg)
        {
            Console.WriteLine("New client: " + arg.UniqueID + " was connected");
        }

        private static void OnDisconnectionWriter(IConnector connector, ArgUniqueID arg)
        {
            Console.WriteLine(arg.UniqueID + " was disconnected");
        }

        private static void OnMessageReceive(IConnector connector, ArgStringMessage arg)
        {
            if (connector == null) { return; }
            Console.WriteLine(arg.ClientID + " say: " + arg.Message);
        }

        private static void SendMessage(long clientID, string message)
        {
            if (_client != null && _client.IsActive) {
                _client[0].SendCommand(new ArgStringMessage(ComReceiveMessage.uniqueID, clientID, message));
            }
        }
    }
}
