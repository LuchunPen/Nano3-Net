/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 15.06.2016 2:30:59
*/

using System;

namespace Nano3.Net
{
    public interface ICommandArg
    {
        long CommandID { get; }

        byte[] Pack(int headerOffset);
        bool UnPack(byte[] pack);
    }
}
