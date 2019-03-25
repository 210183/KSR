using MathNet.Numerics.LinearAlgebra;
using Statistics.Models;

namespace Statistics.Tests
{
    public static class MatrixSource
    {
        public static ConfusionMatrix GetSingleCorrectlyAssigned()
            => new ConfusionMatrix(
                Matrix<double>.Build.DenseOfArray(
                    new double[1, 1]
                    {
                        { 1 }
                    }));

        public static ConfusionMatrix GetTwoIncorrectlyAssigned()
            => new ConfusionMatrix(
                Matrix<double>.Build.DenseOfArray(
                    new double[2, 2]
                    {
                        { 0, 1 },
                        { 1, 0 }
                    }));

        public static ConfusionMatrix GetTwoOneIncorrectlyAssigned()
            => new ConfusionMatrix(
                Matrix<double>.Build.DenseOfArray(
                    new double[2, 2]
                    {
                        { 1, 0 },
                        { 1, 0 }
                    }));
    }
}
