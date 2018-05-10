using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using VigenereCipher;

namespace web.Controllers
{
    [RoutePrefix("api/vigenere")]
    public class VigenereCipherController : ApiController
    {
        private const string englishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string russianAlphabed = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private const string en = "en";
        private const string ru = "ru";
        private Dictionary<string, string> alphabets;

        VigenereCipherController() {
            alphabets = new Dictionary<string, string>()
            {
                { "en", englishAlphabet},
                { "ru", russianAlphabed}
            };
        }

        [Route("encode"), HttpGet]
        public string EncodeText([FromUri] string keyword, [FromUri] string text, [FromUri] string lang = en)
        {
            Cipher cipher = new Cipher(alphabets[lang].ToCharArray());
            return cipher.Encode(text, keyword);
        }

        [Route("decode"), HttpGet]
        public string DecodeText([FromUri] string keyword, [FromUri] string text, [FromUri] string lang = en)
        {
            Cipher cipher = new Cipher(alphabets[lang].ToCharArray());
            return cipher.Decode(text, keyword);
        }

        [Route("kasiski"), HttpGet]
        public IEnumerable<Dictionary<string, double>> DencodeText([FromUri] string text)
        {
            Kasiski kasiski = new Kasiski();
            return kasiski.Decode(text).Select(tpl => new Dictionary<string, double>() {
                { "size", tpl.Item1 },
                { "count", tpl.Item2 },
                { "probability", tpl.Item3}
            });
        }
    }
}
