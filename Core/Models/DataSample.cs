using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class DataSample
    {
        public IReadOnlyList<double> Attributes { get; }
        public IReadOnlyList<string> AttributesNamesInOrder { get; }
        public IReadOnlyList<string> Labels { get; }
    }
}
