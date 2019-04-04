using Classification.Metrics;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Classification.Tests
{
    public class MetricsTests
    {
        private IMetric _metric;

        [TestCase(MetricType.Manhattan, 0.0)]
        [TestCase(MetricType.Euclidean, 0.0)]
        [TestCase(MetricType.Chebyshev, 0.0)]
        public void Both_Zero(MetricType type, double correctDistance)
        {
            TestDistance(type,
                correctDistance,
                new Dictionary<string, double>()
                {
                    {"eald", 0},
                    {"loquacious", 0},
                },
                new Dictionary<string, double>()
                {
                    {"eald", 0},
                    {"loquacious", 0},
                });
        }

        [TestCase(MetricType.Manhattan, 40.4)]
        [TestCase(MetricType.Euclidean, 37.4)]
        [TestCase(MetricType.Chebyshev, 37.3)]
        public void One_Zero(MetricType type, double correctDistance)
        {
            TestDistance(type,
                correctDistance,
                new Dictionary<string, double>()
                {
                    {"eald", 0},
                    {"loquacious", 0},
                },
                new Dictionary<string, double>()
                {
                    {"eald", 3.1},
                    {"loquacious", -37.3},
                });
        }

        [TestCase(MetricType.Manhattan, 6)]
        [TestCase(MetricType.Euclidean, 5.099)]
        [TestCase(MetricType.Chebyshev, 5)]
        public void Both_NonZero(MetricType type, double correctDistance)
        {
            TestDistance(type,
                correctDistance,
                new Dictionary<string, double>()
                {
                    {"eald", 2},
                    {"loquacious", 4},
                },
                new Dictionary<string, double>()
                {
                    {"eald", -3},
                    {"loquacious", 5},
                });
        }

        private void TestDistance(MetricType type,
            double correctDistance,
            IReadOnlyDictionary<string,double> firstAttributes,
            IReadOnlyDictionary<string, double> secondAttributes)
        {
            With_Metric(type);

            double distance = _metric.Distance(firstAttributes, secondAttributes);

            distance.Should().BeApproximately(correctDistance, 0.1);
        }

        private void With_Metric(MetricType type)
        {
            switch (type)
            {
                case MetricType.Manhattan:
                    _metric = new ManhattanMetric();
                    return;
                case MetricType.Euclidean:
                    _metric = new EuclideanMetric();
                    return;
                case MetricType.Chebyshev:
                    _metric = new ChebyshevMetric();
                    return;
                default:
                    throw new NotImplementedException("Unknown metric type");
            }
        }
    }
}