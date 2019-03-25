using Core.Models.Concrete;

namespace Core.Models
{
    public interface IDataSample
    {
        OrderedAttributes Attributes { get; }
        LabelsCollection Labels { get; }
    }
}