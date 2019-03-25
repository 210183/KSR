using MathNet.Numerics.LinearAlgebra;

namespace Statistics.Models
{
    public class ConfusionMatrix
    {
        public ConfusionMatrix(Matrix<double> value)
        {
            Value = value;
        }

        public Matrix<double> Value { get; }
    }
}
