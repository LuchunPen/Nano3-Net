/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 04.07.2016 16:28:26
*/

using System;

namespace Nano3.Net
{
    public interface IHostConnectionModule : IUniq
    {
        bool IsActive { get; }

        event EventHandlerArg<IConnector> NewConnectionEvent;
        event EventHandler StopEvent;
        event EventHandler StartEvent;

        void Start(object startParam);
        void Stop(object stopParam);
    }
}
