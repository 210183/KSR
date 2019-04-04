using Core;
using DataPreprocessing;
using FileSamplesRead;
using KeywordsExtraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = @"Sandoz AG said it planned a joint venture
            to produce herbicides in the Soviet Union.
                The company said it had signed a letter of intent with the
            Soviet Ministry of Fertiliser Production to form the first
                foreign joint venture the ministry had undertaken since the
            Soviet Union allowed Western firms to enter into joint ventures
            two months ago.
                The ministry and Sandoz will each have a 50 pct stake, but
            a company spokeswoman was unable to give details of the size of
            investment or planned output.
            Soviet Soviet Soviet";

            var filter = new StopWordsFilter(WellKnownNames.StopWords);
            var stemmer = new PorterStemmer();
            //Classification(text);

            var dataReader = new DataSamplesReader();
            List<FileSamplesRead.Models.RawSample> samples = dataReader.ReadAllSamples("C:\\Users\\Mateusz\\Desktop\\reuters_przetworzone\\reut2-004.sgm", "places");
            var filtered = samples
                .Select(s => (s.Labels, filter.Filter(s.Value.Body)))
                //.Select(f => (f.Item1,
                //        f.Item2
                //        .Select(w => stemmer.StemWord(w))))
                .ToList();

            KeywordExtractor.Extract(filtered);

            Console.Read();
        }

        private static void Classification(string text)
        {
            //IAttributeExtractor extractor = new CountExtractor(
            //    new List<string>()
            //    {
            //        "Soviet"
            //    },
            //    "SovietCount");

            //(string name, double attributeCount) = extractor.Extract(text.Split(null).ToList());

            //var newClassifiedSample = NearestNeighboursClassifier.Classify(
            //    new AttributesDictionary(
            //        new List<double> { attributeCount },
            //        new List<string> { name }),
            //    new SamplesCollection(new List<DataSample>()
            //    {
            //        new DataSample(
            //            new AttributesDictionary(
            //                new List<double>() {1},
            //                new List<string>() {"SovietCount"}
            //            ),
            //            new LabelsCollection(new List<Label>() {new Label("A bit soviet")})),
            //        new DataSample(
            //            new AttributesDictionary(
            //                new List<double> {2},
            //                new List<string> {"SovietCount"}
            //            ),
            //            new LabelsCollection(new List<Label> {new Label("More soviet")})),
            //        new DataSample(
            //            new AttributesDictionary(
            //                new List<double> {4},
            //                new List<string> {"SovietCount"}
            //            ),
            //            new LabelsCollection(new List<Label> {new Label("Любимый сын Матери России")}))
            //    }),
            //    4,
            //    new ManhattanMetric()
            //);
            //newClassifiedSample.Labels.Values.Select(l => l.Name).ToList().ForEach(Console.WriteLine);
        }
    }
}
