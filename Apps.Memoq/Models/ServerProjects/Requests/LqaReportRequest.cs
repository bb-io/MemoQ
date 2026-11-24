using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.Models.ServerProjects.Requests;

public class LqaReportRequest
{
    [Display("Document IDs")]
    public List<string>? DocumentIds { get; set; }

    [Display("Export format", Description = "Format of the exported report. Select None to return only structured report data.")]
    [StaticDataSource(typeof(LqaExportFormatDataHandler))]
    public string? ExportFormat { get; set; }

    [Display("CSV separator", Description = "Separator used when the export format is CSV.")]
    [StaticDataSource(typeof(CsvSeparatorDataHandler))]
    public string? CsvSeparator { get; set; }
}
