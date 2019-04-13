using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeywordsExtraction
{
    public static class KeywordsSelector
    {
        public static List<string> LoadKeywords(int howManyPopular, int howManyUnpopular)
        {
            Random randomizer = new Random();
            List<string> keywords = new List<string>();

            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.DirectoryName);
            var files = Directory.GetFiles(directoryPath).Where(p => Path.GetExtension(p) == Constants.FileExtensions).ToList();
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file).ToList();
                int separatorIndex = lines.FindIndex(l => l.Equals(Constants.Separator, StringComparison.Ordinal));
                var popularWords = lines.Take(separatorIndex);
                var unpopularWords = lines.Skip(separatorIndex+1);

                keywords.AddRange(popularWords.OrderBy(e => randomizer.Next()).Take(Math.Min(popularWords.Count(), howManyPopular)).Select(l => l.Split()[0]));
                keywords.AddRange(unpopularWords.OrderBy(e => randomizer.Next()).Take(Math.Min(unpopularWords.Count(), howManyUnpopular)).Select(l => l.Split()[0]));
            }

            return keywords.Distinct().ToList();
        }
    }
}
