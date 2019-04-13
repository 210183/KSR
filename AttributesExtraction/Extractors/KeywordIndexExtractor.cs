using Core.Models.Concrete;
using DataPreprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttributesExtraction.Extractors
{
    public class KeywordIndexExtractor : IAttributeExtractor
    {
        public List<string> KeyValues { get; set; }

        public KeywordIndexExtractor(List<string> keyValues)
        {
            KeyValues = keyValues;
        }

        public List<DataSample> Extract(List<PreProcessedSample> samples)
            => samples.Select(s => new DataSample(
                new AttributesDictionary(
                    new Dictionary<string, double>(
                        KeyValues
                            .ToDictionary(
                                kv => $"{kv}_index",
                                kv => (double)s.PreProcessedArticleSample.Body.FindIndex(
                                    w => w.Equals(kv, StringComparison.InvariantCulture))))),
                s.Labels)
            ).ToList();
    }
}
