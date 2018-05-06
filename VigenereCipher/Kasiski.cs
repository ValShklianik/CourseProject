using System;
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

        public Dictionary<string, List<int>> Decode(string text)
        {
            const int minWordLength = 3,
                      maxWordLength = 15;

            return Enumerable
                    .Range(minWordLength, maxWordLength - minWordLength)
                    .SelectMany(len => GetCoordinatesDict(text, len).Where(pair => pair.Value.Count > 1))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
