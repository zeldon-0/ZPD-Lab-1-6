using System;
using System.Numerics;
using Xunit;
using ZPD_Lab_1_6.SignatureAlgorithms;
using ZPD_Lab_1_6.HashFunctions;

namespace ZPD_Lab_1_6.Tests
{
    public class RSASignTest
    {
        [Theory]
        [InlineData("Roujou StunGun no Dengeki ga Utsu Gunshuu no Kage Yai Yai to Hito wa Yuki")]
        public void CheckSign_Message_ShouldReturnTrue(string message)
        {
            RSASign signer = new RSASign();
            BigInteger value = signer.Sign(message);

            bool result = signer.CheckSign(value, message);


            Assert.True(result);

        }
    }
}
