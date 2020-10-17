using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace ZPD_Lab_1_6.HashFunctions
{
    public class MD5Hash
    {
        private uint _a = 0x67452301;
        private uint _b = 0xefcdab89;
        private uint _c = 0x98badcfe;
        private uint _d = 0x10325476;

        private int[,] _s = new int[4, 4]
        {
            { 7, 12, 17, 22 },
            { 5,  9, 14, 20 },
            { 4, 11, 16, 23 },
            { 6, 10, 15, 21 }
        };


        private uint CalculateK(int i, int r)
        {
            return (uint) Math.Pow(2, 32) * (uint) Math.Abs(Math.Sin(i + 16 * (r - 1) ) );
        }

        public BigInteger CalculateHash(char[] message)
        {
            BitArray messageBits = ConvertToBitArrayAndFill(message);

            for (int i = 0; i < messageBits.Length; i += 512)
            {
                BitArray bitBlock = GetBlockBits(messageBits, i );

                uint a = _a;
                uint b = _b;
                uint c = _c;
                uint d = _d;

                for (int j = 0; j < 64; j ++)
                {
                    uint rf;
                    int r;


                    if (j < 16)
                    {
                        rf = (b & c) | ((~b) & d);
                        r = 1;
                    }
                    else if (j < 32)
                    {
                        rf = (d & b) | ((~d) & c);
                        r = 2;
                    }
                    else if (j < 48)
                    {
                        rf = b ^ c ^ d;
                        r = 3;
                    }
                    else
                    {
                        rf = c ^ (b | (~d));
                        r = 4;
                    }

                    int messageBlockIndex = j % 16;

                    a = a ^ rf ^ GetBlockValue(messageBlockIndex, bitBlock) ^ CalculateK(messageBlockIndex, r);
                    a = LeftShift(a, _s[r - 1, j % 4]);
                    a = a ^ b;
                    c = b;
                    d = c;
                    b = a;
                    a = d;

                }

                _a = _a ^ a;
                _b = _b ^ b;
                _c = _c ^ c;
                _d = _d ^ d;
            }

            return GetFinalDecimalValue();

        }

        private BitArray ConvertToBitArrayAndFill(char[] message)
        {

            byte[] bytes = message.Reverse().Select(c => (byte)c).ToArray();
            BitArray bitArray = new BitArray(bytes);

            int fillSize = 448 -  (bitArray.Length + 1) % 512 ;
            bool[] fillBits = new bool[fillSize + 1];
            fillBits[fillSize] = true;

            bool[] bits = new bool[bytes.Length * 8];
            bitArray.CopyTo(bits, 0);
            fillBits = fillBits.Concat(bits).ToArray();

            ulong size = (ulong) message.Length;
            byte[] sizeBytes = BitConverter.GetBytes(size);
            bitArray = new BitArray(sizeBytes);
            bool[] sizeBits = new bool[64];
            bitArray.CopyTo(sizeBits, 0);
            sizeBits = sizeBits.Concat(fillBits).ToArray();

            bitArray = new BitArray(sizeBits);
            return bitArray;

        }

        private uint GetBlockValue (int i, BitArray bitBlock)
        {
            int startingIndex = (15 - i) * 32;
            uint value = 0;

            for (int j = startingIndex; j < startingIndex + 32; j++)
            {
                if (bitBlock[j])
                {
                    value += (uint)(1 << j % 32);
                }
            }

            return value;
        }

        private uint LeftShift(uint value, int shift)
        {
            return (value << shift) | (value >> (32 - shift));
        }

        private BitArray GetBlockBits(BitArray bits, int index)
        {
            bool[] blockBits = new bool[512];
            int startingIndex = bits.Length - index - 512;

            for(int i = startingIndex; i < startingIndex + 512; i++)
            {
                blockBits[i % 512] = bits[i];
            }

            return new BitArray(bits);
        }

        private BigInteger GetFinalDecimalValue()
        {
            
            byte[] dBytes = BitConverter.GetBytes(_d);
            BitArray bitArray = new BitArray(dBytes);
            bool[] bits = new bool[128];
            bitArray.CopyTo(bits, 0);

            byte[] cBytes = BitConverter.GetBytes(_c);
            bitArray = new BitArray(cBytes);
            bitArray.CopyTo(bits, 32);

            byte[] bBytes = BitConverter.GetBytes(_b);
            bitArray = new BitArray(bBytes);
            bitArray.CopyTo(bits, 64);

            byte[] aBytes = BitConverter.GetBytes(_a);
            bitArray = new BitArray(aBytes);
            bitArray.CopyTo(bits, 96);

            BigInteger result = 0;
            for (int i = 0; i < 128; i++)
            {
                if (bits[i])
                {
                    result += (1 << i);
                }
            }

            return result;
        }

    }
}
