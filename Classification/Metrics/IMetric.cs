using System.Collections.Generic;

namespace Classification.Metrics
{
    public interface IMetric
    {
        double Distance(IReadOnlyList<double> first, IReadOnlyList<double> second);
    }
}
