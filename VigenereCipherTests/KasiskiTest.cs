using NUnit.Framework;
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
            Dictionary<string, List<int>> check = kasiski.Decode("LFWKIMJCLPSISWKHJOGLKMVGURAGKMKMXMAMJCVXWUYLGGIISW"+
                                          "ALXAEYCXMFKMKBQBDCLAEFLFWKIMJCGUZUGSKECZGBWYMOACFV"+
                                          "MQKYFWXTWMLAIDOYQBWFGKSDIULQGVSYHJAVEFWBLAEFLFWKIM"+
                                          "JCFHSNNGGNWPWDAVMQFAAXWFZCXBVELKWMLAVGKYEDEMJXHUXD"+
                                          "AVYXL");

            Assert.IsNotNull(check);
        }
    }
}
