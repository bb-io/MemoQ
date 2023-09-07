using Apps.Memoq.Models.Dto;
using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Memoq.Models.Files.Responses;

public class DownloadFileResponse : FileDto
{
    public DownloadFileResponse(FileDto file) : base(file)
    {
    }

    [Display("Document")] public File File { get; set; }
}