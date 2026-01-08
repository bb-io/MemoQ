using MQS.ServerProject;
using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.Dto;

public class QaReportPerDocumentDto(QAReportForDocument sdkQaReport)
{
    [Display("Document ID")]
    public string DocumentId { get; set; } = sdkQaReport.DocumentGuid.ToString();

    [Display("Document name")]
    public string DocumentName { get; set; } = sdkQaReport.DocumentName;

    [Display("Number of errors")]
    public int NumberOfErrors { get; set; } = sdkQaReport.NumberOfErrors;

    [Display("Number of unignored warnings")]
    public int NumberOfUnignoredWarnings { get; set; } = sdkQaReport.NumberOfUnignoredWarnings;

    [Display("Number of warnings")]
    public int NumberOfWarnings { get; set; } = sdkQaReport.NumberOfWarnings;

    [Display("Target language")]
    public string TargetLanguage { get; set; } = sdkQaReport.TargetLanguage;
}
