﻿using Core.Models.Concrete;
using DataPreprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttributesExtraction.Extractors
{
    public class KeyWordsExtractor : IAttributeExtractor
    {
        public List<string> KeyValues { get; set; }

        public KeyWordsExtractor(List<string> keyValues)
        {
            KeyValues = keyValues;
        }

        public List<DataSample> Extract(List<PreProcessedSample> samples)
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
