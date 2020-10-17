using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ZPD_Lab_1_6.HashFunctions
{
    public class SHA1Hash
    {
        private uint _a = 0x67452301;
        private uint _b = 0xefcdab89;
        private uint _c = 0x98badcfe;
        private uint _d = 0x10325476;
        private uint _e = 0xc3d2e1f0;

        private uint[] _k = new uint[4]
        {
            0x5a827999,
            0x6ed9eba1,
            0x8F1bbcdc,
            0x8F1bbcdc
        };

        private uint[] _w = new uint[80];

        public SHA1Hash() { }

        public BigInteger CalculateHash(char[] message)
        {
            BitArray messageBits = ConvertToBitArrayAndFill(message);

            for (int i = 0; i < messageBits.Length; i += 512)
            {
                BitArray bitBlock = GetBlockBits(messageBits, i);

                uint a = _a;
                uint b = _b;
                uint c = _c;
                uint d = _d;
                uint e = _e;

                GenerateW(bitBlock);

                for (int j = 0; j < 80; j++)
                {
                    uint rf;
                    int r;


                    if (j < 20)
                    {
                        rf = (b & c) | ((~b) & d);
                        r = 1;
                    }
                    else if (j < 40)
                    {
                        rf = b ^ c ^ d;
                        r = 2;
                    }
                    else if (j < 60)
                    {
                        rf = (b & c) | (b & d) | (c & d);
                        r = 3;
                    }
                    else
                    {
                        rf = b ^ c ^ d;
                        r = 4;
                    }


                    uint newE = e ^ rf ^ LeftShift(a, 5) + _w[j] + _k[r - 1];
                    e = d;
                    d = c;
                    c = LeftShift(b, 30);
                    b = a;
                    a = newE;

                }

                _a = _a ^ a;
                _b = _b ^ b;
                _c = _c ^ c;
                _d = _d ^ d;
            }

            return GetFinal128BitValue();

        }

        private BitArray ConvertToBitArrayAndFill(char[] message)
        {

            byte[] bytes = message.Reverse().Select(c => (byte)c).ToArray();
            BitArray bitArray = new BitArray(bytes);

            int fillSize = 448 - (bitArray.Length + 1) % 512;
            bool[] fillBits = new bool[fillSize + 1];
            fillBits[fillSize] = true;

            bool[] bits = new bool[bytes.Length * 8];
            bitArray.CopyTo(bits, 0);
            fillBits = fillBits.Concat(bits).ToArray();

            ulong size = (ulong)message.Length;
            byte[] sizeBytes = BitConverter.GetBytes(size);
            bitArray = new BitArray(sizeBytes);
            bool[] sizeBits = new bool[64];
            bitArray.CopyTo(sizeBits, 0);
            sizeBits = sizeBits.Concat(fillBits).ToArray();

            bitArray = new BitArray(sizeBits);
            return bitArray;

        }

        private BitArray GetBlockBits(BitArray bits, int index)
        {
            bool[] blockBits = new bool[512];
            int startingIndex = bits.Length - index - 512;

            for (int i = startingIndex; i < startingIndex + 512; i++)
            {
                blockBits[i % 512] = bits[i];
            }

            return new BitArray(bits);
        }

        private uint Get32BitWordFromMessage(BitArray message, int index)
        {
            int startIngIndex = (15 - index) * 32;

            uint value = 0;
            for (int i = startIngIndex; i < startIngIndex + 32; i++)
            {
                if (message[i])
                {
                    value += (uint)(1 << i % 32);
                }
            }
            return value;
        }

        private uint LeftShift(uint value, int shift)
        {
            return (value << shift) | (value >> (32 - shift));
        }

        private void GenerateW(BitArray message)
        {
            for (int i = 0; i < 16; i++)
            {
                _w[i] = Get32BitWordFromMessage(message, i);
            }

            for(int j = 16; j < 80; j++)
            {
                _w[j] = LeftShift(_w[j - 3] ^ _w[j - 8] ^ _w[j - 14] ^ _w[j - 16], 1);  
            }
        }


        private BigInteger GetFinal128BitValue()
        {

            byte[] eBytes = BitConverter.GetBytes(_e);
            BitArray bitArray = new BitArray(eBytes);
            bool[] bits = new bool[160];
            bitArray.CopyTo(bits, 0);

            byte[] dBytes = BitConverter.GetBytes(_d);
            bitArray = new BitArray(dBytes);
            bitArray.CopyTo(bits, 32);

            byte[] cBytes = BitConverter.GetBytes(_c);
            bitArray = new BitArray(cBytes);
            bitArray.CopyTo(bits, 64);

            byte[] bBytes = BitConverter.GetBytes(_b);
            bitArray = new BitArray(bBytes);
            bitArray.CopyTo(bits, 96);

            byte[] aBytes = BitConverter.GetBytes(_a);
            bitArray = new BitArray(aBytes);
            bitArray.CopyTo(bits, 128);

            BigInteger result = 0;
            for (int i = 0; i < 128; i++)
            {
                if (bits[i])
                {
                    result += ((BigInteger)1) << i;
                }
            }

            return result;
        }

    }
}
