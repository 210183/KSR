using MathNet.Numerics.LinearAlgebra;
using Statistics.Models;
using System.Collections.Generic;

namespace Statistics.Tests
{
    public static class MatrixSource
    {
        public static ConfusionMatrix GetSingleCorrectlyAssigned(IReadOnlyList<string> labels)
            => new ConfusionMatrix(
                Matrix<double>.Build.DenseOfArray(
                    new double[1, 1]
                    {
                        { 1 }
                    }),
                labels);

        public static ConfusionMatrix GetTwoIncorrectlyAssigned(IReadOnlyList<string> labels)
            => new ConfusionMatrix(
                Matrix<double>.Build.DenseOfArray(
                    new double[2, 2]
                    {
                        { 0, 1 },
                        { 1, 0 }
                    }),
                labels);

        public static ConfusionMatrix GetTwoOneIncorrectlyAssigned(IReadOnlyList<string> labels)
            => new ConfusionMatrix(
                Matrix<double>.Build.DenseOfArray(
                    new double[2, 2]
                    {
                        { 1, 0 },
                        { 1, 0 }
                    }),
                labels);
    }
}
