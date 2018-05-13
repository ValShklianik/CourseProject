using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using VigenereCipher;

namespace web.Controllers
{
    public class VigenereParams
    {
        private const string englishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string alphabet;
        public string Text { get; set; }
        public string Keyword { get; set; }
        public string Alphabet
        {
            get
            {
                if (string.IsNullOrEmpty(alphabet)) return englishAlphabet;
                return alphabet;
            }
            set
            {
                alphabet = value;
            }
        }
    }


    [RoutePrefix("api/vigenere")]
    public class VigenereCipherController : ApiController
    {
        [Route("encode"), HttpPost]
        public string EncodeText([FromBody] VigenereParams args)
        {
            Cipher cipher = new Cipher(args.Alphabet.ToCharArray());
            return cipher.Encode(args.Text, args.Keyword);
        }

        [Route("decode"), HttpPost]
        public string DecodeText([FromBody] VigenereParams args)
        {
            Cipher cipher = new Cipher(args.Alphabet.ToCharArray());
            return cipher.Decode(args.Text, args.Keyword);
        }

        [Route("kasiski"), HttpPost]
        public IEnumerable<Dictionary<string, double>> MakeKasiskiMethod([FromBody] VigenereParams args)
        {
            Kasiski kasiski = new Kasiski(args.Alphabet);
            return kasiski.Decode(args.Text).Select(tpl => new Dictionary<string, double>() {
                { "size", tpl.Item1 },
                { "count", tpl.Item2 },
                { "probability", tpl.Item3}
            });
        }
    }
}
