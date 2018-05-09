﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VigenereCipher
{
   public class Kasiski
    {
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

        public IEnumerable<ValueTuple<int, int, double>> Decode(string text)
        {
            const int minWordLength = 3,
                      maxWordLength = 15;
            var divisors = Enumerable
                    .Range(minWordLength, maxWordLength - minWordLength)
                    .SelectMany(len => GetCoordinatesDict(text, len).Where(pair => pair.Value.Count > 0 && pair.Value.Count(d => d == pair.Value.First()) == pair.Value.Count))
                    .SelectMany(pair => pair.Value)
                    .Distinct()
                    .SelectMany(distance => GetDivisors(distance))
                    .Where(divisor => divisor >= minWordLength && divisor <= maxWordLength);
            var pairs = divisors
                .Select(divisor => (divisor: divisor, count: divisors.Count(d => d == divisor)))
                .Distinct()
                .OrderBy(tpl => -tpl.count);
                return pairs.Select(tpl => (divisor: tpl.divisor, count: tpl.count, probability: (double)tpl.count / pairs.ToList().Count));
        }
    }
}
