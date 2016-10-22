/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 20/01/2016 12:32
*/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Nano3
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct FloatInt
    {
        [FieldOffset(0)]
        public int ivalue;
        [FieldOffset(0)]
        public float fvalue;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct DoubleLong
    {
        [FieldOffset(0)]
        public long lvalue;
        [FieldOffset(0)]
        public double dvalue;
    }

    public static class ByteGetterExtend
    {
        public static byte[] GetBytes(this short value)
        {
            return new byte[] { (byte)value, (byte)(value >> 8) };
        }
        public static byte[] GetBytes(this ushort value)
        {
            return new byte[] { (byte)value, (byte)(value >> 8) };
        }
        public static byte[] GetBytes(this int value)
        {
            return new byte[]
            {
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24)
            };
        }
        public static byte[] GetBytes(this uint value)
        {
            return new byte[]
            {
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24)
            };
        }
        public static byte[] GetBytes(this long value)
        {
            return new byte[]
            {
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24),
                (byte)(value >> 32), (byte)(value >> 40), (byte)(value >> 48), (byte)(value >> 56)
            };
        }
        public static byte[] GetBytes(this ulong value)
        {
            return new byte[]
            {
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24),
                (byte)(value >> 32), (byte)(value >> 40), (byte)(value >> 48), (byte)(value >> 56)
            };
        }
        public static byte[] GetBytes(this float value)
        {
            FloatInt fi = new FloatInt(); fi.fvalue = value;
            return new byte[]
            {
                (byte)fi.ivalue, (byte)(fi.ivalue >> 8), (byte)(fi.ivalue >> 16), (byte)(fi.ivalue >> 24)
            };
        }
        public static byte[] GetBytes(this double value)
        {
            DoubleLong dl = new DoubleLong(); dl.dvalue = value;
            return new byte[]
            {
                (byte)dl.lvalue, (byte)(dl.lvalue >> 8), (byte)(dl.lvalue >> 16), (byte)(dl.lvalue >> 24),
                (byte)(dl.lvalue >> 32), (byte)(dl.lvalue >> 40), (byte)(dl.lvalue >> 48), (byte)(dl.lvalue >> 56)
            };
        }
        public static byte[] GetBytesUTF8(this string value)
        {
            if (String.IsNullOrEmpty(value)) return null;
            return Encoding.UTF8.GetBytes(value);
        }
        public static byte[] GetBytesANCII(this string value)
        {
            if (String.IsNullOrEmpty(value)) return null;
            return Encoding.ASCII.GetBytes(value);
        }
    }
}
