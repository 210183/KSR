using System.Collections.Generic;

namespace Core.Models
{
    public interface IOrderedAttributes
    {
        IReadOnlyList<string> Names { get; }
        IReadOnlyList<double> Values { get; }
    }
}