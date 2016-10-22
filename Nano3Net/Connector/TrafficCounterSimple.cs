/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 28.09.2016 20:00:16
*/

using System;
using Nano3.Collection;

namespace Nano3.Net
{
    public class TrafficCounterSimple : IUpdatable
    {
        public event EventHandler TrafficDataChanged;
        private ITrafficSource _trafficSource;

        private double _lastUpdate;
        private double _interval;
        public double Interval
        {
            get { return _interval; }
            set { _interval = value < 0 ? 0 : value; }
        }

        private long _lastInValue;
        private long _lastOutValue;

        private CycleStorage<long> _inStorage;
        private CycleStorage<long> _outStorage;
        public CycleStorage<long> InStorage { get { return _inStorage; } }
        public CycleStorage<long> OutStorage { get { return _outStorage; } }

        public TrafficCounterSimple(ITrafficSource trafficSource, int storageCount, double intervalMs = 100)
        {
            _inStorage = new CycleStorage<long>(storageCount);
            _outStorage = new CycleStorage<long>(storageCount);

            Interval = intervalMs;
            _trafficSource = trafficSource;
        }

        public void Update(double time)
        {
            if (_lastUpdate + _interval > time) { return; }
                _lastUpdate = time;
            
            if (_trafficSource != null) {
                long curInValue = _trafficSource.InTrafficBytes;
                long curOutValue = _trafficSource.OutTrafficBytes;

                _inStorage.Add(curInValue - _lastInValue);
                _outStorage.Add(curOutValue - _lastOutValue);

                _lastInValue = curInValue;
                _lastOutValue = curOutValue;

                TrafficDataChanged?.Invoke(this, null);
            }
        }

        public void SetTrafficSource(ITrafficSource tSource)
        {
            _trafficSource = tSource;
            _inStorage.Clear();
            _outStorage.Clear();
        }

        public void Clear()
        {
            _inStorage.Clear();
            _outStorage.Clear();
        }
    }
}
