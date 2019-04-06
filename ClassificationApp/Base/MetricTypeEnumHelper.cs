using System;
using System.Collections.Generic;
using System.Linq;
using Classification.Metrics;

namespace ClassificationApp.Base
{
    public static class MetricTypeEnumHelper
    {
        public static IEnumerable<string> GetAllValues(Type t)
        {
            if (t != typeof(MetricType))
                throw new ArgumentException($"{nameof(t)} must be an enum type");

            return Enum.GetValues(t).Cast<MetricType>().Select(e => e.ToString()).ToList();
        }
    }
}