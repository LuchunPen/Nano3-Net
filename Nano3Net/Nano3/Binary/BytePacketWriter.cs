/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 17/01/2016 11:17
*/

using System;
using System.Text;

namespace Nano3
{
    public class BytePacketWriter
    {
        private byte[] _items;
        private int _ptr;
        private int _bufferSize;

        public BytePacketWriter()
        {
            _items = new byte[4];
            _bufferSize = 4;
        }
        public BytePacketWriter(int size)
        {
            _items = new byte[size];
            _bufferSize = size;
        }

        public byte[] ToArray()
        {
            if (_ptr == _bufferSize) return _items;
            else {
                byte[] b = new byte[_ptr];
                Array.Copy(_items, b, _ptr);
                return b;
            }
        }

        public int CurrentPosition
        {
            get { return _ptr; }
            set { _ptr = value < 0 ? 0 : value > _items.Length ? _items.Length : value; }
        }

        private void Resize(int addsize = 0)
        {
            if (addsize == 0)
            {
                byte[] newBuffer = new byte[_bufferSize * 2];
                Array.Copy(_items, newBuffer, _bufferSize);
                _items = newBuffer;
                _bufferSize = _bufferSize * 2;
            }
            else
            {
                byte[] newBuffer = new byte[_bufferSize + addsize];
                Array.Copy(_items, newBuffer, _bufferSize);
                _items = newBuffer;
                _bufferSize = _bufferSize + addsize;
            }
        }

        public void Write(bool value)
        {
            if (_ptr + 1 >= _bufferSize) { Resize(1); }
            if (value) _items[_ptr] = 1;
            _ptr++;
        }
        public void Write(byte value)
        {
            if (_ptr + 1 >= _bufferSize) { Resize(1); }
            _items[_ptr] = value;
            _ptr++;
        }
        public void Write(sbyte value)
        {
            if (_ptr + 1 >= _bufferSize) { Resize(1); }
            _items[_ptr] = (byte)value;
            _ptr++;
        }
        public void Write(short value)
        {
            if (_ptr + 2 >= _bufferSize) { Resize(2); }
            _items[_ptr++] = (byte)value;
            _items[_ptr++] = (byte)(value >> 8);
        }
        public void Write(ushort value)
        {
            if (_ptr + 2 >= _bufferSize) { Resize(2); }
            _items[_ptr++] = (byte)value;
            _items[_ptr++] = (byte)(value >> 8);
        }
        public void Write(int value)
        {
            if (_ptr + 4 >= _bufferSize) { Resize(4); }
            _items[_ptr++] = (byte)value;
            _items[_ptr++] = (byte)(value >> 8);
            _items[_ptr++] = (byte)(value >> 16);
            _items[_ptr++] = (byte)(value >> 24);
        }
        public void Write(uint value)
        {
            if (_ptr + 4 >= _bufferSize) { Resize(4); }
            _items[_ptr++] = (byte)value;
            _items[_ptr++] = (byte)(value >> 8);
            _items[_ptr++] = (byte)(value >> 16);
            _items[_ptr++] = (byte)(value >> 24);
        }
        public void Write(long value)
        {
            if (_ptr + 8 >= _bufferSize) { Resize(8); }
            _items[_ptr++] = (byte)value;
            _items[_ptr++] = (byte)(value >> 8);
            _items[_ptr++] = (byte)(value >> 16);
            _items[_ptr++] = (byte)(value >> 24);
            _items[_ptr++] = (byte)(value >> 32);
            _items[_ptr++] = (byte)(value >> 40);
            _items[_ptr++] = (byte)(value >> 48);
            _items[_ptr++] = (byte)(value >> 56);
        }
        public void Write(ulong value)
        {
            if (_ptr + 8 >= _bufferSize) { Resize(8); }
            _items[_ptr++] = (byte)value;
            _items[_ptr++] = (byte)(value >> 8);
            _items[_ptr++] = (byte)(value >> 16);
            _items[_ptr++] = (byte)(value >> 24);
            _items[_ptr++] = (byte)(value >> 32);
            _items[_ptr++] = (byte)(value >> 40);
            _items[_ptr++] = (byte)(value >> 48);
            _items[_ptr++] = (byte)(value >> 56);
        }
        public void Write(float value)
        {
            if (_ptr + 4 >= _bufferSize) { Resize(4); }
            FloatInt fi = new FloatInt(); fi.fvalue = value;
            int val = fi.ivalue;
            _items[_ptr++] = (byte)val;
            _items[_ptr++] = (byte)(val >> 8);
            _items[_ptr++] = (byte)(val >> 16);
            _items[_ptr++] = (byte)(val >> 24);

        }
        public void Write(double value)
        {
            if (_ptr + 8 >= _bufferSize) { Resize(8); }
            DoubleLong dl = new DoubleLong(); dl.dvalue = value;
            long val = dl.lvalue;
            _items[_ptr++] = (byte)val;
            _items[_ptr++] = (byte)(val >> 8);
            _items[_ptr++] = (byte)(val >> 16);
            _items[_ptr++] = (byte)(val >> 24);
            _items[_ptr++] = (byte)(val >> 32);
            _items[_ptr++] = (byte)(val >> 40);
            _items[_ptr++] = (byte)(val >> 48);
            _items[_ptr++] = (byte)(val >> 56);
        }
        public void Write(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                byte[] b = Encoding.UTF8.GetBytes(value);
                int blenght = b.Length;
                Write(blenght);
                if (_ptr + blenght >= _bufferSize) { Resize(blenght); }
                Buffer.BlockCopy(b, 0, _items, _ptr, blenght);
                _ptr += blenght;
            }
            else { Write(0); }
        }

        public void Write(byte[] value)
        {
            if (value == null) return;
            int lenght = value.Length;
            Write(lenght);

            if (_ptr + lenght >= _bufferSize) { Resize(lenght); }
            Buffer.BlockCopy(value, 0, _items, _ptr, lenght);
            _ptr += lenght;
        }

        public void WritePacketAsIs(byte[] packet)
        {
            if (packet == null) return;

            int lenght = packet.Length;
            if (_ptr + lenght >= _bufferSize) { Resize(lenght); }
            Buffer.BlockCopy(packet, 0, _items, _ptr, lenght);
            _ptr += lenght;
        }
    }
}

