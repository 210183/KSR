using Core.Exceptions;
using Core.Models.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class SamplesCollection
    {
        public SamplesCollection(List<DataSample> samples)
        {
            if (samples?.Any() == true)
            {
                Samples = samples;
            }
            else
            {
                throw new SamplesException("Non empty samples are required. ");
            }
        }

        public List<DataSample> Samples { get; }
    }
}
