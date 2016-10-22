using System;

using Nano3.Net;
using Nano3.Net.Example;

namespace Nano3NetTest
{
    class Program
    {
        private static AsyncTCPServerExamp _server;
        static void Main(string[] args)
        {
            NetProtocol.Initialize(new Protocol_12());

            _server = new AsyncTCPServerExamp(5);
            _server.NewConnectionEvent += OnNewConnectionHandler;
            _server.DisconnectEvent += OnConnectorDisconnectHandler;
            _server.OnStartEvent += OnServerStarted;

            _server.Commands.Add(new ComReceiveMessage(OnResendMessageToAll));
           
            _server.Start("127.0.0.1", "20000");

            string com = null;
            while (com != "Stop") {
                com = Console.ReadLine();
            }
        }

        private static void OnServerStarted(object sender, EventArgs arg)
        {
            Console.WriteLine("Server started " + _server.Address);
        }
        private static void OnNewConnectionHandler(object sender, IConnector connector)
        {
            if (connector == null) { return; }
            connector.SendCommand(new ArgUniqueID(ComGetClientID.uniqueID, connector.UniqueID));
            Console.WriteLine("New connection " + connector.UniqueID);
            SendToAllAboutNewConnection(connector);
        }

        private static void OnConnectorDisconnectHandler(object sender, object disconnectionState)
        {
            IConnector con = sender as IConnector;
            if (con != null) {
                Console.WriteLine(con.UniqueID + " disconnected");
                SendToAllAbountDisconnection(con);
            }
        }

        private static void SendToAllAboutNewConnection(IConnector connector)
        {
            if (connector == null) { return; }
            IConnector[] connectors = _server.GetAllConnectors();
            ArgUniqueID arg = new ArgUniqueID(ComNewConnection.uniqueID, connector.UniqueID);
            for (int i = 0; i < connectors.Length; i++) {
                if (connectors[i] == connector) continue;
                connectors[i].SendCommand(arg);
            }
        }

        public static void SendToAllAbountDisconnection(IConnector connector)
        {
            if (connector == null) { return; }
            IConnector[] connectors = _server.GetAllConnectors();
            ArgUniqueID arg = new ArgUniqueID(ComDisconnection.uniqueID, connector.UniqueID);
            for (int i = 0; i < connectors.Length; i++) {
                if (connectors[i] == connector) continue;
                connectors[i].SendCommand(arg);
            }
        }

        private static void OnResendMessageToAll(IConnector connector, ArgStringMessage arg)
        {
            if (connector == null) { return; }

            Console.WriteLine(arg.ClientID + " say: " + arg.Message);

            IConnector[] connectors = _server.GetAllConnectors();
            for (int i = 0; i < connectors.Length; i++) {
                if (connectors[i] == connector) continue;
                connectors[i].SendCommand(arg);
            }
        }
    }
}
