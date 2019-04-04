using System;
using System.Collections.Generic;
using System.Text;
using Core.Models.Concrete;

namespace DataPreprocessing
{
    public class PreProcessedSample
    {
        public PreProcessedSample(LabelsCollection labelsCollection, List<string> words)
        {
            LabelsCollection = labelsCollection ?? throw new ArgumentNullException(nameof(labelsCollection));
            Words = words ?? throw new ArgumentNullException(nameof(words));
        }

        public LabelsCollection LabelsCollection { get; set; }
        public List<string> Words { get; set; }


    }
}
