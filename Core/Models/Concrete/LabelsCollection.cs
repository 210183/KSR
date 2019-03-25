using Core.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models.Concrete
{
    public class LabelsCollection : ILabelsCollection
    {
        public LabelsCollection(IReadOnlyList<Label> labels)
        {
            if (labels?.Any() == true)
            {
                Values = labels;
            }
            else
            {
                throw new LabelsException("Non empty attributes vector is required. ");
            }
        }

        public IReadOnlyList<Label> Values { get; }
    }
}
