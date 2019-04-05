using System;
using System.Collections.Generic;
using System.Linq;
using FileSamplesRead.Models;

namespace DataPreprocessing
{
    public class StopWordsFilter
    {
        private readonly HashSet<string> _stopWordsSet;

        public StopWordsFilter(IReadOnlyList<string> stopWords)
        {
            _stopWordsSet = new HashSet<string>(stopWords, StringComparer.InvariantCultureIgnoreCase);
        }

        public PreProcessedSample Filter(RawSample input)
            => new PreProcessedSample(
                input.Labels,
                new PreProcessedArticleSample(
                    input.Value.Title,
                    input.Value.Dateline,
                    input.Value.Body.Split(new[] { ' ', '\t', '\r', '\n', ',', ';', '.' },
                            StringSplitOptions.RemoveEmptyEntries)
                            .Where(w => !_stopWordsSet.Contains(w)
                            && !double.TryParse(w, out _)
                            && w.Length > 1
                            && w != "--"
                            ).ToList()
                    ));

    }

}
