using Core.Models.Concrete;
using DataPreprocessing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace AttributesExtraction.Extractors
{
    public class NGramExtractor : IAttributeExtractor
    {
        public NGramExtractor(int n)
        {
            N = n;
        }

        public int N { get; }

        public List<DataSample> Extract(List<PreProcessedSample> samples)
        {
            ConcurrentBag<DataSample> result = new ConcurrentBag<DataSample>();
            samples
                    .ToImmutableList()
                    .AsParallel()
                    .ForAll(s =>
                    {
                        var sample = s.PreProcessedArticleSample.Body;
                        var attributesDictionary = new Dictionary<string, double>(50);
                        for (int j = 0; j < sample.Count - N; j++)
                        {
                            StringBuilder builder = new StringBuilder(N * 6);
                            for (int k = j; k < j + N; k++)
                            {
                                builder.Append(sample[k]);
                            }
                            string ngram = builder.ToString();
                            if (attributesDictionary.ContainsKey(ngram))
                            {
                                attributesDictionary[ngram] = attributesDictionary[ngram] + 1;
                            }
                            else
                            {
                                attributesDictionary[ngram] = 1;
                            }
                        }
                        result.Add(new DataSample(
                            new AttributesDictionary(attributesDictionary),
                            s.Labels));
                    });
            return result.ToList();
        }
    }
}
