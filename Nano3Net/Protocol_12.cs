/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 27.07.2016 15:15:02
*/

using System;

namespace Nano3.Net
{
    public class Protocol_12 : IProtocol
    {
        private const int _HEADER_LENGTH = 12;
        private static readonly long BWORDKEY = 118871556707436796;

        private readonly long _startWord = 0;
        private readonly byte[] _bword_Array;

        public int HeaderLenght
        {
            get { return _HEADER_LENGTH; }
        }

        public Protocol_12()
        {
            _startWord = BWORDKEY;
            _bword_Array = BytePacketReader.GetBytes(_startWord);
        }
        public Protocol_12(long startWord)
        {
            _bword_Array = BytePacketReader.GetBytes(_startWord);
        }

        public byte[] GetPacket(ref byte[] buffer)
        {
            int lengthVal = 0;
            int packetLenght = 0;
            int ptr = 0;
            if (buffer.Length > _HEADER_LENGTH)
            {
                long bword = BytePacketReader.ReadLong(buffer, ref ptr);

                if (bword == _startWord)
                {
                    lengthVal = BytePacketReader.ReadInt(buffer, ref ptr);
                    packetLenght = int.MaxValue & lengthVal;
                    int compressed = 1 & lengthVal << 31;
                }
            }
            else { return null; }
            if (buffer.Length < _HEADER_LENGTH + packetLenght) { return null; }

            byte[] packet = new byte[packetLenght];
            Buffer.BlockCopy(buffer, _HEADER_LENGTH, packet, 0, packetLenght);

            int newBufferLenght = buffer.Length - (_HEADER_LENGTH + packetLenght);
            if (newBufferLenght > 0)
            {
                byte[] newBuffer = new byte[newBufferLenght];
                Buffer.BlockCopy(buffer, _HEADER_LENGTH + packetLenght, newBuffer, 0, newBufferLenght);
                buffer = newBuffer;
            }
            else { buffer = new byte[0]; }
            return packet;
        }

        public void SignPacket(ref byte[] packet)
        {
            if (packet == null) return;
            if (packet.Length <= _HEADER_LENGTH) return;
            int ptr = 0;

            long bword = BytePacketReader.ReadLong(packet, ref ptr);
            int pLenght = BytePacketReader.ReadInt(packet, ref ptr);
            if (bword == 0 && pLenght == 0)
            {
                Buffer.BlockCopy(_bword_Array, 0, packet, 0, 8);
                byte[] b_packetL = BytePacketReader.GetBytes(packet.Length - _HEADER_LENGTH);
                Buffer.BlockCopy(b_packetL, 0, packet, 8, b_packetL.Length);
            }
            else
            {
                byte[] newPack = new byte[packet.Length + _HEADER_LENGTH];
                byte[] packLength = BytePacketReader.GetBytes(packet.Length);

                Buffer.BlockCopy(_bword_Array, 0, newPack, 0, _bword_Array.Length);
                Buffer.BlockCopy(packLength, 0, newPack, 8, packLength.Length);
                Buffer.BlockCopy(packet, 0, newPack, _HEADER_LENGTH, packet.Length);
                packet = newPack;
            }
        }
    }
}
