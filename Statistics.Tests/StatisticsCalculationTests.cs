using NUnit.Framework;
using Statistics.Calculation;
using Statistics.Models;
using System.Collections;

namespace Statistics.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        [Test, TestCaseSource(typeof(TestCaseSource), nameof(TestCaseSource.AccuracySources))]
        public double CalculateAccuracyTest(ConfusionMatrix matrix)
        {
            return Calculator.CalculateAccuracy(matrix);
        }

        [TestCaseSource(typeof(TestCaseSource), nameof(TestCaseSource.PrecisionSources))]
        public double CalculatePrecisionTest(ConfusionMatrix matrix)
        {
            return Calculator.CalculatePrecision(matrix);
        }

        [TestCaseSource(typeof(TestCaseSource), nameof(TestCaseSource.RecallSources))]
        public double CalculateRecallTest(ConfusionMatrix matrix)
        {
            return Calculator.CalculateRecall(matrix);
        }
    }

    public class TestCaseSource
    {
        private static TestCaseData tc1 => 
            new TestCaseData(MatrixSource.GetSingleCorrectlyAssigned())
                .SetName("Single correct");

        private static TestCaseData tc2 =>
            new TestCaseData(MatrixSource.GetTwoOneIncorrectlyAssigned())
                .SetName("One of two incorrect");

        private static TestCaseData tc3 =>
            new TestCaseData(MatrixSource.GetTwoIncorrectlyAssigned())
                .SetName("Two incorrect");

        public static IEnumerable AccuracySources
        {
            get
            {
                yield return tc1.SetName($"Accuracy {tc1.TestName}").Returns(1d);
                yield return tc2.SetName($"Accuracy {tc2.TestName}").Returns(0.5d);
                yield return tc3.SetName($"Accuracy {tc3.TestName}").Returns(0d);
            }
        }

        public static IEnumerable PrecisionSources
        {
            get
            {
                yield return tc1.SetName($"Precision {tc1.TestName}").Returns(1d);
                yield return tc2.SetName($"Precision {tc2.TestName}").Returns(0.75d);
                yield return tc3.SetName($"Precision {tc3.TestName}").Returns(0d);
            }
        }

        public static IEnumerable RecallSources
        {
            get
            {
                yield return tc1.SetName($"Recall {tc1.TestName}").Returns(1d);
                yield return tc2.SetName($"Recall {tc2.TestName}").Returns(0.75d);
                yield return tc3.SetName($"Recall {tc3.TestName}").Returns(0d);
            }
        }
    }
}
