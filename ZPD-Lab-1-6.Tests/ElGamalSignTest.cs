using System;
using System.Numerics;
using Xunit;
using ZPD_Lab_1_6.SignatureAlgorithms;
using ZPD_Lab_1_6.HashFunctions;

namespace ZPD_Lab_1_6.Tests
{
    public class ElGamalSignTest
    {
        [Theory]
        [InlineData("Roujou StunGun no Dengeki ga Utsu Gunshuu no Kage Yai Yai to Hito wa Yuki")]
        public void CheckSign_Message_ShouldReturnTrue(string message)
        {
            ElGamalSign signer = new ElGamalSign(251, 2, 16, 17);
            (int r, int s) = signer.Sign(message);

            bool result = signer.CheckSign(message, r, s);


            Assert.True(result);

        }
    }
}
