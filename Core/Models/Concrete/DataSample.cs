namespace Core.Models.Concrete
{
    public class DataSample : IDataSample
    {
        public DataSample(AttributesDictionary attributes,
            LabelsCollection labels)
        {
            Attributes = attributes;
            Labels = labels;
        }

        public AttributesDictionary Attributes { get; }
        public LabelsCollection Labels { get; }
    }
}
