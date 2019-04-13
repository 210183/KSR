using DataPreprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeywordsExtraction
{
    public static class KeywordExtractor
    {
        public static void Extract(List<PreProcessedSample> samples)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.DirectoryName);
            var dir = Directory.CreateDirectory(filePath);
            dir.EnumerateFiles().ToList().ForEach(f => f.Delete());

            var labelGroups = samples.GroupBy(s => s.Labels.Values.First().Name);
            foreach (var labelGroup in labelGroups)
            {
                var counterWords = labelGroup
                    .Select(g => g.PreProcessedArticleSample.Body)
                    .SelectMany(x => x)
                    .GroupBy(s => s)
                    .Select(s => (s.Key, s.Count()))
                    .ToList();
                counterWords.Sort((p, n) => n.Item2.CompareTo(p.Item2));
                SaveToFile(counterWords, Path.Combine(filePath, labelGroup.Key));
            }
        }

        private static void SaveToFile(List<(string word, int counter)> words, string path)
        {
            int howManyToSave = Math.Min(100, words.Count);

            using (var writer = new StreamWriter(new FileStream(Path.ChangeExtension(path, Constants.FileExtensions), FileMode.Create)))
            {
                foreach (var (word, counter) in words.Take(howManyToSave))
                {
                    writer.WriteLine($"{word} {counter}");
                }
                writer.WriteLine(Constants.Separator);
                foreach (var (word, counter) in words.Skip(words.Count - howManyToSave))
                {
                    writer.WriteLine($"{word} {counter}");
                }
            }
        }
    }
}
