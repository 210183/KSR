using System;
using System.Collections.Generic;
using System.Linq;

namespace Classification.Metrics
{
    public class ChebyshevMetric : IMetric
    {
        public double Distance(IReadOnlyList<double> first, IReadOnlyList<double> second)
        {
            return first
                .Zip(second, (f, s) => Math.Abs(f - s))
                .Max();
        }
    }
}
