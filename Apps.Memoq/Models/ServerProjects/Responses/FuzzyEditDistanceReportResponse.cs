using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class FuzzyEditDistanceReportResponse
    {
        [Display("Report ID")]
        public Guid? ReportId { get; set; }

        [Display("Total")]
        public FuzzyEditDistanceResult? Total { get; set; }

        [Display("By language")]
        public FuzzyEditDistanceResultForLang[]? ByLanguage { get; set; }
    }
}
