﻿using System.Collections.Generic;

namespace DataPreprocessing
{
    public class PreProcessedArticleSample
    {
        public PreProcessedArticleSample(string title, string dateline, List<string> body)
        {
            Title = title;
            Dateline = dateline;
            Body = body;
        }

        public string Title { get; }
        public string Dateline { get; }
        public List<string> Body { get; }
    }
}
