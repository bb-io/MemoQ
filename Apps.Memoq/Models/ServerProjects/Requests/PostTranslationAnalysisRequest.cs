using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.Models.ServerProjects.Requests
{
    public class PostTranslationAnalysisRequest
    {
        [Display("Documents IDs")]
        public List<string>? DocumentIds { get; set; }

        [Display("Language codes")]
        [StaticDataSource(typeof(TargetLanguageDataHandler))]
        public List<string>? LanguageCodes { get; set; }

        [Display("Note")]
        public string? Note { get; set; }

        [Display("Repetition preference over 100%")]
        public bool? RepetitionPreferenceOver100 { get; set; }

        [Display("Store report in project")]
        public bool? StoreReportInProject { get; set; }

        [Display("Tag weight (chars)")]
        public double? TagWeightChar { get; set; }

        [Display("Tag weight (words)")]
        public double? TagWeightWord { get; set; }
    }
}
