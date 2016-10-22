/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 03.08.2016 22:19:47
*/

using System;

namespace Nano3.Net.Example
{
    public class ArgUniqueID : ICommandArg
    {
        private long _commandID;
        public long CommandID
        {
            get { return _commandID; }
        }

        private long _uniqueID;
        public long UniqueID {
            get { return _uniqueID; }
        }

        public ArgUniqueID() { }

        public ArgUniqueID(long commandID, long uniqueID)
        {
            _commandID = commandID;
            _uniqueID = uniqueID;
        }

        public byte[] Pack(int headerOffset)
        {
            BytePacketWriter bp = new BytePacketWriter(headerOffset + 16);
            bp.CurrentPosition = headerOffset;

            bp.Write(_commandID);
            bp.Write(_uniqueID);

            return bp.ToArray();
        }

        public bool UnPack(byte[] pack)
        {
            if (pack == null || pack.Length < 16) { return false; }
            int ptr = 0;
            _commandID = BytePacketReader.ReadLong(pack, ref ptr);
            _uniqueID = BytePacketReader.ReadLong(pack, ref ptr);

            return true;
        }
    }
}
