using System.Collections.Generic;

namespace Core.Models
{
    public interface IAttributesDictionary
    {
        IReadOnlyDictionary<string, double> Values { get; }
    }
}