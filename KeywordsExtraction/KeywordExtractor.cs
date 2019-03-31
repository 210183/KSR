using Core.Models.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeywordsExtraction
{
    public static class KeywordExtractor
    {
        public static void Extract(List<(LabelsCollection labels, IEnumerable<string> words)> samples)
        {
            var labelGroups = samples.GroupBy(s => s.labels.Values.First().Name);
            foreach (var labelGroup in labelGroups)
            {
                var counterWords = labelGroup
                    .Select(g => g.words)
                    .SelectMany(x => x)
                    .GroupBy(s => s)
                    .Select(s => (s.Key, s.Count()))
                    .ToList();
                counterWords.Sort((p, n) => n.Item2.CompareTo(p.Item2));
                SaveToFile(counterWords, labelGroup.Key);
            }
        }

        private static void SaveToFile(List<(string word, int counter)> words, string label)
        {
            int howManyToSave = Math.Min(100, words.Count);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "countedWords");
            Directory.CreateDirectory(filePath);
            string path = Path.Combine(filePath, label);
            using (var writer = new StreamWriter(new FileStream(Path.ChangeExtension(path, "words"), FileMode.OpenOrCreate)))
            {
                writer.WriteLine("---- ----- ----");
                foreach (var countedWord in words.Take(howManyToSave))
                {
                    writer.WriteLine($"{countedWord.word} {countedWord.counter}");
                }
                writer.WriteLine("---- ----- ----");
                foreach (var countedWord in words.Skip(words.Count - howManyToSave))
                {
                    writer.WriteLine($"{countedWord.word} {countedWord.counter}");
                }
            }
        }
    }
}
