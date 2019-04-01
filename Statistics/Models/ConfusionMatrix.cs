using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace Statistics.Models
{
    public class ConfusionMatrix
    {
        public ConfusionMatrix(Matrix<double> value, IReadOnlyList<string> labels)
        {
            Value = value;
            Labels = labels;
        }

        public Matrix<double> Value { get; }
        public IReadOnlyList<string> Labels { get; }
    }
}
