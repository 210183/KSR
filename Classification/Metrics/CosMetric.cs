using System;
using System.Collections.Generic;
using System.Linq;

namespace Classification.Metrics
{
    public class CosMetric : IMetric
    {
        public double Distance(IReadOnlyDictionary<string, double> first, IReadOnlyDictionary<string, double> second)
        {
            double numerator = 0, d1 = 0, d2 = 0;

            first.Keys
                .ToList()
                .Concat(second.Keys)
                .Distinct()
                .ToList()
                .ForEach(k =>
                {
                    first.TryGetValue(k, out double f);
                    second.TryGetValue(k, out double s);
                    numerator += f * s;
                    d1 += f * f;
                    d2 += s * s;
                });
            return Math.Abs(numerator) / Math.Sqrt(d1 + d2);
        }
    }
}
