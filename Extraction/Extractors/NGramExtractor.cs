using Core.Models.Concrete;
using DataPreprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttributesExtraction.Extractors
{
    class NGramExtractor
    {
        public NGramExtractor(int n)
        {
            N = n;
        }

        public int N { get; }

        public List<DataSample> Extract(List<PreProcessedSample> samples)
        {
            HashSet<string> allNgrams = new HashSet<string>();
            for (int i = 0; i < samples.Count; i++)
            {
                List<string> sample = samples[i].PreProcessedArticleSample.Body;
                ReadOnlySpan<string> sampleSpan = sample.ToArray().AsSpan();
                for (int j = 0; j < sample.Count - N; j++)
                {
                    var slice = sampleSpan.Slice(j, N);
                    for (int k = 0; k < slice.Length; k++)
                    {

                    }
                }
            }

            return null;
        }
            
            //=> samples.Select(s => new DataSample(
            //        new AttributesDictionary(
            //            new Dictionary<string, double>(
            //                KeyValues
            //                .ToDictionary(
            //                    kv => kv,
            //                    kv => (double)s.PreProcessedArticleSample.Body.Count(
            //                        w => w.Equals(kv, StringComparison.InvariantCulture))))),
            //        s.Labels)
            //).ToList();
    }
}
