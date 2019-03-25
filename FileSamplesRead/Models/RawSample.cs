using Core.Models.Concrete;

namespace FileSamplesRead.Models
{
    public class RawSample
    {
        public RawSample(ArticleSample value, LabelsCollection labels)
        {
            Value = value;
            Labels = labels;
        }

        public ArticleSample Value { get; }
        public LabelsCollection Labels { get; }
    }
}
