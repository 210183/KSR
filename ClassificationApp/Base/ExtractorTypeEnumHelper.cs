using AttributesExtraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassificationApp.Base
{
    public static class ExtractorTypeEnumHelper
    {
        public static IEnumerable<string> GetAllValues(Type t)
        {
            if (t != typeof(ExtractorType))
                throw new ArgumentException($"{nameof(t)} must be an enum type");

            return Enum.GetValues(t).Cast<ExtractorType>().Select(e => e.ToString()).ToList();
        }
    }
}
