/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 22.10.2016 14:32:21
*/

using System;

namespace Nano3.Net.Example
{
    public class ArgStringMessage : ICommandArg
    {
        private long _commandID;
        public long CommandID {
            get {
                return _commandID;
            }
        }

        private long _clientID;
        public long ClientID { get { return _clientID; } }
        private string _message;
        public string Message {
            get {
                return _message;
            }
        }

        public ArgStringMessage() { }

        public ArgStringMessage(long commandID, long clientID, string message)
        {
            _commandID = commandID;
            _clientID = clientID;
            _message = message;
        }

        public byte[] Pack(int headerOffset)
        {
            BytePacketWriter bp = new BytePacketWriter(headerOffset + 8);
            bp.CurrentPosition = headerOffset;

            bp.Write(_commandID);

            bp.Write(_clientID);
            bp.Write(_message);

            return bp.ToArray();
        }

        public bool UnPack(byte[] pack)
        {
            if (pack == null || pack.Length < 20) return false;
            int ptr = 0;
            _commandID = _clientID = BytePacketReader.ReadLong(pack, ref ptr);
            _clientID = BytePacketReader.ReadLong(pack, ref ptr);
            _message = BytePacketReader.ReadString(pack, ref ptr);
            return true;
        }
    }
}
