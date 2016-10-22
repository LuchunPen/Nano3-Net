/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 22.10.2016 14:30:29
*/

using System;

namespace Nano3.Net.Example
{
    public class ComDisconnection : Command<ArgUniqueID>
    {
        public static readonly long uniqueID = 12347;
        public static readonly string name = typeof(ComDisconnection).Name;

        public override long UniqueID {
            get { return uniqueID; }
        }
        public override string Description {
            get { return null; }
        }
        public override string Name {
            get { return name; }
        }

        public ComDisconnection(Action<IConnector, ArgUniqueID> executor)
            : base(executor) { }
    }
}
