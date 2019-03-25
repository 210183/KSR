using System.Collections.Generic;

namespace Core.Models
{
    public interface ILabelsCollection
    {
        IReadOnlyList<Label> Values { get; }
    }
}