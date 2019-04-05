using Core.Models.Concrete;
using DataPreprocessing;
using System.Collections.Generic;

namespace AttributesExtraction
{
    public interface IAttributeExtractor
    {
        List<DataSample> Extract(List<PreProcessedSample> samples);
    }
}
