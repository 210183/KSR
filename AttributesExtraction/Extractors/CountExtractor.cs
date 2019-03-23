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
            this.KeyValues = keyValues;
            this.AttributeName = attributeName;

        }

        public (string name, double count) Extract(List<string> words)
        {
            return (AttributeName, words.Count(w => KeyValues.Contains(w)));

        }
    }
}
