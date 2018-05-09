using NUnit.Framework;
using System;
using System.Collections.Generic;
using VigenereCipher;

namespace VigenereCipherTests
{
    [TestFixture]
    class KasiskiTest
    {
        [Test]
        public void TestEncode()
        {
            Kasiski kasiski = new Kasiski();
            IEnumerable<ValueTuple<int, int, double>> check = kasiski.Decode("LFWKIMJCLPSISWKHJOGLKMVGURAGKMKMXMAMJCVXWUYLGGIISW"+
                                          "ALXAEYCXMFKMKBQBDCLAEFLFWKIMJCGUZUGSKECZGBWYMOACFV"+
                                          "MQKYFWXTWMLAIDOYQBWFGKSDIULQGVSYHJAVEFWBLAEFLFWKIM"+
                                          "JCFHSNNGGNWPWDAVMQFAAXWFZCXBVELKWMLAVGKYEDEMJXHUXD"+
                                          "AVYXL");

            Assert.IsNotNull(check);
        }
    }
}
