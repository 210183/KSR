using Core.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class OrderedAttributes
    {
        public OrderedAttributes(IReadOnlyList<double> attributes, IReadOnlyList<string> attributesNamesInOrder)
        {
            if (attributes?.Any() == true
                && attributesNamesInOrder != null)
            {
                Names = attributesNamesInOrder;
                Values = attributes;
            }
            else
            {
                throw new AttributesException("Non empty, named attributes vector is required. ");
            }
        }

        public IReadOnlyList<string> Names { get; }
        public IReadOnlyList<double> Values { get; }
    }
}
