using Core.Exceptions;
using Core.Models.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class SamplesCollection
    {
        public SamplesCollection(IReadOnlyList<DataSample> samples)
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

        public IReadOnlyList<DataSample> Samples { get; }
    }
}
