using System;
using System.Collections.Generic;
using System.Linq;

namespace Classification.Metrics
{
    public class ManhattanMetric : IMetric
    {
        public double Distance(IReadOnlyList<double> first, IReadOnlyList<double> second)
        {
            return first
                .Zip(second, (f, s) => Math.Abs(f - s))
                .Sum();
        }
    }
}
