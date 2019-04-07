namespace Statistics.Models
{
    public class MatrixStatistics
    {
        public MatrixStatistics(double accuracy, double precision, double recall, double specificity)
        {
            Accuracy = accuracy;
            Precision = precision;
            Recall = recall;
            Specificity = specificity;
        }

        public double Accuracy { get; }
        public double Precision { get; }
        public double Recall { get; }
        public double Specificity { get; }
    }
}
