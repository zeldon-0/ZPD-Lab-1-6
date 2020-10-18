using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;
using ZPD_Lab_1_6.SignatureAlgorithms;

namespace ZPD_Lab_1_6.Tests
{
    public class GOSTSignTest
    {
        [Theory]
        [InlineData("Roujou StunGun no Dengeki ga Utsu Gunshuu no Kage Yai Yai to Hito wa Yuki")]
        [InlineData("Tobe Kagaku no ada Yuurei Hikouki Yuke jigoku no sata shouki wo taite Dare mo shiranu ma ni tobitatsu Ari mo senu sora wo KIMI e")]
        public void CheckSign_Message_ShouldReturnTrue(string message)
        {
            GOSTSign signer = new GOSTSign(5, 6);
            (int w, BigInteger s) = signer.Sign(message);

            bool result = signer.CheckSignature(message, w, s);


            Assert.True(result);

        }
    }
}
