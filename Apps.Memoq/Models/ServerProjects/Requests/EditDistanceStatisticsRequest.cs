using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.Models.ServerProjects.Requests
{
    public class EditDistanceStatisticsRequest
    {
        [Display("Calculate for slices")]
        public bool? CalculateForSlices { get; set; }

        [Display("Documents IDs")]
        public List<string>? DocumentIds { get; set; }

        [Display("Language codes")]
        [StaticDataSource(typeof(TargetLanguageDataHandler))]
        public List<string>? LanguageCodes { get; set; }

        [Display("Store repoкt in project")]
        public bool? StoreReportInProject { get; set; }

        [Display("Word count mode")]
        [StaticDataSource(typeof(AnalysisAlgorithmDataHandler))]
        public string? WordCountMode { get; set; }
    }
}
