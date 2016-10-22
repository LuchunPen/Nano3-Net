/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 22.10.2016 13:50:09
*/

using System;
using Nano3.Collection;

namespace Nano3.Net.Example
{
    public class AsyncTCPServerExamp
    {
        public event EventHandlerArg<IConnector> NewConnectionEvent;
        public event EventHandlerArg<object> DisconnectEvent;

        public event EventHandler OnStartEvent;
        public event EventHandler OnStopEvent;

        private AsyncTCPListenModule _listener;
        public string Address {
            get { return _listener.ListenAddress.ToString(); }
        }

        private CommandStorage _commands;
        public CommandStorage Commands { get { return _commands; } }

        private FastDictionaryM2<long, IConnector> _connectors;
        private object _cSync;

        private int _maxCount;

        public AsyncTCPServerExamp(int maxCount)
        {
            _maxCount = maxCount > 1 ? maxCount : 1;
            _commands = new CommandStorage();
            _connectors = new FastDictionaryM2<long, IConnector>();
            _cSync = new object();
            _listener = new AsyncTCPListenModule(1, NetHelper.READ_BUFFER_NORMAL);

            _listener.NewConnectionEvent += OnNewConnectionHandler;
            _listener.StartEvent += OnListenerStartHandler;
            _listener.StopEvent += OnListenerStopHandler;
        }

        public void Start(string IP, string port)
        {
            if (!_listener.IsActive) {
                _listener.Start(NetHelper.GetIPEndPoint(IP, port));
            }
        }

        public void Stop()
        {
            if (_listener.IsActive) { _listener.Stop(null); }
            DisconnectAll();
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

        public void OnNewConnectionHandler(object sender, IConnector connector)
        {
            if (connector == null) return;

            lock (_cSync) {
                if (_connectors.Count >= _maxCount) { connector.Disconnect(); return; }
                _connectors.Add(connector.UniqueID, connector);
            }
            connector.DisconnectEvent += OnConnectorDisconnectHandler;
            connector.ReceiveAction += OnReceveDataHandler;
            connector.ReceiveComAction += OnReceiveCommandHandler;

            NewConnectionEvent?.Invoke(this, connector);
        }

        public void OnConnectorDisconnectHandler(object sender, object disconnectionState)
        {
            IConnector connector = sender as IConnector;
            if (connector == null) return;

            connector.DisconnectEvent -= OnConnectorDisconnectHandler;
            connector.ReceiveAction -= OnReceveDataHandler;
            connector.ReceiveComAction -= OnReceiveCommandHandler;

            lock (_cSync) {
                _connectors.Remove(connector.UniqueID);
            }

            DisconnectEvent?.Invoke(connector, disconnectionState);
        }

        private void OnListenerStartHandler(object sender, EventArgs arg)
        {
            OnStartEvent?.Invoke(this, arg);
        }

        private void OnListenerStopHandler(object sender, EventArgs arg)
        {
            OnStopEvent?.Invoke(this, arg);
        }

        public IConnector GetConnector(long connectionID)
        {
            lock (_cSync) {
                IConnector con = null;
                _connectors.TryGetValue(connectionID, out con);
                return con;
            }
        }

        public IConnector[] GetAllConnectors()
        {
            lock (_cSync) {
                return _connectors.GetValues();
            }
        }

        private void Disconnect(long _connectorID)
        {
            lock (_cSync) {
                IConnector conn = null;
                _connectors.TryGetValue(_connectorID, out conn);
                if (conn != null) { conn.Disconnect(); }
            }
        }

        private void DisconnectAll()
        {
            IConnector[] cons = null;
            lock (_cSync) { cons = _connectors.GetValues(); }
            if (cons != null) {
                for (int i = 0; i < cons.Length; i++) {
                    if (cons[i] != null) {
                        cons[i].Disconnect();
                    }
                }
            }
        }
    }
}
