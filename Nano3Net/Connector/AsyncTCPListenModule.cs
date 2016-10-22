/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08.07.2016 18:39:02
*/

using System;
using System.Net;
using System.Net.Sockets;

using System.Threading;

namespace Nano3.Net
{
    public class AsyncTCPListenModule : IHostConnectionModule
    {
        public event EventHandlerArg<IConnector> NewConnectionEvent;
        public event EventHandler StopEvent;
        public event EventHandler StartEvent;

        private int _isActive;
        public bool IsActive
        {
            get { return _isActive == 1; }
        }

        private long _moduleID;
        public long UniqueID
        {
            get { return _moduleID; }
        }

        private Socket _listenSocket;
        private int _backLog;

        public IPEndPoint ListenAddress
        {
            get
            {
                if (_listenSocket != null){
                    IPEndPoint ipe = _listenSocket.LocalEndPoint as IPEndPoint;
                    return ipe;
                }
                return null;
            }
        }

        public AsyncTCPListenModule(long moduleID, int connectorReceiveBufferSize = NetHelper.READ_BUFFER_NORMAL, int backLog = 100)
        {
            if (backLog < 1) { throw new ArgumentOutOfRangeException("Back log < 1"); }
            _backLog = backLog;

            _moduleID = moduleID;
        }

        public void Start(object startParam)
        {
            int active = Interlocked.CompareExchange(ref _isActive, 1, 0);
            if (active != 0){
                NetLogger.Log(this.GetType().Name + "[" + _moduleID + "] module is already listening " + ListenAddress);
                return;
            }

            IPEndPoint ep = startParam as IPEndPoint;
            if (ep == null) {
                Stop(new ArgumentNullException("Incorrect Listen endpoint"));
                return;
            }

            try
            {
                _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _listenSocket.Bind(ep);
                _listenSocket.Listen(_backLog);

                _listenSocket.BeginAccept(OnAcceptCallback, null);
                StartEvent?.Invoke(this, null);
            }
            catch (Exception ex){ Stop(ex); }
        }

        private void OnAcceptCallback(IAsyncResult ar)
        {
            AsyncTCPConnector conn = null;
            Socket sock = null;
            try
            {
                if (_isActive == 1){
                    sock = _listenSocket.EndAccept(ar);
                }
            }
            catch(Exception ex){
                NetLogger.Log(ex, LogType.Exception);
                sock.Close();
                sock = null;
            }

            try
            {
                if (sock != null){
                    if (NewConnectionEvent == null) { sock.Close(); }

                    conn = new AsyncTCPConnector(sock, DateTime.Now.Ticks, NetHelper.READ_BUFFER_BIG);
                    NewConnectionEvent?.Invoke(this, conn);
                }
            }
            catch(Exception ex){
                NetLogger.Log(ex, LogType.Exception);

                if (conn != null) { conn.Disconnect(); }
                else if (sock != null) { sock.Close(); }
            }

            try
            {
                if (_isActive == 1){
                    _listenSocket.BeginAccept(OnAcceptCallback, null);
                }
            }
            catch (Exception ex) { Stop(ex); }
        }

        public void Stop(object stopParam)
        {
            int active = Interlocked.CompareExchange(ref _isActive, 0, 1);
            if (active != 1) {
                NetLogger.Log(this.GetType().Name + "[" + _moduleID + "] module is not started");
                return;
            }
            try
            {
                _listenSocket.Close(50);
                NetLogger.Log(stopParam, LogType.Log);
            }
            catch (Exception ex) { NetLogger.Log(ex, LogType.Log); }
            StopEvent?.Invoke(this, null);
        }
    }
}
