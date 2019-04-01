using Core.Models.Concrete;
using Statistics.Calculation;
using Statistics.Models;
using System.Collections.Generic;

namespace ClassificationApp.ViewModels
{
    public class ResultsViewModel
    {
        private ConfusionMatrix _confusionMatrix;
        private double _accuracy;
        private double _precision;
        private double _specificity;
        private double _recall;

        public ResultsViewModel(IReadOnlyList<IClassifiedDataSample> classified)
        {
            _confusionMatrix = Calculator.CalculateConfusionMatrix(classified);
            _accuracy = Calculator.CalculateAccuracy(Matrix);
            _precision = Calculator.CalculatePrecision(Matrix);
            _specificity = Calculator.CalculateSpecificity(Matrix);
            _recall = Calculator.CalculateRecall(Matrix);
        }

        public double Recall
        {
            get { return _recall; }
        }

        public double Specificity
        {
            get { return _specificity; }
        }

        public double Precision
        {
            get { return _precision; }
        }

        public double Accuracy
        {
            get { return _accuracy; }
        }

        public ConfusionMatrix Matrix
        {
            get { return _confusionMatrix; }
        }
    }
}
