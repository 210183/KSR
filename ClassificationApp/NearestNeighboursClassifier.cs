using Classification.Metrics;
using Core.Models;
using Core.Models.Concrete;
using System.Linq;

namespace Classification
{
    public static class NearestNeighboursClassifier
    {
        public static ClassifiedDataSample Classify(
            DataSample newSample,
            SamplesCollection classifiedSamples,
            int neighboursCount,
            IMetric metric)
        {

            var distances = classifiedSamples.Samples
                .Select(s => new
                {
                    labels = s.Labels,
                    distane = metric.Distance(s.Attributes.Values, newSample.Attributes.Values)
                })
                .ToList();

            distances.Sort((p, n) => p.distane.CompareTo(n.distane));

            distances = distances.Take(neighboursCount).ToList();

            //TODO: implement k usage
            return new ClassifiedDataSample(
                newSample.Attributes,
                newSample.Labels,
                distances.First().labels
                );
        }
    }
}
