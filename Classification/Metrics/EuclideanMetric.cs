using System;
using System.Collections.Generic;
using System.Linq;

namespace Classification.Metrics
{
    public class EuclideanMetric : IMetric
    {
        public double Distance(IReadOnlyDictionary<string, double> first, IReadOnlyDictionary<string, double> second)
        {
            return Math.Sqrt(first.Keys
                .ToList()
                .Concat(second.Keys)
                .Distinct()
                .Select(k =>
                {
                    first.TryGetValue(k, out double f);
                    second.TryGetValue(k, out double s);
                    return Math.Pow(f - s, 2.0);
                })
                .Sum());
        }
    }
}
