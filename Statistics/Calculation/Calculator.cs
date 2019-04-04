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
                .Concat(reducedSamples
                    .Select(d => d.assigned.Name))
                .Distinct()
                .ToList();
            uniqueLabels.Sort();
            int numberOfClasses = uniqueLabels.Count;

            var indexDictionary = Enumerable.Range(0, numberOfClasses)
                .Zip(uniqueLabels, (i, label) => (i, label))
                .ToDictionary(v => v.label, v => v.i);

            Matrix<double> result = Matrix<double>.Build.Dense(numberOfClasses, numberOfClasses);

            foreach (var (assigned, actual) in reducedSamples)
            {
                result[
                    indexDictionary[actual.Name],
                    indexDictionary[assigned.Name]
                ]++;
            }

            return new ConfusionMatrix(result, uniqueLabels);
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
                classPrecisions.Add(ttp / (ttp + TotalFalseNegatives(matrix.Value, i)));
            }

            return classPrecisions.Mean();
        }

        public static double CalculateSpecificity(ConfusionMatrix matrix)
        {
            if (matrix.Value.RowCount == 1)
            {
                return matrix.Value[0, 0];
            }
            double ttn = TotalTrueNegativesAll(matrix.Value);
            List<double> classSpecificity = new List<double>();
            for (int i = 0; i < matrix.Value.RowCount; i++)
            {
                classSpecificity.Add(ttn / (ttn + TotalFalsePositives(matrix.Value, i)));
            }

            return classSpecificity.Mean();
        }

        private static double TotalTruePositives(Matrix<double> matrix)
            => matrix.Diagonal().Sum();

        private static double TotalFalsePositives(Matrix<double> matrix, int rowIndex)
            => matrix.Row(rowIndex).Sum() - matrix[rowIndex,rowIndex];

        private static double TotalFalseNegatives(Matrix<double> matrix, int columnIndex)
            => matrix.Column(columnIndex).Sum() - matrix[columnIndex, columnIndex];

        private static double TotalTrueNegativesAll(Matrix<double> matrix)
            => Enumerable.Range(0, matrix.RowCount)
                .Select(i => TotalTrueNegatives(matrix, i))
                .Sum();

        private static double TotalTrueNegatives(Matrix<double> matrix, int index)
            => matrix.ColumnSums().Sum()
               - matrix.Column(index).Sum()
               - matrix.Row(index).Sum()
               + matrix[index, index];


        private static double TotalTestCases(Matrix<double> matrix)
            => matrix.RowSums().Sum();
    }
}
