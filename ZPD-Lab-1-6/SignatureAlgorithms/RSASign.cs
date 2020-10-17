using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ZPD_Lab_1_6.HashFunctions;

namespace ZPD_Lab_1_6.SignatureAlgorithms
{
    public class RSASign
    {
        private const int _p = 23;
        private const int _q = 7;
        private const int _e = 41;
        private const int _m = 16;


        private int _n;
        private int _euler;
        private int _d;
        public RSASign()
        {
            _n = _p * _q;
            _euler = (_p - 1) * (_q - 1);
            _d = _modInverse(_e, _euler);
        }
        public BigInteger Sign(string message)
        {
            BigInteger m = new MD5Hash().CalculateHash(message.ToCharArray());
            BigInteger s = BigInteger.Pow(m, _d) % _n;

            return s;
        }

        public bool CheckSign(BigInteger s, string message)
        {
            BigInteger hashM = new MD5Hash().CalculateHash(message.ToCharArray()) % _n;
            BigInteger m = BigInteger.Pow(s, _e) % _n;


            return m == hashM;
        }



        private int _modInverse(int a, int m)
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
