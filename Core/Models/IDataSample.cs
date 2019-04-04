using Core.Models.Concrete;

namespace Core.Models
{
    public interface IDataSample
    {
        AttributesDictionary Attributes { get; }
        LabelsCollection Labels { get; }
    }
}