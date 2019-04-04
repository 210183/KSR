using Core.Models.Concrete;
using DataPreprocessing;
using System.Collections.Generic;
using System.Linq;

namespace AttributesExtraction.Extractors
{
    public class TFTExtractor : IAttributeExtractor
    {
        public List<DataSample> Extract(List<PreProcessedSample> samples)
            => samples.Select(s => new DataSample(
                new AttributesDictionary(
                    s.Words
                        .GroupBy(w => w)
                        .ToDictionary(g => g.Key, g => (double)g.Count())),
                s.LabelsCollection)).ToList();
    }
}
