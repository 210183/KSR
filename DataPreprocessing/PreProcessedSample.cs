using System;
using System.Collections.Generic;
using System.Text;
using Core.Models.Concrete;

namespace DataPreprocessing
{
    public class PreProcessedSample
    {
        public PreProcessedSample(LabelsCollection labels, PreProcessedArticleSample preProcessedArticleSample)
        {
            Labels = labels ?? throw new ArgumentNullException(nameof(labels));
            PreProcessedArticleSample = preProcessedArticleSample ?? throw new ArgumentNullException(nameof(preProcessedArticleSample));
        }

        public LabelsCollection Labels { get; set; }
        public PreProcessedArticleSample PreProcessedArticleSample { get; set; }


    }
}
