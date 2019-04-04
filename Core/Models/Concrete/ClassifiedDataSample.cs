namespace Core.Models.Concrete
{
    public class ClassifiedDataSample : DataSample, IClassifiedDataSample
    {
        public ClassifiedDataSample(AttributesDictionary attributes, LabelsCollection labels, LabelsCollection assignedLabels) : base(attributes, labels)
        {
            AssignedLabels = assignedLabels;
        }

        public LabelsCollection AssignedLabels { get; set; }
    }
}
