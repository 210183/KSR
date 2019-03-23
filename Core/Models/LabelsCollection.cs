using Core.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class LabelsCollection
    {
        public LabelsCollection(IReadOnlyList<string> labels)
        {
            if (labels?.Any() == true)
            {
                Labels = labels;
            }
            else
            {
                throw new LabelsException("Non empty attributes vector is required. ");
            }
        }

        public IReadOnlyList<string> Labels { get; }
    }
}
