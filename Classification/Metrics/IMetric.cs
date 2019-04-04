using System.Collections.Generic;

namespace Classification.Metrics
{
    public interface IMetric
    {
        double Distance(IReadOnlyDictionary<string, double> first, IReadOnlyDictionary<string, double> second);
    }
}
