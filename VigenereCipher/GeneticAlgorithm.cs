using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VigenereCipher
{
    public class GeneticAlgorithm
    {
        private Dictionary<char, double> frequencyDictionary;
        private int keywordLength;
        private char[] alphabet;
        private Random rand = new Random();
        private string text;
        private Cipher cipher;

        public GeneticAlgorithm(Dictionary<char, double> frequencyDictionary, int keywordLength, string text)
        {
            this.frequencyDictionary = frequencyDictionary;
            this.keywordLength = keywordLength;
            this.alphabet = frequencyDictionary.Keys.ToArray();
            this.text = Regex.Replace(text.ToUpper(), "[^" + alphabet + "]", "");
            this.cipher = new Cipher(this.alphabet);
        }

        Dictionary<char, int> GetFrequencyDictionary(string decoded)
        {
            int textLength = decoded.Length;
            return frequencyDictionary.ToDictionary(p => p.Key, p => decoded.Count(c => c == p.Value) / textLength);
        }

        private double Fitness(string ind)
        {
            var decoded = cipher.Decode(text, ind);
            return GetFrequencyDictionary(decoded).Sum(pair => Math.Abs(pair.Value - frequencyDictionary[pair.Key]));
        }

        private IEnumerable<string> Reproduction(string ind1, string ind2)
        {
            yield return string.Join("", ind1.Zip(ind2, (i1, i2) => rand.NextDouble() > 0.5 ? i1 : i2));
            yield return string.Join("", ind2.Zip(ind1, (i1, i2) => rand.NextDouble() > 0.5 ? i1 : i2));

        }

        private string Mutation(string ind)
        {
            int i = rand.Next(0, ind.Length);
            return ind.Replace(ind[i], alphabet[rand.Next(0, alphabet.Length)]);
        }

        private IEnumerable<IEnumerable<string>> GetPopulation(IEnumerable<string> initial)
        {
            IEnumerable<string> population = initial;
            int size = initial.Count();

            while (true)
            {
                yield return population;
                population = population
                                .SelectMany(ind => this.Reproduction(ind, population.Skip(rand.Next(population.Count())).First()), (ind, child) => this.Mutation(child))
                                .Concat(population)
                                .OrderBy(ind => this.Fitness(ind))
                                .Take(size)
                                .ToList();
            }

        }

        string GetRandomWord()
        {
            return string.Join("", Enumerable.Range(0, keywordLength).Select(i => alphabet[rand.Next(0, alphabet.Length)]));
        }

        public IEnumerable<string> GetKeywords()
        {
            int size = 25;

            IEnumerable<string> population = Enumerable.Range(0, size).Select(i => GetRandomWord());

            return this.GetPopulation(population).First(pop => pop.Any(ind => this.Fitness(ind) < 0.95)).Distinct();
        

        }
    }
}
