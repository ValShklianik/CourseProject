using VigenereCipher;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class CodingTextService
    {
        public string EncodeText(string text, string keyword, string alphabet)
        {
            var repository = new Repository();
            var result = repository.GetEncodedText(text, keyword);

            if (ReferenceEquals(result, null))
            {
                var cipher = new Cipher(alphabet.ToCharArray());
                var encoded = cipher.Encode(text, keyword);
                result = repository.AddEncodedText(text, encoded, keyword);
            }
            return result.Value;
        }

        public string DecodeText(string text, string keyword, string alphabet)
        {
            var repository = new Repository();
            var result = repository.GetDecodedText(text, keyword);

            if (ReferenceEquals(result, null))
            {
                var cipher = new Cipher(alphabet.ToCharArray());
                var encoded = cipher.Decode(text, keyword);
                result = repository.GetDecodedText(encoded, keyword);
            }
            return result.Value;
        }

        public IEnumerable<ValueTuple<int, double>> GetKasiskiResult(string encodedText, string alphabet)
        {
            var repository = new Repository();
            var result = repository.GetKasiskiResult(encodedText);

            if (ReferenceEquals(result, null))
            {
                var kasiski = new Kasiski(alphabet);
                var res = kasiski.Decode(encodedText);
                result = repository.AddKasiskiResult(encodedText, res);
            }
            return result.Results.Select(item => (item.Size, item.Probability));
        }
    }
}
