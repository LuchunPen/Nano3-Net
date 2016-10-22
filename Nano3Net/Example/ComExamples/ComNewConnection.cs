/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 22.10.2016 14:29:09
*/

using System;

namespace Nano3.Net.Example
{
    public class ComNewConnection : Command<ArgUniqueID>
    {
        public static readonly long uniqueID = 12346;
        public static readonly string name = typeof(ComNewConnection).Name;

        public override long UniqueID {
            get { return uniqueID; }
        }
        public override string Description {
            get { return null; }
        }
        public override string Name {
            get { return name; }
        }

        public ComNewConnection(Action<IConnector, ArgUniqueID> executor)
            : base(executor) { }
    }
}
