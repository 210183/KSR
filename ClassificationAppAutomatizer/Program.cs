using AttributesExtraction;
using Classification.Metrics;
using ClassificationApp.ViewModels;
using Statistics.Calculation;
using Statistics.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassificationAppAutomatizer
{
    internal class Program
    {
        private const int K_Max = 5;
        private const int ColdStart_Min = 100;
        private const int ColdStart_Step = 20;
        private const int ColdStart_Max = 301;
        private const int SamplesToClassify = 1000;
        private const int ClassificationTries_Max = 5;
        private const int KeywordsExtractionTries = 1;

        private static void Main(string[] args)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "results");
            MainViewModel context = new MainViewModel();

            context.SamplesToClassify = SamplesToClassify;
            context.LabelName = "places";
            context.ShouldUseJsonDataFile = true;
            context.IsDataSgm = false;

            context.LoadFilesCommand.Execute(null);

            for (int k = 4; k <= K_Max; k++)
            {
                context.NearestNeighboursNumber = k;
                TestForK(context, Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "results"), $"k_{k}"));
            }
        }

        private static void TestForK(MainViewModel context, string filePath)
        {
            for (int coldStart = ColdStart_Min; coldStart <= ColdStart_Max; coldStart += ColdStart_Step)
            {
                context.ColdStartSamples = coldStart;
                TestForColdStart(context, Path.Combine(filePath, $"cold_{coldStart}"));
            }
        }

        private static void TestForColdStart(MainViewModel context, string filePath)
        {
            context.MetricType = MetricType.Manhattan;
            TestForMetricType(context, Path.Combine(filePath, $"metric_{MetricType.Manhattan}"));

            context.MetricType = MetricType.Euclidean;
            TestForMetricType(context, Path.Combine(filePath, $"metric_{MetricType.Euclidean}"));

            context.MetricType = MetricType.Chebyshev;
            TestForMetricType(context, Path.Combine(filePath, $"metric_{MetricType.Chebyshev}"));
        }

        private static void TestForMetricType(MainViewModel context, string filePath)
        {
            context.ExtractorType = ExtractorType.NGram;
            TestForExtractoryType(context, Path.Combine(filePath, $"extractor_{ ExtractorType.NGram}"));

            context.ExtractorType = ExtractorType.TFMWords;
            TestForExtractoryType(context, Path.Combine(filePath, $"extractor_{ ExtractorType.TFMWords}"));
        }

        private static void TestForExtractoryType(MainViewModel context, string filePath)
        {
            context.ExtractCommand.Execute(null);
            List<ConfusionMatrix> results = new List<ConfusionMatrix>();
            for (int classificationTries = 0; classificationTries < ClassificationTries_Max; classificationTries++)
            {
                context.ClassifyCommand.Execute(null);
                results.Add(Calculator.CalculateConfusionMatrix(context.ConcurrentBagOfClassifiedSamples.ToList()));
            }
            SaveResults(MeanOfMatrix(results), Path.Combine(filePath, $"tries_{ClassificationTries_Max}"));
        }

        private static ConfusionMatrix MeanOfMatrix(List<ConfusionMatrix> matrices)
        {
            int maxSize = matrices.Select(m => m.Value.RowCount).Max();
            var labelsGroups = matrices.Where(m => m.Value.RowCount == maxSize)
                .GroupBy(m => m.Labels)
                .Select(g => new {group = g, count = g.Count()});
            var chosenGroup = labelsGroups
                .Where(g => g.count == labelsGroups.Max(m => m.count))
                .Select(g => g.group)
                .First();

            return new ConfusionMatrix(
                chosenGroup.Select(c => c.Value).Aggregate((p, n) => p + n)
                    .Divide(chosenGroup.Count()),
                matrices.First().Labels
                );
        }

        private static void SaveResults(ConfusionMatrix result, string filePath)
        {
            Directory.CreateDirectory(filePath);
            using (var writer = new StreamWriter(new FileStream(Path.Combine(filePath, "results.txt"), FileMode.Create)))
            {
                var rawResult = result.Value;
                writer.WriteLine($"Accuracy: {Calculator.CalculateAccuracy(result).ToString("F2")}");
                writer.WriteLine($"Precision: {Calculator.CalculatePrecision(result).ToString("F2")}");
                writer.WriteLine($"Recall: {Calculator.CalculateRecall(result).ToString("F2")}");
                writer.WriteLine($"Specificity: {Calculator.CalculateSpecificity(result).ToString("F2")}");
                writer.WriteLine(result.Labels.Aggregate((p, n) => $"{p} {n}"));
                for (int i = 0; i < rawResult.RowCount; i++)
                {
                    StringBuilder rowBuilder = new StringBuilder();
                    rawResult.Row(i).ToList().ForEach(v => rowBuilder.Append($"{v.ToString("F2")} "));
                    writer.WriteLine(rowBuilder.ToString());
                }
            }
        }
    }
}
