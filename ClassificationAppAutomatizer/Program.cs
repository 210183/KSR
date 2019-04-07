using AttributesExtraction;
using Classification.Metrics;
using ClassificationApp.ViewModels;
using ClosedXML.Excel;
using Statistics.Calculation;
using Statistics.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassificationAppAutomatizer
{
    internal class Program
    {
        private const int K_Min = 5;
        private const int K_Step = 123;
        private const int K_Max = 5;
        private const int ColdStart_Min = 20;
        private const int ColdStart_Step = 10;
        private const int ColdStart_Max = 100;
        private const int SamplesToClassify_Min = 150;
        private const int SamplesToClassify_Step = 150;
        private const int SamplesToClassify_Max = 150;
        private const int ClassificationTries_Max = 5;
        private const int KeywordsExtractionTries = 1;
        private const bool saveToExcel = true;
        private static List<MatrixStatistics> _statistics;
        private static List<MetricType> _metricTypes = new List<MetricType>()
        {
            //MetricType.Manhattan,
            MetricType.Euclidean,
            //MetricType.Chebyshev,
            //MetricType.Cosin,
        };

        private static List<ExtractorType> _extractorTypes = new List<ExtractorType>()
        {
            //ExtractorType.NGram,
            //ExtractorType.TFMWords,
            ExtractorType.TFMKeyWords,
        };

        private static void Main(string[] args)
        {
            _statistics = new List<MatrixStatistics>();
            MainViewModel context = new MainViewModel();

            context.NForNGram = 3;
            context.LabelName = "TOPICS";
            context.ShouldUseJsonDataFile = false;
            context.IsDataSgm = true;

            context.LoadFilesCommand.Execute(null);
            for (int i = SamplesToClassify_Min; i <= SamplesToClassify_Max; i += SamplesToClassify_Step)
            {
                context.SamplesToClassify = i;
                TestForMetricType(context, Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "results"), $"samples_{i}"));
            }

            if (saveToExcel)
                SaveSummaryToExcel(Path.Combine(Directory.GetCurrentDirectory(), "summary"));
        }

        private static void TestForMetricType(MainViewModel context, string filePath)
        {
            for (int t = 0; t < KeywordsExtractionTries; t++)
            {
                foreach (var extractor in _extractorTypes)
                {
                    context.ExtractorType = extractor;
                    context.ExtractCommand.Execute(null);
                    TestForSamplesAmount(context, Path.Combine(filePath, $"extractor_{extractor}"));
                }
            }
        }

        private static void TestForSamplesAmount(MainViewModel context, string filePath)
        {
            for (int k = K_Min; k <= K_Max; k += K_Step)
            {
                context.NearestNeighboursNumber = k;
                TestForK(context, Path.Combine(filePath, $"k_{k}"));
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
            foreach (var type in _metricTypes)
            {
                context.MetricType = type;
                TestForExtractoryType(context, Path.Combine(filePath, $"metric_{type}"));
            }
        }


        private static void TestForExtractoryType(MainViewModel context, string filePath)
        {
            List<ConfusionMatrix> results = new List<ConfusionMatrix>();
            for (int classificationTries = 0; classificationTries < ClassificationTries_Max; classificationTries++)
            {
                context.ClassifyCommand.Execute(null);
                results.Add(Calculator.CalculateConfusionMatrix(context.ConcurrentBagOfClassifiedSamples.ToList()));
            }

            if(saveToExcel)
                SaveResultsToExcel(MeanOfMatrix(results), Path.Combine(filePath, $"tries_{ClassificationTries_Max}"));
            else
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
        
        private static void SaveResultsToExcel(ConfusionMatrix result, string filePath)
        {
            //Directory.CreateDirectory(filePath);

            //DataTable table = new DataTable();
            //table.Columns.Add("Accuracy");
            //table.Columns.Add("Precision");
            //table.Columns.Add("Recall");
            //table.Columns.Add("Specificity");

            double accuracy = Calculator.CalculateAccuracy(result);
            double precision = Calculator.CalculatePrecision(result);
            double recall = Calculator.CalculateRecall(result);
            double specificity = Calculator.CalculateSpecificity(result);
            _statistics.Add(new MatrixStatistics(accuracy, precision, recall, specificity));

            //table.Rows.Add(accuracy.ToString("F2"),
            //        precision.ToString("F2"),
            //        recall.ToString("F2"),
            //        specificity.ToString("F2"));  
            
            //DataTable tableResults = new DataTable();

            //for (int i=0; i<result.Labels.Count; i++)
            //{
            //    tableResults.Columns.Add(result.Labels[i]);
            //}
            
            //var rawResult = result.Value;
            //for (int i=0; i<rawResult.RowCount; i++)
            //{
            //    DataRow dataRow = tableResults.NewRow();
                
            //    for (int j=0; j<rawResult.ColumnCount; j++)
            //    {
            //        dataRow[result.Labels[j]] = result.Value[i, j];
            //    }
            //    tableResults.Rows.Add(dataRow);
            //}

            
            //XLWorkbook workbook = new XLWorkbook();
            //var wb1 = workbook.Worksheets.Add(table, "Statistics").SetTabColor(XLColor.Amber);
            //var wb2 = workbook.Worksheets.Add(tableResults, "Matrix").SetTabColor(XLColor.Aqua);
            
            //wb1.ColumnWidth = 14;
            //wb2.ColumnWidth = 14;
            //workbook.SaveAs(Path.Combine(filePath, "results.xlsx"));
        }


        private static void SaveSummaryToExcel(string filePath)
        {
            Directory.CreateDirectory(filePath);

            DataTable table = new DataTable();
            table.Columns.Add("Cold start");
            table.Columns.Add("Accuracy");
            table.Columns.Add("Precision");
            table.Columns.Add("Recall");
            table.Columns.Add("Specificity");

            int i = 0;
            //for (int k = K_Min; k <= K_Max; k += K_Step)
            //{
            //    table.Rows.Add(
            //        k,
            //        _statistics[i].Accuracy.ToString("F2"),
            //        _statistics[i].Precision.ToString("F2"),
            //        _statistics[i].Recall.ToString("F2"),
            //        _statistics[i].Specificity.ToString("F2"));
            //    i++;
            //}
            for (int k = ColdStart_Min; k <= ColdStart_Max; k += ColdStart_Step)
            {
                table.Rows.Add(
                    k,
                    _statistics[i].Accuracy.ToString("F2"),
                    _statistics[i].Precision.ToString("F2"),
                    _statistics[i].Recall.ToString("F2"),
                    _statistics[i].Specificity.ToString("F2"));
                i++;
            }
            //for (int j = SamplesToClassify_Min; j <= SamplesToClassify_Max; j += SamplesToClassify_Step)
            //{
            //    table.Rows.Add(
            //        j,
            //        _statistics[i].Accuracy.ToString("F2"),
            //        _statistics[i].Precision.ToString("F2"),
            //        _statistics[i].Recall.ToString("F2"),
            //        _statistics[i].Specificity.ToString("F2"));
            //    i++;
            //}

            //for (int t = 0; t < KeywordsExtractionTries; t++)
            //{
            //    table.Rows.Add(
            //        t,
            //        _statistics[i].Accuracy.ToString("F2"),
            //        _statistics[i].Precision.ToString("F2"),
            //        _statistics[i].Recall.ToString("F2"),
            //        _statistics[i].Specificity.ToString("F2"));
            //    i++;
            //}

            XLWorkbook workbook = new XLWorkbook();
            var wb1 = workbook.Worksheets.Add(table, "Statistics").SetTabColor(XLColor.Amber);
            wb1.ColumnWidth = 14;
            workbook.SaveAs(Path.Combine(filePath, "summary.xlsx"));
        }
    }
}
