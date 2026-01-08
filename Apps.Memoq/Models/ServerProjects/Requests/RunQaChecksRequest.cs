using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.Models.ServerProjects.Requests;

public class RunQaChecksRequest
{
    [StaticDataSource(typeof(QaReportTypeDataHandler))]
    [Display("Report type")]
    public string ReportType { get; set; }

    [Display("Include locked rows", Description = "If false, all locked rows are skipped and not checked")]
    public bool IncludeLockedRows { get; set; }

    [DataSource(typeof(DocumentDataHandler))]
    [Display("Document IDs", Description = 
        "The IDs of the documents for which QA should be executed. Empty value means to run QA on the entire project")]
    public IEnumerable<string>? DocumentGuids { get; set; }

    [StaticDataSource(typeof(TwoLetterLanguageCodesDataHandler))]
    [Display("Report display language",
        Description = 
            "2-letter region code and optional country code. " +
            "English is the default if the input is missing or unavailable")]
    public string? ReportLanguage { get; set; }
}
