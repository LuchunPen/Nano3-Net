/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 25/05/2016 12:42
*/

using System;

namespace Nano3
{
    public class EventArgs<TValue> : EventArgs
    {
        private TValue _value;
        public TValue Value { get { return _value; } }

        public EventArgs(TValue value)
        {
            _value = value;
        }
    }
}
