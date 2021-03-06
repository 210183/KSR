﻿using Classification.Metrics;
using Core.Models;
using Core.Models.Concrete;
using System.Collections.Generic;
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

            var groups = distances
                .GroupBy(a => a.labels.Values.First().Name);
            var max = groups.Max(g => g.Count());
            var assignedLabel = groups
                .Where(g => g.Count() == max)
                .Select(g => new
                {
                    label = g.Key,
                    avgDistance = g.Select(p => p.distane).Average()
                })
                .OrderBy(g => g.avgDistance)
                .First().label;
                
            return new ClassifiedDataSample(
                newSample.Attributes,
                newSample.Labels,
                new LabelsCollection(new List<Label> { new Label(assignedLabel) })
                );
        }
    }
}
