using Core.Models;
using System.Collections.Generic;

namespace Statistics.Models
{
    public class SummaryStatistics
    {
        public SummaryStatistics(Models.Statistics summary, Dictionary<Label, Models.Statistics> labelsStatistics)
        {
            Summary = summary;
            LabelsStatistics = labelsStatistics;
        }

        public Models.Statistics Summary { get; }
        public Dictionary<Label, Models.Statistics> LabelsStatistics { get; }
    }
}
