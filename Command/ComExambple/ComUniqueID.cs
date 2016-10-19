/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 03.08.2016 22:20:58
*/

using System;

namespace Nano3.Net
{
    public class ComUniqueID : Command<ArgUniqueID>
    {
        public static readonly long uniqueID = Uid64.LoadFromString("DDA165332F88AA03").Data;
        public static readonly string name = typeof(ComUniqueID).Name;

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

        public ComUniqueID(Action<IConnector, ArgUniqueID> executor) 
            : base(executor) { }
    }
}
