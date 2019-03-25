namespace Core.Models.Concrete
{
    public class DataSample : IDataSample
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
