using System;
using System.Collections.Generic;

namespace Core.Models.Concrete
{
    public class AttributesDictionary : IAttributesDictionary
    {
        public AttributesDictionary(IReadOnlyDictionary<string, double> attributes)
        {
            Values = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        public IReadOnlyDictionary<string, double> Values { get; }
    }
}
