using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ZPD_Lab_1_6.HashFunctions;

namespace ZPD_Lab_1_6.SignatureAlgorithms
{
    public class ElGamalSign
    {
        private int _p;
        private int _g;
        private int _x;
        private int _y;
        private int _a;
        private int _k;
        private int _m;

        public ElGamalSign(int p, int x, int k, int m)
        {
            _p = p;
            _x = x;
            _k = k;
            CalculateInitialRoot();

            _y = (int) (BigInteger.Pow(_g, _x) % _p);

            GenerateA();
        }

        private void CalculateInitialRoot()
        {
            for (int i = 1; i < _p; i++)
            {
                if (Math.Pow(i, (_p - 1)) % _p == 1)
                {
                    _g = i;
                    return;
                }
            }

        }

        public (int, int) Sign(string message)
        {
            BigInteger m = new SHA1Hash().CalculateHash(message.ToCharArray());
            int r = (int) (BigInteger.Pow(_g, _x) % _p);
            BigInteger u = (m - _k * r) % (_p - 1);

            int xInverse = ModInverse(_x, _p - 1);

            int s = (int) (xInverse * u) % (_p - 1);

            return (r, s);
        }

        public bool CheckSign(string message, int r, int s)
        {
            BigInteger m = new SHA1Hash().CalculateHash(message.ToCharArray());

            BigInteger leftSide = (BigInteger.Pow(_y, r) * BigInteger.Pow(r, s)) % _p;
            BigInteger rightSide = Power(_g, _m) % _p;

            return leftSide == rightSide;

        }


        private void GenerateA()
        {

            _a = (int)Math.Pow(_g, _k) % _p;
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

        private BigInteger Power(BigInteger value, BigInteger exponent)
        {
            BigInteger originalValue = value;
            while (exponent-- > 1)
            {
                value = BigInteger.Multiply(value, originalValue);
            }
            return value;
        }
    }
}
