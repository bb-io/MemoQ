using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.Models.Files.Requests;

public class DocumentExistsRequest
{
    [Display("File ID")] public string? Guid { get; set; }

    [Display("File name")] public string? DocumentName { get; set; }

    [Display("Target language code"), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public string? TargetLanguageCode { get; set; }

    [Display("External file ID")] public string? ExternalDocumentId { get; set; }

    [Display("Web translation URL")] public string? WebTransUrl { get; set; }

    public bool IsValid()
    {
        return Guid != null || DocumentName != null || TargetLanguageCode != null || ExternalDocumentId != null ||
               WebTransUrl != null;
    }
}