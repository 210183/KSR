using Classification.Exceptions;
using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Classification.Models
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
