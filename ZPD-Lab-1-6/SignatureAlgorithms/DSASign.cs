using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ZPD_Lab_1_6.HashFunctions;

namespace ZPD_Lab_1_6.SignatureAlgorithms
{
    public class DSASign
    {
        private int _p = 251;
        private int _q = 25;
        private int _k;
        private int _x;
        private int _g;
        private int _y;
        public DSASign(int k, int x, int g)
        {
            _k = k;
            _x = x;
            _g = g;

            _y = (int)(BigInteger.Pow(g, k) % _p);

        }

        public (int, int) Sign(string message)
        {
            int r = (int)(BigInteger.Pow(_g, _x) % _p % _q);
            int xInverse = ModInverse(_x, _q);
            BigInteger m = new SHA1Hash().CalculateHash(message.ToCharArray());
            int s = (int)(BigInteger.Multiply(xInverse, (m + _k * r) ) % _q);

            return (r, s);
        }

        public bool CheckSign(string message, int r, int s)
        {
            int w = ModInverse(s, _q);
            BigInteger m = new SHA1Hash().CalculateHash(message.ToCharArray());
            int u1 = (int) ((m * w) % _q);
            int u2 = (r * w) % _q;

            int v = (int) ((BigInteger.Pow(_g, u1) * BigInteger.Pow(_y, u2)) % _p % _q);

            return v == r;
        }

        private int ModInverse(int a, int m)
        {
            a = a % m;

            if (a < 0)
                a = a + m;
            for (int x = 1; x < m; x++)
                if ((a * x) % m == 1)
                    return x;
            return 1;
        }
    }
}
