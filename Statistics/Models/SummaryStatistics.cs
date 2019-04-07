using Core.Models;
using System.Collections.Generic;

namespace Statistics.Models
{
    public class SummaryStatistics
    {
        public SummaryStatistics(Models.MatrixStatistics summary, Dictionary<Label, Models.MatrixStatistics> labelsStatistics)
        {
            Summary = summary;
            LabelsStatistics = labelsStatistics;
        }

        public Models.MatrixStatistics Summary { get; }
        public Dictionary<Label, Models.MatrixStatistics> LabelsStatistics { get; }
    }
}
