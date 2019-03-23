namespace Core.Models
{
    public class DataSample
    {
        public DataSample(OrderedAttributes attributes,
            LabelsCollection labels)
        {
            Attributes = attributes;
            Labels = labels;
        }

        public OrderedAttributes Attributes { get; }
        public LabelsCollection Labels { get; }
    }
}
