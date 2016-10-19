/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08.06.2016 19:05:42
*/

using System;

namespace Nano3.Net
{
    public interface IProtocol
    {
        int HeaderLenght { get; }

        void SignPacket(ref byte[] packet);
        byte[] GetPacket(ref byte[] buffer);
    }
}
