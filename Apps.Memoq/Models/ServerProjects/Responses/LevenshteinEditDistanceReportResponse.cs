using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class LevenshteinEditDistanceReportResponse
    {
        [Display("Report ID")]
        public Guid? ReportId { get; set; }

        [Display("Total")]
        public LevenshteinEditDistanceResult? Total { get; set; }

        [Display("By language")]
        public LevenshteinEditDistanceResultForLang[]? ByLanguage { get; set; }
    }
}
