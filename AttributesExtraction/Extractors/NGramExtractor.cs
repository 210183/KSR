using Core.Models.Concrete;
using DataPreprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttributesExtraction.Extractors
{
    class NGramExtractor
    {
        public NGramExtractor(string n)
        {
            N = n ?? throw new ArgumentNullException(nameof(n));
        }

        public string N { get; }

        public List<DataSample> Extract(List<PreProcessedSample> samples)
        {

        }
            
            
            => samples.Select(s => new DataSample(
                    new AttributesDictionary(
                        new Dictionary<string, double>(
                            KeyValues
                            .ToDictionary(
                                kv => kv,
                                kv => (double)s.PreProcessedArticleSample.Body.Count(
                                    w => w.Equals(kv, StringComparison.InvariantCulture))))),
                    s.Labels)
            ).ToList();
    }
}
