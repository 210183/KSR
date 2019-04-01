using Core.Models.Concrete;
using Statistics.Calculation;
using Statistics.Models;
using System.Collections.Generic;

namespace ClassificationApp.ViewModels
{
    public class ResultsViewModel
    {
        private string[][] _confusionMatrixTable;
        private double _accuracy;
        private double _precision;
        private double _specificity;
        private double _recall;

        public ResultsViewModel(IReadOnlyList<IClassifiedDataSample> classified)
        {
            var confusionMatrix = Calculator.CalculateConfusionMatrix(classified);
            _accuracy = Calculator.CalculateAccuracy(confusionMatrix);
            _precision = Calculator.CalculatePrecision(confusionMatrix);
            _specificity = Calculator.CalculateSpecificity(confusionMatrix);
            _recall = Calculator.CalculateRecall(confusionMatrix);
            StringifyMatrix(confusionMatrix);
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

        public string[][] ConfusionMatrixTable
        {
            get { return _confusionMatrixTable; }
        }

        private void StringifyMatrix(ConfusionMatrix matrix)
        {
            _confusionMatrixTable = new string[matrix.Value.RowCount+1][];
            ConfusionMatrixTable[0] = new string[matrix.Value.ColumnCount + 1];
            ConfusionMatrixTable[0][0] = "";

            for (int i = 1; i < matrix.Value.RowCount+1; i++)
            {
                ConfusionMatrixTable[i] = new string[matrix.Value.ColumnCount+1];
                for (int j = 1; j < matrix.Value.ColumnCount+1; j++)
                {
                    ConfusionMatrixTable[i][j] = matrix.Value[i-1,j-1].ToString();
                }
            }

            for (int i = 1; i < matrix.Value.RowCount+1; i++)
            {
                ConfusionMatrixTable[0][i] = matrix.Labels[i-1];
            }
            for (int i = 1; i < matrix.Value.RowCount + 1; i++)
            {
                ConfusionMatrixTable[i][0] = matrix.Labels[i-1];
            }
        }
    }
}
