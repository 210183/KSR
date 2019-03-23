﻿using AttributesExtraction;
using AttributesExtraction.Extractors;
using Classification;
using Classification.Metrics;
using Classification.Models;
using Core.Models;
using FileSamplesRead;
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

            //Classification(text);

            var dataReader = new DataSamplesReader();
            var samples = dataReader.ReadAllSamples("C:\\Users\\Mateusz\\Desktop\\reuters\\reut2-001.sgm");

            Console.Read();
        }

        private static void Classification(string text)
        {
            IAttributeExtractor extractor = new CountExtractor(
                new List<string>()
                {
                    "Soviet"
                },
                "SovietCount");

            (string name, double attributeCount) = extractor.Extract(text.Split(null).ToList());

            var newClassifiedSample = NearestNeighboursClassifier.Classify(
                new OrderedAttributes(
                    new List<double> { attributeCount },
                    new List<string> { name }),
                new SamplesCollection(new List<DataSample>()
                {
                    new DataSample(
                        new OrderedAttributes(
                            new List<double>() {1},
                            new List<string>() {"SovietCount"}
                        ),
                        new LabelsCollection(new List<string>() {"A bit soviet"})),
                    new DataSample(
                        new OrderedAttributes(
                            new List<double> {2},
                            new List<string> {"SovietCount"}
                        ),
                        new LabelsCollection(new List<string> {"More soviet"})),
                    new DataSample(
                        new OrderedAttributes(
                            new List<double> {4},
                            new List<string> {"SovietCount"}
                        ),
                        new LabelsCollection(new List<string> {"Любимый сын Матери России"}))
                }),
                4,
                new ManhattanMetric()
            );
            newClassifiedSample.Labels.Labels.ToList().ForEach(Console.WriteLine);
        }
    }
}
