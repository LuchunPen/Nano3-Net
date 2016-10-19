/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 03.08.2016 22:19:47
*/

using System;

namespace Nano3.Net
{
    public class ArgUniqueID : ICommandArg
    {
        private long _commandID;
        public long CommandID
        {
            get { return _commandID; }
        }

        public long UniqueID;

        public ArgUniqueID()
        {
            _commandID = ComUniqueID.uniqueID;
        }

        public ArgUniqueID(long commandID)
        {
            _commandID = commandID;
        }

        public byte[] Pack(int headerOffset)
        {
            BytePacketWriter bp = new BytePacketWriter(headerOffset + 16);
            bp.CurrentPosition = headerOffset;

            bp.Write(_commandID);
            bp.Write(UniqueID);

            return bp.GetPacket();
        }

        public bool UnPack(byte[] pack)
        {
            if (pack == null || pack.Length < 16) { return false; }
            int ptr = 0;
            _commandID = BytePacketReader.ReadLong(pack, ref ptr);
            UniqueID = BytePacketReader.ReadLong(pack, ref ptr);

            return true;
        }
    }
}
