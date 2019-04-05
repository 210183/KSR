using Core.Models.Concrete;

namespace DataPreprocessing
{
    public class PreProcessedSample
    {
        public PreProcessedSample(LabelsCollection labels, PreProcessedArticleSample preProcessedArticleSample)
        {
            Labels = labels;
            PreProcessedArticleSample = preProcessedArticleSample;
        }

        public LabelsCollection Labels { get; set; }
        public PreProcessedArticleSample PreProcessedArticleSample { get; set; }


    }
}
