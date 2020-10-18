using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ZPD_Lab_1_6.HashFunctions
{
    public class GostHash
    {
        public BigInteger CalculateHash(char[] message)
        {
            BitArray messageBits = ConvertToBitArray(message);
            BigInteger l = 0;
            BitArray i = new BitArray(256);
            BitArray gost = new BitArray(256);
            GOSTCipher cipher = new GOSTCipher();

            if (messageBits.Length > 256)
            {
                (BitArray leftPart, BitArray rightPart) = Split(messageBits);
                gost = cipher.Encode(rightPart, gost);
                l = (l + 256) % BigInteger.Pow(2, 256);
                i = i.Xor(rightPart);
                messageBits = leftPart; 
            }

            l = (messageBits.Length + l) % BigInteger.Pow(2, 256);
            i = i.Xor(Concat(new BitArray(256 - messageBits.Length), messageBits));

            gost = cipher.Encode(Concat(new BitArray(256 - messageBits.Length), messageBits), gost);
            gost = cipher.Encode(i, gost);

            BigInteger endValue = GetFinalValue(gost);

            return endValue;
        }

        private BigInteger GetFinalValue(BitArray bitArray)
        {

            bool[] bits = new bool[256];
            bitArray.CopyTo(bits, 0);

            BigInteger result = 0;
            for (int i = 0; i < 256; i++)
            {
                if (bits[i])
                {
                    result += ((BigInteger)1) << i;
                }
            }

            return result;
        }
        private BitArray ConvertToBitArray(char[] message)
        {
            byte[] bytes = message.Reverse().Select(c => (byte)c).ToArray();
            if( bytes.Length > 64)
            {
                bytes = bytes.SkipLast(bytes.Length - 64).ToArray();
            }

            BitArray bitArray = new BitArray(bytes);
            return bitArray;

        }

        private (BitArray, BitArray) Split(BitArray bitArray)
        {
            bool[] rightPart = new bool[256];
            for(int i = 0; i < 256; i ++)
            {
                rightPart[i] = bitArray[i];
            }
            BitArray rightBits = new BitArray(rightPart);

            bool[] leftPart = new bool[bitArray.Length - 256];
            for (int i = 256; i < bitArray.Length; i++)
            {
                leftPart[i - 256] = bitArray[i];
            }
            BitArray leftBits = new BitArray(leftPart);

            return (leftBits, rightBits);
        }

        private BitArray Concat(BitArray start, BitArray end)
        {
            bool[] startBits = new bool[start.Length];
            start.CopyTo(startBits, 0);

            bool[] endBits = new bool[end.Length];
            end.CopyTo(endBits, 0);

            startBits = startBits.Concat(endBits).ToArray();

            return new BitArray(startBits);
        }

    }
}
