using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace AttributesExtraction
{
    public interface IAttributeExtractor
    {
        (string name, double count) Extract(List<string> words);
    }
}
