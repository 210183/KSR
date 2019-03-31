using System;
using System.Collections.Generic;
using System.Linq;

namespace DataPreprocessing
{
    public class StopWordsFilter
    {
        private readonly HashSet<string> _stopWordsSet;

        public StopWordsFilter(IReadOnlyList<string> stopWords)
        {
            _stopWordsSet = new HashSet<string>(stopWords, StringComparer.InvariantCultureIgnoreCase);
        }

        public IEnumerable<string> Filter(string input)
            => input.Split(new[] { ' ', '\t', '\r', '\n', ',', ';', '.'}, StringSplitOptions.RemoveEmptyEntries)
                .Where(w => !_stopWordsSet.Contains(w) && w.Length > 1);
        
    }

}
