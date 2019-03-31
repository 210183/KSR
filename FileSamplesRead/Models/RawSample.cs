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

        public RawSample(RawSample sample, string newBody)
        {
            Value = new ArticleSample(
                sample.Value.Title,
                sample.Value.Dateline,
                newBody);
            Labels = sample.Labels;
        }

        public ArticleSample Value { get; }
        public LabelsCollection Labels { get; }
    }
}
