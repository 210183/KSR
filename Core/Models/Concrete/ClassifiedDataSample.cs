namespace Core.Models.Concrete
{
    public class ClassifiedDataSample : DataSample, IClassifiedDataSample
    {
        public ClassifiedDataSample(OrderedAttributes attributes, LabelsCollection labels, LabelsCollection assignedLabels) : base(attributes, labels)
        {
            AssignedLabels = assignedLabels;
        }

        public LabelsCollection AssignedLabels { get; set; }
    }
}
