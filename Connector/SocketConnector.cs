/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08.07.2016 15:14:03
*/

using System;
using System.Net;
using System.Net.Sockets;

using System.Threading;

namespace Nano3.Net
{
    public abstract class SocketConnector : IConnector, ITrafficSource
    {
        public event EventHandler DisconnectEvent;

        protected long _inTrafficBytes;
        protected long _outTrafficBytes;
        public long InTrafficBytes
        {
            get {
                return _inTrafficBytes;
            }
        }
        public long OutTrafficBytes
        {
            get {
                return _outTrafficBytes;
            }
        }

        private readonly long _connectionID;
        public long UniqueID
        {
            get { return _connectionID; }
        }

        private Action<IConnector, byte[]> _receiveAction;
        public Action<IConnector, byte[]> ReceiveAction
        {
            get { return _receiveAction; }
            set
            {
                if (value == null && _receiveAction != null)
                {
                    foreach(var d in _receiveAction.GetInvocationList()){
                        _receiveAction -= (d as Action<IConnector, byte[]>);
                    }
                }
                else { _receiveAction = value; }
            }
        }

        private Action<IConnector, ICommandArg> _receiveComAction;
        public Action<IConnector, ICommandArg> ReceiveComAction
        {
            get { return _receiveComAction; }
            set
            {
                if (value == null && _receiveComAction != null)
                {
                    foreach (var d in _receiveComAction.GetInvocationList()){
                        _receiveComAction -= (d as Action<IConnector, ICommandArg>);
                    }
                }
                _receiveComAction = value;
            }
        }

        protected Socket _socket;
        public Socket CSocket { get { return _socket; } }

        protected int _isClosed;
        public bool IsClosed
        {
            get { if (_isClosed == 1) { return true; } else return false; }
        }

        public bool IsConnected
        {
            get
            {
                if (_isClosed == 1) { return false; }
                bool part1 = _socket.Poll(1000, SelectMode.SelectRead);
                bool part2 = _socket.Available == 0;
                if ((part1 && part2) || _isClosed == 1) { return false; }
                else { return true; }
            }
        }

        protected IPEndPoint _localEndPoint;
        public IPEndPoint LocalEndPoint { get { return _localEndPoint; } }

        public SocketConnector(Socket socket, long connectionID)
        {
            if (socket == null) { throw new ArgumentNullException("Socket is null"); }
            _connectionID = connectionID;

            _socket = socket;
            _localEndPoint = _socket.LocalEndPoint as IPEndPoint;
        }

        public virtual void SendCommand(ICommandArg comArg, object sendParam = null)
        {
            if (comArg == null) return;
            byte[] comData = comArg.Pack(NetProtocol.HeaderLenght);
            NetProtocol.SingPacket(ref comData);
            Send(comData, sendParam);
        }

        public virtual void SendCommands(ICommandArg[] comArgs, object sendParam = null)
        {
            if (comArgs == null || comArgs.Length == 0) { return; }
            BytePacketWriter bp = new BytePacketWriter();
            for (int i = 0; i < comArgs.Length; i++)
            {
                if (comArgs[i] == null) continue;
                byte[] comData = comArgs[i].Pack(NetProtocol.HeaderLenght);
                NetProtocol.SingPacket(ref comData);
                bp.WritePacketAsIs(comData);
            }

            if (bp.CurrentPosition > NetProtocol.HeaderLenght) { Send(bp.GetPacket()); }
        }

        public abstract void Send(byte[] data, object sendParam = null);
        
        public void Disconnect()
        {
            int cl = Interlocked.CompareExchange(ref _isClosed, 1, 0);
            if (cl == 1) return;

            ReceiveAction = null;
            ReceiveComAction = null;

            InternalClose();
        }

        protected virtual void InternalClose()
        {
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close(50);
            }
            catch (Exception ex){
                SocketException sEx = ex as SocketException;
                if (sEx != null){
                    NetLogger.Log(UniqueID + ": " + sEx.ErrorCode + ", " + sEx, LogType.Exception);
                }
                else{ NetLogger.Log(UniqueID + ": " + ex, LogType.Exception); }
            }
            DisconnectTrigger();
        }

        protected void DisconnectTrigger()
        {
            DisconnectEvent?.Invoke(this, null);
        }

        protected void CatchedException(Exception ex)
        {
            SocketException sEx = ex as SocketException;
            if (sEx != null) {
                NetLogger.Log(UniqueID + ": " + sEx.ErrorCode + ", " + sEx, LogType.Exception);
                Disconnect();
            }
            else {
                NetLogger.Log(UniqueID + ": " + ex, LogType.Exception);
            }
        }
    }
}
