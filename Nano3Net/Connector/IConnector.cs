/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08.06.2016 15:42:57
*/

using System;

namespace Nano3.Net
{
    public interface IConnector : IUniq
    {
        event EventHandler DisconnectEvent;
        bool IsConnected { get; }

        Action<IConnector, byte[]> ReceiveAction { get; set; }
        void Send(byte[] data, object sendParam = null);

        Action<IConnector, ICommandArg> ReceiveComAction { get; set; }
        void SendCommand(ICommandArg comArg, object sendParam = null);
        void SendCommands(ICommandArg[] comArgs, object sendParam = null);

        void Disconnect();
    }

    public interface ITrafficSource
    {
        long InTrafficBytes { get; }
        long OutTrafficBytes { get; }
    }
}
