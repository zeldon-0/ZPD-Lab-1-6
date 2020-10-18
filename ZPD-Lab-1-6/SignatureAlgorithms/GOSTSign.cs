using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ZPD_Lab_1_6.HashFunctions;

namespace ZPD_Lab_1_6.SignatureAlgorithms
{
    public class GOSTSign
    {
        private int _p = 67;
        private int _q = 11;
        private int _a;
        private int _x;
        private int _y;
        private int _k;

        public GOSTSign(int k, int x)
        {
            _k = k;
            _x = x;
            CalculateA();
            _y = (int) Math.Pow(_a, _x) % _p;
        }

        public (int, BigInteger) Sign(string message)
        {
            BigInteger m = new GostHash().CalculateHash(message.ToCharArray());
            int w = (int) (BigInteger.Pow(_a, _k) % _p);
            w = w % _q;
            BigInteger s = (_x * w + _k * m) % _q;

            return (w, s);
        }

        public bool CheckSignature(string message, int w, BigInteger s)
        {
            BigInteger m = new GostHash().CalculateHash(message.ToCharArray());
            BigInteger v = BigInteger.Pow(m, _q - 2) % _q;
            int z1 = (int) ((s * v) % _q);
            int z2 = (int) (((_q - w) * v) % _q);
            BigInteger u = ((BigInteger.Pow(_a, z1) * BigInteger.Pow(_y, z2)) % _p % _q);

            return w == u;

        }

        private void CalculateA()
        {
            int a = 0;
            while (a < _p - 1)
            {
                if (BigInteger.Pow(a, _q) == 1)
                {
                    _a = a;
                    return;
                }
                a++;
            }
        }
    }
}
