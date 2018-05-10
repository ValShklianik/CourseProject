﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace VigenereCipher
{
    public class Cipher
    {
        private int alphabetSize;
        private char[] characters;

        public Cipher(char[] alphabet)
        {
            characters = alphabet;
            alphabetSize = characters.Length;
        }

        private string Code(string input, string keyword, Func<char, char, char> resultSelector)
        {
            string inputText = Regex.Replace(input.ToUpper(), "[^" + new String(characters) + "]", "");
            int repeatCount = inputText.Length / keyword.Length + 1;
            string ciph = string.Join("", string.Join("", Enumerable.Repeat(keyword.ToUpper(), repeatCount)).Take(inputText.Length));

            char[] result = inputText.ToUpper().Zip(ciph, resultSelector).ToArray();

            return string.Join("", result);
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
