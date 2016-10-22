/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08.06.2016 19:01:05
*/

using System;

namespace Nano3.Net
{
    public static class NetProtocol
    {
        private static IProtocol _protocol;
        public static IProtocol Protocol { get { return _protocol; } }

        public static int HeaderLenght
        {
            get
            {
                if (_protocol == null) return -1;
                return _protocol.HeaderLenght;
            }
        }

        public static bool Initialize(IProtocol protocol)
        {
            if (_protocol == null)
            {
                if (protocol == null) { throw new ArgumentNullException("Protocol is null"); }
                _protocol = protocol;
                return true;
            }
            return false;
        }

        public static void SingPacket(ref byte[] packet)
        {
            _protocol.SignPacket(ref packet);
        }
        public static byte[] GetPacket(ref byte[] buffer)
        {
            return  _protocol.GetPacket(ref buffer);
        }
    }
}
