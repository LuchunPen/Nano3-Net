/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 22.10.2016 14:31:31
*/

using System;

namespace Nano3.Net.Example
{
    public class ComReceiveMessage : Command<ArgStringMessage>
    {
        public static readonly long uniqueID = 12348;
        public static readonly string name = typeof(ComReceiveMessage).Name;

        public override long UniqueID {
            get { return uniqueID; }
        }

        public override string Description {
            get { return null; }
        }

        public override string Name {
            get { return name; }
        }

        public ComReceiveMessage(Action<IConnector, ArgStringMessage> executor)
            : base(executor) { }
    }
}
