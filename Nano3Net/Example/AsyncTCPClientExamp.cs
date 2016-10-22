/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 04/08/2016 01:13
*/

using System;
using System.Net;
using System.Net.Sockets;

using Nano3;
namespace Nano3.Net.Example
{
    public class AsyncTCPNetClientExamp : INetClient
    {
        public event EventHandlerArg<IConnector> ConnectEvent;
        public event EventHandlerArg<object> DisconnectEvent;

        private bool _isActive;
        public bool IsActive { get { return _isActive; } }

        private CommandStorage _commands;
        public CommandStorage Commands { get { return _commands; } }

        private AsyncTCPConnector _TCP;
        public IConnector this[int connectorIndex] {
            get { return _TCP; }
        }

        private long _clientID;
        public long ClientID {
            get { return _clientID; }
            set { _clientID = value; }
        }

        public AsyncTCPNetClientExamp()
        {
            _commands = new CommandStorage();
            _isActive = false;
        }

        public void Connect(object connectParam)
        {
            if (_isActive) {
                NetLogger.Log("TCP client is already connected");
                return;
            }
            IPEndPoint endPoint = connectParam as IPEndPoint;
            if (endPoint == null) { throw new ArgumentNullException("Incorrect connect param, need IPEndPoint"); }

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.BeginConnect(endPoint, OnConnectCallback, sock);
        }

        private void OnConnectCallback(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;
            if (sock == null) return;

            IPEndPoint ep = null;
            try {
                sock.EndConnect(ar);
                ep = sock.RemoteEndPoint as IPEndPoint;

                _TCP = new AsyncTCPConnector(sock, DateTime.Now.Ticks, NetHelper.READ_BUFFER_BIG);
                _TCP.DisconnectEvent += OnDisconnectEventHandler;
                _TCP.ReceiveAction += OnReceveDataHandler;
                _TCP.ReceiveComAction += OnReceiveCommandHandler;
            }
            catch (SocketException ex) {
                sock.Close();
                sock = null;
                NetLogger.Log(ex);
                return;
            }

            _isActive = true;
            if (ep != null) { NetLogger.Log("Connected to " + sock.RemoteEndPoint); }
            ConnectEvent?.Invoke(this, _TCP);
        }

        private void OnReceveDataHandler(IConnector connector, byte[] data)
        {
            try {
                if (connector != null) { _commands.Execute(connector, data); }
            }
            catch (Exception ex) { NetLogger.Log(ex.ToString(), LogType.Exception); }
        }
        private void OnReceiveCommandHandler(IConnector connector, ICommandArg arg)
        {
            try {
                if (connector != null) { _commands.Execute(connector, arg); }
            }
            catch (Exception ex) { NetLogger.Log(ex, LogType.Exception); }
        }

        public void Disconnect()
        {
            if (_TCP == null) {
                NetLogger.Log("TCP connection module is not connected");
                return;
            }
            _TCP.Disconnect();
        }

        private void OnDisconnectEventHandler(object sender, object disconnectState)
        {
            _TCP.DisconnectEvent -= OnDisconnectEventHandler;
            _TCP.ReceiveAction -= OnReceveDataHandler;
            _TCP.ReceiveComAction -= OnReceiveCommandHandler;

            _clientID = 0;
            _isActive = false;

            string s = "Disconnected ";
            if (disconnectState != null) { s += disconnectState.ToString(); }
            NetLogger.Log(s);

            DisconnectEvent?.Invoke(_TCP, disconnectState);
            _TCP = null;
        }
    }
}
