/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 10.07.2016 9:57:51
*/

using System;

namespace Nano3.Net
{
    public interface INetClient
    {
        event EventHandlerArg<IConnector> ConnectEvent;
        event EventHandlerArg<object> DisconnectEvent;

        bool IsActive { get; }
        long ClientID { get; }

        CommandStorage Commands { get; }
        IConnector this[int connector] { get; }

        void Connect(object connectionParam);
        void Disconnect();
    }
}
