using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;

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

    public class KasiskiParams
    {
        private const string englishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string alphabet;
        private char mostPopular;
        public string Text { get; set; }
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
        public int Length { get; set; }
        public char MostPopular
        {
            get
            {
                if (mostPopular == 0) return 'E';
                return mostPopular;
            }
            set => mostPopular = value;
        }
    }


    [RoutePrefix("api/vigenere")]
    public class VigenereCipherController : ApiController
    {
        private CodingTextService servie;
        public VigenereCipherController()
        {
            servie = new CodingTextService();
        }

        [Route("encode"), HttpPost]
        public string EncodeText([FromBody] VigenereParams args)
        {
            return servie.EncodeText(args.Text, args.Keyword, args.Alphabet);
        }

        [Route("decode"), HttpPost]
        public string DecodeText([FromBody] VigenereParams args)
        {
            return servie.DecodeText(args.Text, args.Keyword, args.Alphabet);
        }

        [Route("kasiski"), HttpPost]
        public IEnumerable<Dictionary<string, double>> MakeKasiskiMethod([FromBody] KasiskiParams args)
        {
            return servie.GetKasiskiResult(args.Text, args.Alphabet).Select(tpl => new Dictionary<string, double>() {
                { "size", tpl.Item1 },
                { "probability", tpl.Item2}
            });
        }

        [Route("get_keyword"), HttpPost]
        public string GetKeyword([FromBody] KasiskiParams args)
        {
            Kasiski kasiski = new Kasiski(args.Alphabet);
            return kasiski.GetKeyword(args.Text, args.Length, args.MostPopular);
        }
    }
}
