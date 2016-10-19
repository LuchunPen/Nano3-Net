/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 17.06.2016 22:34:23
*/

using System;

namespace Nano3.Net
{
    public static class ComArgHelper
    {
        public static readonly int COMARG_HEADER_LENGHT = 8;

        public static long GetCommandID(byte[] comData)
        {
            if (comData == null || comData.Length < COMARG_HEADER_LENGHT) return 0;
            return BitConverter.ToInt64(comData, 0);
        }
    }
}
