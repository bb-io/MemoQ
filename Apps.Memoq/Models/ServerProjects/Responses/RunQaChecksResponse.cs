using Apps.MemoQ.Models.Dto;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.MemoQ.Models.ServerProjects.Responses;

public record RunQaChecksResponse(FileReference? DetailedReport, IEnumerable<QaReportPerDocumentDto>? QaReportsPerDocument)
{
    [Display("Detailed report")]
    public FileReference? DetailedReport { get; set; } = DetailedReport;

    [Display("QA reports per document")]
    public IEnumerable<QaReportPerDocumentDto>? QaReportsPerDocument { get; set; } = QaReportsPerDocument;
}
