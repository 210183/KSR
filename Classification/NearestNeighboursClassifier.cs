using System;
using System.Collections.Generic;
using System.Linq;
using Classification.Metrics;
using Core.Models;

namespace Classification
{
    public static class NearestNeighboursClassifier
    {
        public static DataSample Classify(
            List<double> newSampleAttributes,
            List<DataSample> classifiedSamples,
            int neighboursCount,
            IMetric metric)
        {

            var distances = classifiedSamples
                .Select(s => metric.Distance(s.Attributes, newSampleAttributes))
                .ToList();

            distances.Sort();

            distances = distances.Take(neighboursCount).ToList();

        }
    }
}
