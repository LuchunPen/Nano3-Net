/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 12.06.2016 0:29:44
*/

using System;

namespace Nano3.Net
{
    public interface IConnectorStorage
    {
        int Count { get; }
        int MaxCount { get; }

        IConnector GetConnector(long connectorID);
        bool AddConnector(IConnector connector);
        IConnector RemoveConnector(long connectoID);
        IConnector[] GetAllConnectors();

        void CloseAndRemoveAll();
    }
}
