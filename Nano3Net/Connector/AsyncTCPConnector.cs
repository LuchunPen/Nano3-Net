/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08.07.2016 16:53:20
*/

using System;
using System.Net.Sockets;

namespace Nano3.Net
{
    public class AsyncTCPConnector : SocketConnector
    {
        private byte[] _readBuffer;
        private byte[] _readData = new byte[0];

        public AsyncTCPConnector(Socket socket, long connectionID, int readBufferLenght = NetHelper.READ_BUFFER_NORMAL)
            :base (socket, connectionID)
        {
            if (socket.AddressFamily != AddressFamily.InterNetwork
               || socket.ProtocolType != ProtocolType.Tcp
               || socket.SocketType != SocketType.Stream){
                throw new ArgumentException("Incompatible socket");
            }

            socket.NoDelay = true;

            if (readBufferLenght <= 512){
                throw new ArgumentOutOfRangeException("The buffer lenght is too small");
            }
            _readBuffer = new byte[readBufferLenght];
            if (IsConnected){
                _socket.BeginReceive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None, OnReceiveCallback, null);
            }
            else { Disconnect(); }
        }

        private void OnReceiveCallback(IAsyncResult ar)
        {
            int rLenght = 0;
            try
            {
                if (_isClosed == 0){
                    rLenght = _socket.EndReceive(ar);
                }
            }
            catch(Exception ex){ CatchedException(ex); }

            if (rLenght == 0){
                NetLogger.Log("Other side close connection");
                Disconnect();
            }

            try
            {
                int curReceived = _readData.Length;
                Array.Resize(ref _readData, curReceived + rLenght);
                Buffer.BlockCopy(_readBuffer, 0, _readData, curReceived, rLenght);
                _inTrafficBytes += rLenght;

            GETPACKET:
                byte[] packet = NetProtocol.GetPacket(ref _readData);

                if (packet != null){
                    ReceiveAction?.Invoke(this, packet);
                    goto GETPACKET;
                }
                if (_isClosed == 0){
                    _socket.BeginReceive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None, OnReceiveCallback, null);
                }
            }
            catch (Exception ex){ CatchedException(ex); }
        }

        public override void Send(byte[] data, object sendParam = null)
        {
            try
            {
                _socket.BeginSend(data, 0, data.Length, SocketFlags.None, OnSendCallback, data.Length);
            }
            catch(Exception ex){ CatchedException(ex); }
        }

        private void OnSendCallback(IAsyncResult ar)
        {
            int dataL = (int)ar.AsyncState;
            try
            {
                if (_isClosed == 0)
                {
                    _socket.EndSend(ar);
                    _outTrafficBytes += dataL;
                }
            }
            catch (Exception ex){ CatchedException(ex); }
        }
    }
}
