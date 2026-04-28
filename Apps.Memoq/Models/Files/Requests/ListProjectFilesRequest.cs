using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.Models.Files.Requests;

public class ListProjectFilesRequest
{
    [Display("Fill in assignment information")]
    public bool? FillInAssignmentInformation { get; set; }

    [Display("Target language"), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public string? TargetLanguageCode { get; set; }
}