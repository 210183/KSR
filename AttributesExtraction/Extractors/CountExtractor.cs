using Core.Models.Concrete;
using DataPreprocessing;
using System.Collections.Generic;
using System.Linq;

namespace AttributesExtraction.Extractors
{
    public class CountExtractor : IAttributeExtractor
    {
        public List<string> KeyValues { get; set; }
        public string AttributeName { get; set; }


        public CountExtractor(List<string> keyValues, string attributeName)
        {
            KeyValues = keyValues;
            AttributeName = attributeName;

        }

        public List<DataSample> Extract(List<PreProcessedSample> samples)
            => samples.Select(s => new DataSample(
                new AttributesDictionary(
                    new Dictionary<string, double>
                    {
                        {AttributeName, s.Words.Count(w => KeyValues.Contains(w))}
                    }),
                s.LabelsCollection)).ToList();
    }
}
