/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 17/01/2016 09:17
*/

using System;
using System.Text;

namespace Nano3
{
    public class BytePacketReader
    {
        private byte[] _items;
        private int _bufferSize;
        private int _ptr;

        public int CurrentPosition { get { return _ptr; } }

        public BytePacketReader(byte[] packet)
        {
            if (packet == null) { new ArgumentNullException("Source buffer is null)"); }
            _items = packet;
            _bufferSize = _items.Length;
        }

        public bool ReadBoolean()
        {
            return _items[_ptr++] == 1;
        }
        public byte ReadByte()
        {
            return _items[_ptr++];
        }
        public sbyte ReadSbyte()
        {
            return (sbyte)_items[_ptr++];
        }
        public short ReadShort()
        {
            return (short)(_items[_ptr++] | _items[_ptr++] << 8);
        }
        public ushort ReadUshort()
        {
            return (ushort)(_items[_ptr++] | _items[_ptr++] << 8);
        }
        public int ReadInt()
        {
            return (int)(_items[_ptr++] | _items[_ptr++] << 8 | _items[_ptr++] << 16 | _items[_ptr++] << 24);
        }
        public uint ReadUint()
        {
            return (uint)(_items[_ptr++] | _items[_ptr++] << 8 | _items[_ptr++] << 16 | _items[_ptr++] << 24);
        }
        public long ReadLong()
        {
            return (_items[_ptr++] | (long)_items[_ptr++] << 8
                | (long)_items[_ptr++] << 16 | (long)_items[_ptr++] << 24
                | (long)_items[_ptr++] << 32 | (long)_items[_ptr++] << 40
                | (long)_items[_ptr++] << 48 | (long)_items[_ptr++] << 56);
        }
        public ulong ReadUlong()
        {
            return (_items[_ptr++] | (ulong)_items[_ptr++] << 8 
                | (ulong)_items[_ptr++] << 16 | (ulong)_items[_ptr++] << 24 
                | (ulong)_items[_ptr++] << 32 | (ulong)_items[_ptr++] << 40 
                | (ulong)_items[_ptr++] << 48 | (ulong)_items[_ptr++] << 56);
        }
        public float ReadSingle()
        {
            FloatInt fi = new FloatInt();
            fi.ivalue = (int)(_items[_ptr++] | _items[_ptr++] << 8 | _items[_ptr++] << 16 | _items[_ptr++] << 24);
            return fi.fvalue;
        }
        public double ReadDouble()
        {
            DoubleLong dl = new DoubleLong();
            dl.lvalue = (_items[_ptr++] | (long)_items[_ptr++] << 8
                | (long)_items[_ptr++] << 16 | (long)_items[_ptr++] << 24
                | (long)_items[_ptr++] << 32 | (long)_items[_ptr++] << 40
                | (long)_items[_ptr++] << 48 | (long)_items[_ptr++] << 56);
            return dl.dvalue;
        }
        public string ReadString()
        {
            int size = ReadInt();
            if (size > 0 && _ptr + size <= _items.Length)
            {
                int n = _ptr;
                _ptr += size;
                return Encoding.UTF8.GetString(_items, n, size);
            }
            return "";
        }

        public byte[] ReadPacket()
        {
            int size = ReadInt();
            if (size > 0 && _ptr + size <= _items.Length)
            {
                int n = _ptr;
                _ptr += size;
                byte[] result = new byte[size];
                Buffer.BlockCopy(_items, n, result, 0, size);
                return result;
            }
            return new byte[0];
        }

        #region StaticFunc
        public static bool ReadBoolean(byte[] packet, ref int position)
        {
            return packet[position++] == 1;
        }
        public static byte ReadByte(byte[] packet, ref int position)
        {
            return packet[position++];
        }
        public static sbyte ReadSbyte(byte[] packet, ref int position)
        {
            return (sbyte)packet[position++];
        }
        public static short ReadShort(byte[] packet, ref int position)
        {
            return (short)(packet[position++] | packet[position++] << 8);
        }
        public static ushort ReadUshort(byte[] packet, ref int position)
        {
            return (ushort)(packet[position++] | packet[position++] << 8);
        }
        public static int ReadInt(byte[] packet, ref int position)
        {
            return (int)(packet[position++] | packet[position++] << 8 | packet[position++] << 16 | packet[position++] << 24);
        }
        public static uint ReadUint(byte[] packet, ref int position)
        {
            return (uint)(packet[position++] | packet[position++] << 8 | packet[position++] << 16 | packet[position++] << 24);
        }
        public static long ReadLong(byte[] packet, ref int position)
        {
            return (packet[position++] | (long)packet[position++] << 8
                | (long)packet[position++] << 16 | (long)packet[position++] << 24
                | (long)packet[position++] << 32 | (long)packet[position++] << 40
                | (long)packet[position++] << 48 | (long)packet[position++] << 56);
        }
        public static ulong ReadUlong(byte[] packet, ref int position)
        {
            return (packet[position++] | (ulong)packet[position++] << 8
                | (ulong)packet[position++] << 16 | (ulong)packet[position++] << 24
                | (ulong)packet[position++] << 32 | (ulong)packet[position++] << 40
                | (ulong)packet[position++] << 48 | (ulong)packet[position++] << 56);
        }
        public static float ReadSingle(byte[] packet, ref int position)
        {
            FloatInt fi = new FloatInt();
            fi.ivalue = (int)(packet[position++] | packet[position++] << 8 | packet[position++] << 16 | packet[position++] << 24);
            return fi.fvalue;
        }
        public static double ReadDouble(byte[] packet, ref int position)
        {
            DoubleLong dl = new DoubleLong();
            dl.lvalue = (packet[position++] | (long)packet[position++] << 8
                | (long)packet[position++] << 16 | (long)packet[position++] << 24
                | (long)packet[position++] << 32 | (long)packet[position++] << 40
                | (long)packet[position++] << 48 | (long)packet[position++] << 56);
            return dl.dvalue;
        }
        public static string ReadString(byte[] packet, ref int position)
        {
            int size = ReadInt(packet, ref position);
            if (size > 0 && position + size <= packet.Length)
            {
                int pos = position;
                position += size;
                return Encoding.UTF8.GetString(packet, pos, size);
            }
            return "";
        }
        public static byte[] ReadPacket(byte[] packet, ref int position)
        {
            int size = ReadInt(packet, ref position);
            byte[] result = new byte[size];
            if (result.Length == 0) return result;
            Buffer.BlockCopy(packet, position, result, 0, size);
            position += size;
            return result;
        }

        public static byte[] GetBytes(short value)
        {
            return new byte[] { (byte)value, (byte)(value >> 8) };
        }
        public static byte[] GetBytes(ushort value)
        {
            return new byte[] { (byte)value, (byte)(value >> 8) };
        }
        public static byte[] GetBytes(int value)
        {
            return new byte[]
            {
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24)
            };
        }
        public static byte[] GetBytes(uint value)
        {
            return new byte[]
            {
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24)
            };
        }
        public static byte[] GetBytes(long value)
        {
            return new byte[]
            {
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24),
                (byte)(value >> 32), (byte)(value >> 40), (byte)(value >> 48), (byte)(value >> 56)
            };
        }
        public static byte[] GetBytes(ulong value)
        {
            return new byte[]
            {
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24),
                (byte)(value >> 32), (byte)(value >> 40), (byte)(value >> 48), (byte)(value >> 56)
            };
        }
        public static byte[] GetBytes(float value)
        {
            FloatInt fi = new FloatInt(); fi.fvalue = value;
            return new byte[]
            {
                (byte)fi.ivalue, (byte)(fi.ivalue >> 8), (byte)(fi.ivalue >> 16), (byte)(fi.ivalue >> 24)
            };
        }
        public static byte[] GetBytes(double value)
        {
            DoubleLong dl = new DoubleLong(); dl.dvalue = value;
            return new byte[]
            {
                (byte)dl.lvalue, (byte)(dl.lvalue >> 8), (byte)(dl.lvalue >> 16), (byte)(dl.lvalue >> 24),
                (byte)(dl.lvalue >> 32), (byte)(dl.lvalue >> 40), (byte)(dl.lvalue >> 48), (byte)(dl.lvalue >> 56)
            };
        }
        public static byte[] GetBytesUTF8(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;
            return Encoding.UTF8.GetBytes(value);
        }
        public static byte[] GetBytesANCII(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;
            return Encoding.ASCII.GetBytes(value);
        }
        #endregion StaticFunc
    }
}
