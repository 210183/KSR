using System.Collections.Generic;

namespace Core.Models.Concrete
{
    public class LabelsCollection : ILabelsCollection
    {
        public LabelsCollection(IReadOnlyList<Label> labels)
        {
            Values = labels;
        }

        public IReadOnlyList<Label> Values { get; set; }
    }
}
