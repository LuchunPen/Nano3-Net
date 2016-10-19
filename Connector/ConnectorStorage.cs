/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 12.06.2016 21:29:10
*/

using System;
using System.Threading;
using Nano3.Collection;

namespace Nano3.Net
{
    public class ConnectorStorage : IConnectorStorage
    {
        private FastDictionaryM2<long, IConnector> _storage;
        private IConnector[] _lastAllConnectors;

        private int _count;
        public int Count { get { return _count; } }

        private int _maxCount;
        public int MaxCount { get { return _maxCount; } }

        public ConnectorStorage(int maxCount = 100)
        {
            if (maxCount <= 0) { throw new ArgumentOutOfRangeException("max count <= 0"); }
            _storage = new FastDictionaryM2<long, IConnector>();

            _maxCount = maxCount;
            _count = 0;
        }

        public bool Contains(long connectionID)
        {
            return _storage.ContainsKey(connectionID);
        }

        public bool AddConnector(IConnector connector)
        {
            if (connector == null) return false;
            if (_count >= _maxCount) { return false; }
            if (_storage.ContainsKey(connector.UniqueID)) { return false; }

            _lastAllConnectors = null;
            Interlocked.Increment(ref _count);
            _storage.Add(connector.UniqueID, connector);
            return true;
        }

        public IConnector GetConnector(long connectorID)
        {
            IConnector conn;
            _storage.TryGetValue(connectorID, out conn);
            return conn;
        }

        public IConnector RemoveConnector(long connectoID)
        {
            IConnector conn;
            _storage.TryGetAndRemove(connectoID, out conn);
            if (conn != null)
            {
                Interlocked.Decrement(ref _count);
                _lastAllConnectors = null;
            }
            return conn;
        }

        public IConnector[] GetAllConnectors()
        {
            if(_lastAllConnectors == null) {
                _lastAllConnectors = _storage.GetValues();
            }
            return _lastAllConnectors;
        }

        public void CloseAndRemoveAll()
        {
            if (_lastAllConnectors == null){
                _lastAllConnectors = _storage.GetValues();
            }
            _storage.Clear();
            _count = 0;

            for (int i = 0; i < _lastAllConnectors.Length; i++){
                _lastAllConnectors[i].Disconnect();
            }
            _lastAllConnectors = null;
        }
    }
}
