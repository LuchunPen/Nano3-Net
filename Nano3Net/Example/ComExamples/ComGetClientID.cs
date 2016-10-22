/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 03.08.2016 22:20:58
*/

using System;

namespace Nano3.Net.Example
{
    public class ComGetClientID : Command<ArgUniqueID>
    {
        public static readonly long uniqueID = 12345;
        public static readonly string name = typeof(ComGetClientID).Name;

        public override long UniqueID
        {
            get { return uniqueID; }
        }
        public override string Description
        {
            get { return null; }
        }
        public override string Name
        {
            get { return name; }
        }

        public ComGetClientID(Action<IConnector, ArgUniqueID> executor)
            : base(executor) { }
    }
}
