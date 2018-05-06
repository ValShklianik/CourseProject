using System;
using NUnit.Framework;
using VigenereCipher;

namespace VigenereCipherTests
{
    [TestFixture]
    public class CipherTests
    {
        [Test]
        public void TestEncode()
        {
            Cipher cipher = new Cipher();
            string check = cipher.Encode("Lera is beautiful", "nikita");

            Assert.AreEqual("ymbibsomkcmiscv".ToUpper(), check);
        }
    }
}
