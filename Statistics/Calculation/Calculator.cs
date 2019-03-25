using Core.Models;
using Core.Models.Concrete;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using Statistics.Models;
using System.Collections.Generic;
using System.Linq;

namespace Statistics.Calculation
{
    /// <summary>
    /// Assumes given Matrix is square. If not, may throw exception of any type.
    /// </summary>
    public static class Calculator
    {
        public static ConfusionMatrix CalculateConfusionMatrix(IReadOnlyList<IClassifiedDataSample> classified)
        {
            List<(Label assigned, Label actual)> reducedSamples = classified
                .Select(d => (d.AssignedLabels.Values.First(), d.Labels.Values.First()))
                .ToList();

            var uniqueLabels = reducedSamples
                .Select(d => d.actual.Name)
                .Distinct()
                .ToList();
            uniqueLabels.Sort();
            int numberOfClasses = uniqueLabels.Count;

            var indexDictionary = Enumerable.Range(0, numberOfClasses)
                .Zip(uniqueLabels, (i, label) => (i, label))
                .ToDictionary(v => v.Item2, v => v.Item1);

            Matrix<double> result = Matrix<double>.Build.Dense(numberOfClasses, numberOfClasses);

            foreach (var sample in reducedSamples)
            {
                result[
                    indexDictionary[sample.actual.Name],
                    indexDictionary[sample.assigned.Name]
                ]++;
            }

            return new ConfusionMatrix(result);
        }

        public static double CalculateAccuracy(ConfusionMatrix matrix) 
            => TotalTruePositives(matrix.Value) / TotalTestCases(matrix.Value);

        public static double CalculatePrecision(ConfusionMatrix matrix)
        {
            double ttp = TotalTruePositives(matrix.Value);
            List<double> classPrecisions = new List<double>();
            for (int i = 0; i < matrix.Value.RowCount; i++)
            {
                classPrecisions.Add(ttp / (ttp + TotalFalsePositives(matrix.Value, i)));
            }

            return classPrecisions.Mean();

        }

        public static double CalculateRecall(ConfusionMatrix matrix)
        {
            double ttp = TotalTruePositives(matrix.Value);
            List<double> classPrecisions = new List<double>();
            for (int i = 0; i < matrix.Value.RowCount; i++)
            {
                classPrecisions.Add(ttp / (ttp + TotalFalsePositives(matrix.Value, i)));
            }

            return classPrecisions.Mean();
        }

        public static double CalculateSpecificity(ConfusionMatrix matrix)
        {
            return 0;
        }

        private static double TotalTruePositives(Matrix<double> matrix)
            => matrix.Diagonal().Sum();

        private static double TotalFalsePositives(Matrix<double> matrix, int index)
            => matrix.Row(index).Sum() - matrix[index,index];

        private static double TotalFalseNegatives(Matrix<double> matrix, int index)
            => matrix.Column(index).Sum() - matrix[index, index];

        private static double TotalTestCases(Matrix<double> matrix)
            => matrix.RowSums().Sum();
    }
}
