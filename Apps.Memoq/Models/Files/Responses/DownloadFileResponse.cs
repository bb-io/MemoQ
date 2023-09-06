using Apps.Memoq.Models.Dto;
using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Memoq.Models.Files.Responses;

public class DownloadFileResponse : FileDto
{
    public DownloadFileResponse(FileDto file)
    {
        Guid = file.Guid;
        ParentDocumentId = file.ParentDocumentId;
        Name = file.Name;
        Status = file.Status;
        ExportPath = file.ExportPath;
        TargetLanguageCode = file.TargetLanguageCode;
    }

    [Display("Document")]
    public File File { get; set; }
}