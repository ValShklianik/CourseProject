using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VigenereCipher
{
   public class Kasiski
   {
        private string alphabet = "";
        public Kasiski() {}
        public Kasiski(string alphabet)
        {
            this.alphabet = alphabet;
        }

        private Dictionary<string, List<int>> GetCoordinatesDict(string text, int len)
        {
            return Enumerable
                .Range(0, text.Length - len)
                .GroupBy(index => text.Substring(index, len))
                .ToDictionary(qs => qs.Key, value => GetDistances(value.ToList()));
        }

        private List<int> GetDistances(List<int> coords)
        {
            List<int> distances = new List<int>();
            for (int i = 1; i < coords.Count; i++)
            {
                distances.Add(coords[i] - coords[i - 1]);
            }
            return distances;
        }

        private IEnumerable<int> GetDivisors(int number)
        {
            for (int i = 2; i < Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                {
                    yield return number / i;
                    yield return i;
                }
            }
            int sqrt = (int)Math.Sqrt(number);
            if (number % sqrt == 0) yield return sqrt;
        }

        private int NOD(int firstEl, int secondEl)
        {
            while (firstEl != secondEl)
            {
                if (firstEl > secondEl)
                {
                    firstEl = firstEl - secondEl;
                }
                else
                {
                    secondEl = secondEl - firstEl;
                }
            }
            return firstEl;
        }

        private string PrepareText(string text)
        {
            return Regex.Replace(text.ToUpper(), "[^" + alphabet + "]", "");
        }

        public int GetIndex(char c)
        {
            for (int i = 0; i < alphabet.Length; i++)
            {
                if (alphabet[i] == c)
                    return i;
            }

            return 0;
        }

        public string GetKeyword(string text, int length, char mostPopular)
        {
            char[,] strings = new char[500000, length];

            text = PrepareText(text);

            for (int i = 0; i < text.Length; i++)
            {
                int ii = i / length;
                int ij = i - (ii * length);

                strings[ii, ij] = text[i];
            }

            for (int i = 0; i < (text.Length / length) + 1; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.Write(strings[i, j] + " ");
                }
                Console.WriteLine();
            }

            string password = "";

            for (int i = 0; i < length; i++)
            {
                Dictionary<char, int> dict = new Dictionary<char, int>();

                for (int j = 0; j < (text.Length / length) + 1; j++)
                    if (dict.ContainsKey(strings[j, i]))
                        dict[strings[j, i]]++;
                    else
                        dict.Add(strings[j, i], 1);

                var sortict = dict.OrderByDescending(x => x.Value);

                char sy = mostPopular;
                foreach (KeyValuePair<char, int> kvp in sortict)
                {
                    sy = kvp.Key;
                    break;
                }

                int res = (GetIndex(sy) - GetIndex(mostPopular) + alphabet.Length) % alphabet.Length;

                password += alphabet[res];

            }
            return password;
        }

        public IEnumerable<Tuple<int, double>> Decode(string text)
        {
            const int minWordLength = 3,
                      maxWordLength = 15;
            var divisors = Enumerable
                    .Range(minWordLength, maxWordLength - minWordLength)
                    .SelectMany(len => GetCoordinatesDict(PrepareText(text), len).Where(pair => pair.Value.Count > 0 && pair.Value.Count(d => d == pair.Value.First()) == pair.Value.Count))
                    .SelectMany(pair => pair.Value)
                    .Distinct()
                    .SelectMany(distance => GetDivisors(distance))
                    .Where(divisor => divisor >= minWordLength && divisor <= maxWordLength);
            return divisors
                .Select(divisor => (divisor: divisor, count: divisors.Count(d => d == divisor)))
                .Distinct()
                .OrderBy(tpl => -tpl.count)
                .Select(tpl => Tuple.Create(tpl.divisor, (double)tpl.count / divisors.ToList().Count));
        }
    }
}
