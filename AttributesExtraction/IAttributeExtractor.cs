using System.Collections.Generic;

namespace AttributesExtraction
{
    public interface IAttributeExtractor
    {
        (string name, double count) Extract(List<string> words);
    }
}
