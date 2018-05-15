using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VigenereCipher
{
    class Delimeter
    {
        public string Value { get; set; }
        public int PreviousIndex { get; set; }
        public int Index { get; set; }
        public int Key { get; set; }
    }

    public class Cipher
    {
        private int alphabetSize;
        private char[] characters;

        public Cipher(char[] alphabet)
        {
            characters = alphabet;
            alphabetSize = characters.Length;
        }

        private string RestoreDelimeters(char[] text, List<Delimeter> delimeters)
        {
            string result = new String(text);

            foreach (var delimeter in delimeters)
            {
                result = result.Insert(delimeter.Index, delimeter.Value);
            }
            return result;
        }

        private List<Delimeter> GetDelimeters(string text, string pattern)
        {
            var delimitersMatch = new Regex(pattern).Match(text);
            List<Delimeter> delimeters = new List<Delimeter>();
            int previousIndex = -1;
            int count = 0;
            while (delimitersMatch.Success)
            {
                delimeters.Add(new Delimeter()
                {
                    Value = delimitersMatch.Value,
                    Index = delimitersMatch.Index,
                    PreviousIndex = previousIndex,
                    Key = count
                });
                count++;
                previousIndex = delimitersMatch.Index;
                delimitersMatch = delimitersMatch.NextMatch();
            }
            return delimeters;
        }

        private string Code(string input, string keyword, Func<char, char, char> resultSelector)
        {
            var pattern = "[^" + new String(characters).ToUpper() + "]";
            var upperedText = input.ToUpper();
            var delimeters = GetDelimeters(upperedText, pattern);
            string inputText = Regex.Replace(upperedText, pattern, "");
            int repeatCount = inputText.Length / keyword.Length + 1;
            string ciph = string.Join("", string.Join("", Enumerable.Repeat(keyword.ToUpper(), repeatCount)).Take(inputText.Length));

            char[] result = inputText.ToUpper().Zip(ciph, resultSelector).ToArray();

            return RestoreDelimeters(result, delimeters);
        }

        public string Encode(string input, string keyword)
        {
            return Code(input, keyword, (char i, char k) => {
                int c = (Array.IndexOf(characters, i) +
                    Array.IndexOf(characters, k)) % alphabetSize;
                    
                return characters[c];
            });
        }

        //расшифровать
        public string Decode(string input, string keyword)
        {
            return Code(input, keyword, (char i, char k) => {
                int c = (Array.IndexOf(characters, i) + alphabetSize -
                    Array.IndexOf(characters, k)) % alphabetSize;
                return characters[c];
            });
        }

    }
}
