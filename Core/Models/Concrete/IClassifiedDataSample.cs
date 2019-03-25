namespace Core.Models.Concrete
{
    public interface IClassifiedDataSample : IDataSample
    {
        LabelsCollection AssignedLabels { get; set; }
    }
}