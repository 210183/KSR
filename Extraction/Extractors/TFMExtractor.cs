using Core.Models.Concrete;
using DataPreprocessing;
using System.Collections.Generic;
using System.Linq;

namespace AttributesExtraction.Extractors
{
    public class TFMExtractor : IAttributeExtractor
    {
        public List<DataSample> Extract(List<PreProcessedSample> samples)
            => samples.Select(s => new DataSample(
                new AttributesDictionary(
                    s.PreProcessedArticleSample.Body
                        .GroupBy(w => w)
                        .ToDictionary(g => g.Key, g => (double)g.Count())),
                s.Labels)).ToList();
    }
}
