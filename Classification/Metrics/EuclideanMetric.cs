using System;
using System.Collections.Generic;
using System.Linq;

namespace Classification.Metrics
{
    public class EuclideanMetric : IMetric
    {
        public double Distance(IReadOnlyList<double> first, IReadOnlyList<double> second)
        {
            return Math.Sqrt(
                first
                    .Zip(second, (f, s) => Math.Pow(f - s, 2.0))
                    .Sum());
        }
    }
}
