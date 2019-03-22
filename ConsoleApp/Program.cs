using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AttributesExtraction;
using AttributesExtraction.Extractors;
using Core.Models;

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
            investment or planned output.";


            IAttributeExtractor extractor = new CountExtractor(
                new List<string>()
                {
                    "Soviet"
                },
                "SovietCount");

            var attributes = extractor.Extract(text.Split(null).ToList());

            DataSample sample = new DataSample();


        }
    }
}
